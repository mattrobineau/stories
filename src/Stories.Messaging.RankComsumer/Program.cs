using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stories.Data.DbContexts;
using Stories.Messaging.Configuration;
using Stories.Messaging.Constants;
using Stories.Messaging.Providers;
using Stories.Messaging.Services;
using Stories.Ranking.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Linq;

namespace Stories.Messaging.RankComsumer
{
    class Program
    {
        static IServiceProvider ServiceProvider;
        static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Rank Consumer");
            try
            {
                ConfigureServices();
                var dbContext = ServiceProvider.GetService<StoriesDbContext>();
                var rankService = ServiceProvider.GetService<IRankService>();
                var provider = ServiceProvider.GetService<IMessageService>();
                var logger = ServiceProvider.GetService<ILogger<Program>>();

                IModel model = null;
                try
                {
                    model = provider.GetModel();
                }
                catch (Exception e)
                {
                    //logger.LogCritical($"Unable to get connection to RabbitMQ: {e.ToString()}", e.ToString());
                    return;
                }

                model.BasicQos(0, 20, false);

                while (true)
                {
                    var handledIds = new List<int>();

                    for (var i = 0; i < 20; i++)
                    {
                        var result = model.BasicGet(RabbitMQQueues.Stories, autoAck: false); // NoAck aka, True to auto ack message, false to noack..

                        if (result == null)
                            break;

                        try
                        {
                            int id = Convert.ToInt32(Encoding.UTF8.GetString(result.Body));

                            if (handledIds.Contains(id))
                                continue;

                            var updated = Task.Run(async () => { return await UpdateStoryRank(dbContext, rankService, id); });

                            handledIds.Add(id);

                            if (!updated.Result)
                            {
                                logger.LogWarning($"Failed to update. StoryId = {id}");
                                model.BasicReject(result.DeliveryTag, true);
                                continue;
                            }

                            model.BasicAck(result.DeliveryTag, false);
                        }
                        catch (Exception e)
                        {
                            //logger.LogCritical($"Story -- Exception attempting to handle message body: {result.Body}", e);
                            model.BasicReject(result.DeliveryTag, false);
                        }
                    }

                    logger.LogDebug($"Updated story scores for {handledIds.Count}: {string.Join(",", handledIds.Select(x => x.ToString()).ToArray())}");
                    handledIds.Clear();

                    for (var i = 0; i < 20; i++)
                    {
                        var result = model.BasicGet(RabbitMQQueues.Comments, autoAck: false);

                        if (result == null)
                            break;

                        try
                        {
                            int id = Convert.ToInt32(Encoding.UTF8.GetString(result.Body));

                            if (handledIds.Contains(id))
                                continue;

                            var updated = Task.Run(async () => { return await UpdateCommentRank(dbContext, rankService, id); });

                            handledIds.Add(id);

                            if (!updated.Result)
                            {
                                logger.LogWarning($"Failed to update. CommentId = {id}");
                                model.BasicReject(result.DeliveryTag, false);
                                continue;
                            }

                            model.BasicAck(result.DeliveryTag, true);
                        }
                        catch (Exception e)
                        {
                            //logger.LogCritical($"Comment -- Exception attempting to handle message body: {result.Body} -- Stack: {e.ToString()}", e);
                            model.BasicReject(result.DeliveryTag, false);
                        }
                    }
                    logger.LogDebug($"Updated comment scores for {handledIds.Count}: {string.Join(",", handledIds.Select(x => x.ToString()).ToArray())}");
                    handledIds.Clear();

                    Thread.Sleep(5 * 60 * 1000); // 5mins
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        private static async Task<bool> UpdateStoryRank(IDbContext dbContext, IRankService rankService, int storyId)
        {
            var story = await dbContext.Stories.FirstOrDefaultAsync(s => s.Id == storyId);

            rankService.CalculateStoryScore(story);

            return await dbContext.SaveChangesAsync() > 0;
        }

        private static async Task<bool> UpdateCommentRank(IDbContext dbContext, IRankService rankService, int commentId)
        {
            var comment = await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            rankService.CalculateCommentScore(comment);

            return await dbContext.SaveChangesAsync() > 0;
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

            var AmpqOptions = Configuration.GetSection("AMQPOptions").Get<AMQPOptions>();
            Configuration.Bind(AmpqOptions);

            services.AddDbContext<StoriesDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("NpgsqlConnection")));
            services.AddScoped<IDbContext>(s => s.GetService<StoriesDbContext>());
            services.AddScoped<IMessageService, RabbitMQMessageService>();

            services.AddSingleton(AmpqOptions);

            services.AddSingleton<IRabbitMQConnectionProvider, RabbitMQConnectionProvider>();
            services.AddScoped<IRankService, RankService>();

            services.AddLogging();

            ServiceProvider = services.BuildServiceProvider();

            var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            //loggerFactory.AddNLog();
            //loggerFactory.ConfigureNLog("nlog.config");
        }
    }
}