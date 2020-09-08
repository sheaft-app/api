using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Handlers;
using Sheaft.Application.Queries;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Interop;
using Sheaft.Mappers;
using Sheaft.Options;
using Sheaft.Services;
using Sheaft.Services.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Sheaft.Functions.Startup))]
namespace Sheaft.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = ConfigurationManager.BuildConfiguration(builder.Services);

            var sendgridSettings = configuration.GetSection(SendgridOptions.SETTING);
            builder.Services.Configure<SendgridOptions>(sendgridSettings);

            var databaseSettings = configuration.GetSection(DatabaseOptions.SETTING);
            builder.Services.Configure<DatabaseOptions>(databaseSettings);

            var storageSettings = configuration.GetSection(StorageOptions.SETTING);
            builder.Services.Configure<StorageOptions>(storageSettings);

            var serviceBusSettings = configuration.GetSection(ServiceBusOptions.SETTING);
            builder.Services.Configure<ServiceBusOptions>(serviceBusSettings);

            builder.Services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.SETTING));
            builder.Services.Configure<CorsOptions>(configuration.GetSection(CorsOptions.SETTING));
            builder.Services.Configure<SearchOptions>(configuration.GetSection(SearchOptions.SETTING));
            builder.Services.Configure<ApiOptions>(configuration.GetSection(ApiOptions.SETTING));
            builder.Services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SETTING));
            builder.Services.Configure<FreshdeskOptions>(configuration.GetSection(FreshdeskOptions.SETTING));
            builder.Services.Configure<LandingOptions>(configuration.GetSection(LandingOptions.SETTING));
            builder.Services.Configure<PortalOptions>(configuration.GetSection(PortalOptions.SETTING));
            builder.Services.Configure<ScoringOptions>(configuration.GetSection(ScoringOptions.SETTING));
            builder.Services.Configure<SearchOptions>(configuration.GetSection(SearchOptions.SETTING));
            builder.Services.Configure<SendgridOptions>(configuration.GetSection(SendgridOptions.SETTING));
            builder.Services.Configure<SignalrOptions>(configuration.GetSection(SignalrOptions.SETTING));
            builder.Services.Configure<SireneOptions>(configuration.GetSection(SireneOptions.SETTING));
            builder.Services.Configure<SponsoringOptions>(configuration.GetSection(SponsoringOptions.SETTING));

            builder.Services.BuildServiceProvider();

            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(UserCommandsHandler).Assembly }.ToArray());

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton(configuration);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            builder.Services.AddScoped<ISendGridClient, SendGridClient>(c => new SendGridClient(sendgridConfig.ApiKey));

            builder.Services.AddScoped<IIdentifierService, IdentifierService>();
            builder.Services.AddScoped<IQueueService, QueueService>();
            builder.Services.AddScoped<IBlobService, BlobService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISignalrService, SignalrService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IAgreementQueries, AgreementQueries>();
            builder.Services.AddScoped<ICompanyQueries, CompanyQueries>();
            builder.Services.AddScoped<IDeliveryQueries, DeliveryQueries>();
            builder.Services.AddScoped<IDepartmentQueries, DepartmentQueries>();
            builder.Services.AddScoped<IJobQueries, JobQueries>();
            builder.Services.AddScoped<ILeaderboardQueries, LeaderboardQueries>();
            builder.Services.AddScoped<INotificationQueries, NotificationQueries>();
            builder.Services.AddScoped<IPackagingQueries, PackagingQueries>();
            builder.Services.AddScoped<IProductQueries, ProductQueries>();
            builder.Services.AddScoped<IPurchaseOrderQueries, PurchaseOrderQueries>();
            builder.Services.AddScoped<IQuickOrderQueries, QuickOrderQueries>();
            builder.Services.AddScoped<IRegionQueries, RegionQueries>();
            builder.Services.AddScoped<ITagQueries, TagQueries>();
            builder.Services.AddScoped<IConsumerQueries, ConsumerQueries>();
            builder.Services.AddScoped<IUserQueries, UserQueries>();

            builder.Services.AddScoped<IDapperContext, DapperContext>();

            builder.Services.AddOptions();

            var databaseConfig = databaseSettings.Get<DatabaseOptions>();
            builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x => x.UseNetTopologySuite());
            }, ServiceLifetime.Scoped);

            builder.Services.AddLocalization(ops => ops.ResourcesPath = "Resources");
            builder.Services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("fr")
                    };

                    opts.DefaultRequestCulture = new RequestCulture("en", "fr");
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;
                });

            var commandsInQueueType = typeof(RegisterStoreCommand).Assembly.GetTypes().Where(t => t.GetFields(BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy).Any(c => c.Name == "QUEUE_NAME"));
            var eventsInQueueType = typeof(ExportUserDataSucceededEvent).Assembly.GetTypes().Where(t => t.GetFields(BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy).Any(c => c.Name == "QUEUE_NAME"));

            var queues = new List<string>();

            foreach (var type in commandsInQueueType)
            {
                var prop = type.GetField("QUEUE_NAME").GetRawConstantValue();
                queues.Add((string)prop);
            }

            foreach (var type in eventsInQueueType)
            {
                var prop = type.GetField("QUEUE_NAME").GetRawConstantValue();
                queues.Add((string)prop);
            }

            var managementClient = new Microsoft.Azure.ServiceBus.Management.ManagementClient(serviceBusSettings.Get<ServiceBusOptions>().ConnectionString);

            const int take = 100;
            var skip = 0;

            var query = true;
            var existingQueues = new List<string>();
            while (query)
            {
                var results = managementClient.GetQueuesAsync(take, skip).Result.Select(q => q.Path).ToList();
                if (results.Any())
                    existingQueues.AddRange(results);
                else
                    query = false;

                skip += take;
            }

            foreach (var queue in queues)
            {
                if (existingQueues.Contains(queue))
                    continue;

                managementClient.CreateQueueAsync(queue).Wait();
            }

            foreach (var existingQueue in existingQueues)
            {
                if (queues.Contains(existingQueue))
                    continue;

                managementClient.DeleteQueueAsync(existingQueue).Wait();
            }
        }
    }

    internal static class ConfigurationManager
    {
        public static IConfiguration BuildConfiguration(IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}