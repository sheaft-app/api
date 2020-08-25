﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Handlers;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Interop;
using Sheaft.Options;
using Sheaft.Services;
using Sheaft.Services.Interop;
using System;
using System.Collections.Generic;
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

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient();
            builder.Services.AddMediatR(new List<Assembly>() { typeof(RegisterCompanyCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(AccountCommandsHandler).Assembly }.ToArray());

            builder.Services.AddSingleton(configuration);
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            builder.Services.AddScoped<ISendGridClient, SendGridClient>(c => new SendGridClient(sendgridConfig.ApiKey));

            builder.Services.AddScoped<IIdentifierService, IdentifierService>();
            builder.Services.AddScoped<IQueueService, QueueService>();
            builder.Services.AddScoped<IBlobService, BlobService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISignalrService, SignalrService>();

            var databaseConfig = databaseSettings.Get<DatabaseOptions>();
            builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x => x.UseNetTopologySuite());
            }, ServiceLifetime.Scoped);

            var commandsInQueueType = typeof(RegisterCompanyCommand).Assembly.GetTypes().Where(t => t.GetFields(BindingFlags.Public | BindingFlags.Static |
               BindingFlags.FlattenHierarchy).Any(c => c.Name == "QUEUE_NAME"));
            var eventsInQueueType = typeof(AccountExportDataSucceededEvent).Assembly.GetTypes().Where(t => t.GetFields(BindingFlags.Public | BindingFlags.Static |
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
                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}