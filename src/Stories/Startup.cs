using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using Stories.Configuration;
using Stories.Data.DbContexts;
using Stories.Extensions;
using Stories.Messaging.Configuration;
using Stories.Messaging.Providers;
using Stories.Messaging.Services;
using Stories.Services;
using Stories.Validation.BusinessRules;
using Stories.Validation.Validators;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Stories
{
    public class Startup
    {

        private Container container = new Container();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<StoriesDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("NpgsqlConnection"), b => b.MigrationsAssembly("Stories")), ServiceLifetime.Scoped);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationScheme = "StoriesCookieAuthentication",
            //    LoginPath = new PathString("/auth/login"),
            //    AccessDeniedPath = new PathString("/auth/login"),
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    ReturnUrlParameter = "returnUrl"
            //});

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => 
                    {
                        options.LoginPath = new PathString("/auth/login");
                        options.LogoutPath = new PathString("/auth/logout");
                        options.AccessDeniedPath = new PathString("/auth/login");
                        options.ReturnUrlParameter = "returnUrl";
                    });

            services.AddMvcCore();
            services.AddMvc();
            
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/auth/login");
                options.AccessDeniedPath = new PathString("/auth/login");
                options.ReturnUrlParameter = "returnUrl";
            });

            services.UseSimpleInjectorAspNetRequestScoping(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InitializeContainer(app);

            // Todo: This has some setup to do so we can start monitoring
            // https://blogs.msdn.microsoft.com/webdev/2015/05/19/application-insights-for-asp-net-5-youre-in-control/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<StoriesDbContext>().Database.Migrate();
            }

            app.SeedData();

            app.UseStaticFiles();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "story",
                    template: "story/{hashId}/{*slug}",
                    defaults: new { controller = "Story", action = "Index" }
                    );
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services.            
            container.Register<IDbContext>(() => app.GetRequiredRequestService<StoriesDbContext>(), Lifestyle.Scoped);

            container.Register<IStoryService, StoryService>();
            container.Register<IAuthenticationService, AuthenticationService>();
            container.Register<IPasswordService, PasswordService>();
            container.Register<IUserService, UserService>();
            container.Register<ICommentService, CommentService>();
            container.Register<IVoteService, VoteService>(Lifestyle.Scoped);
            container.Register(typeof(IValidator<>), new[] { typeof(IValidator<>).GetTypeInfo().Assembly });
            container.RegisterConditional(typeof(IValidator<>), typeof(NullValidator<>), c => !c.Handled);
            container.Register<IEmailRule, EmailRule>();
            container.Register<IReferralCodeRule, ReferralCodeRule>();
            container.Register<IReferralService, ReferralService>();
            container.Register<IVoteQueueService, VoteQueueService>(Lifestyle.Scoped);
            container.Register<IBanService, BanService>(Lifestyle.Scoped);
            container.Register<IUserRules, UserRules>(Lifestyle.Scoped);
            container.Register<IStoryRules, StoryRules>(Lifestyle.Scoped);
            container.Register<IFlagService, FlagService>(Lifestyle.Scoped);

            var AmpqOptions = Configuration.GetSection("AMQPOptions").Get<AMQPOptions>();
            container.Register<IRabbitMQConnectionProvider>(() => new RabbitMQConnectionProvider(AmpqOptions), Lifestyle.Scoped);
            container.Register<IMessageService, RabbitMQMessageService>(Lifestyle.Scoped);

            MailgunOptions mailgunOptions = Configuration.GetSection("Mailgun").Get<MailgunOptions>();
            container.Register<IMailService>(() => new MailgunEmailService(mailgunOptions));

            container.Register(() => Configuration.GetSection("Invites").Get<InviteOptions>(), Lifestyle.Scoped);
        }
    }
}
