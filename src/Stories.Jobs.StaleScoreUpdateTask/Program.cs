using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Stories.Data.DbContexts;
using Stories.Ranking.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Jobs.RankStories
{
    public class Program
    {
        static IServiceProvider ServiceProvider;
        static IConfigurationRoot Configuration { get; set; }
        
        static void Main(string[] args)
        {
            Console.Write("Starting update of stale scores job.");
            ConfigureServices();
            var dbContext = ServiceProvider.GetService<StoriesDbContext>();
            var rankService = ServiceProvider.GetService<IRankService>();

            Task.Run(async () => {
                // Cron job is setup to run every 12h. Date for the last 13 hours to avoid missing by minutes.
                try
                {
                    var updateLastDateTime = DateTime.UtcNow.AddHours(-13);

                    var commentScores = await dbContext.CommentScores.Include(s => s.Comment)
                                                   .Where(s => updateLastDateTime <= s.ModifiedDate)
                                                   .ToListAsync();

                    foreach (var score in commentScores)
                        rankService.CalculateCommentScore(score.Comment);

                    var storyScores = await dbContext.StoryScores.Include(s => s.Story)
                                                        .Where(s => updateLastDateTime <= s.ModifiedDate)
                                                        .ToListAsync();

                    foreach (var score in storyScores)
                        rankService.CalculateStoryScore(score.Story);

                    var rows = await dbContext.SaveChangesAsync();

                    Log.Information($"RankStories job updated {rows} scores.");
                }
                catch(Exception e)
                {
                    Log.Fatal(e, "Unknown error occured");
                }
            }).GetAwaiter().GetResult();

            return;
        }

        static void ConfigureServices()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true);

            var services = new ServiceCollection();

            Configuration = builder.Build();

            services.AddDbContext<StoriesDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("NpgsqlConnection")));
            services.AddScoped<IDbContext>(s => s.GetService<StoriesDbContext>());
            services.AddScoped<IRankService, RankService>();
            services.AddLogging();
            ServiceProvider = services.BuildServiceProvider();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }
    }
}