using AutoMapper;
using MangoPay.SDK;
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
using Sheaft.Application.Interop;
using Sheaft.Application.Mappers;
using Sheaft.Options;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Sheaft.Infrastructure.Services;
using Sheaft.Infrastructure.Persistence;

[assembly: FunctionsStartup(typeof(Sheaft.Functions.Startup))]
namespace Sheaft.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var context = builder.GetContext();
            var Configuration = context.Configuration;
            var services = builder.Services;

            var sendgridSettings = Configuration.GetSection(SendgridOptions.SETTING);
            services.Configure<SendgridOptions>(sendgridSettings);

            var databaseSettings = Configuration.GetSection(DatabaseOptions.SETTING);
            services.Configure<DatabaseOptions>(databaseSettings);

            var storageSettings = Configuration.GetSection(StorageOptions.SETTING);
            services.Configure<StorageOptions>(storageSettings);

            var serviceBusSettings = Configuration.GetSection(ServiceBusOptions.SETTING);
            services.Configure<ServiceBusOptions>(serviceBusSettings);

            var pspSettings = Configuration.GetSection(PspOptions.SETTING);
            services.Configure<PspOptions>(pspSettings);

            services.Configure<AuthOptions>(Configuration.GetSection(AuthOptions.SETTING));
            services.Configure<CorsOptions>(Configuration.GetSection(CorsOptions.SETTING));
            services.Configure<SearchOptions>(Configuration.GetSection(SearchOptions.SETTING));
            services.Configure<ApiOptions>(Configuration.GetSection(ApiOptions.SETTING));
            services.Configure<DatabaseOptions>(Configuration.GetSection(DatabaseOptions.SETTING));
            services.Configure<FreshdeskOptions>(Configuration.GetSection(FreshdeskOptions.SETTING));
            services.Configure<LandingOptions>(Configuration.GetSection(LandingOptions.SETTING));
            services.Configure<PortalOptions>(Configuration.GetSection(PortalOptions.SETTING));
            services.Configure<ScoringOptions>(Configuration.GetSection(ScoringOptions.SETTING));
            services.Configure<SearchOptions>(Configuration.GetSection(SearchOptions.SETTING));
            services.Configure<SendgridOptions>(Configuration.GetSection(SendgridOptions.SETTING));
            services.Configure<SignalrOptions>(Configuration.GetSection(SignalrOptions.SETTING));
            services.Configure<SireneOptions>(Configuration.GetSection(SireneOptions.SETTING));
            services.Configure<SponsoringOptions>(Configuration.GetSection(SponsoringOptions.SETTING));
            services.Configure<RoutineOptions>(Configuration.GetSection(RoutineOptions.SETTING));

            services.BuildServiceProvider();
            
            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(UserCommandsHandler).Assembly }.ToArray());
            
            services.AddMemoryCache();
            services.AddHttpClient();
            
            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            services.AddScoped<ISendGridClient, SendGridClient>(_ => new SendGridClient(sendgridConfig.ApiKey));

            services.AddScoped<IIdentifierService, IdentifierService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPspService, PspService>();
            services.AddScoped<IFeesService, FeesService>();
            services.AddScoped<ISheaftMediatr, SheaftMediatr>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IDapperContext, DapperContext>();

            services.AddOptions();

            var databaseConfig = databaseSettings.Get<DatabaseOptions>();
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x => x.UseNetTopologySuite());
            }, ServiceLifetime.Scoped);

            var pspOptions = pspSettings.Get<PspOptions>();
            services.AddScoped<MangoPayApi>(_ => new MangoPayApi
            {
                Config = new MangoPay.SDK.Core.Configuration
                {
                    BaseUrl = pspOptions.ApiUrl,
                    ClientId = pspOptions.ClientId,
                    ClientPassword = pspOptions.ApiKey
                }
            });

            services.AddLocalization(ops => ops.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(
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
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}