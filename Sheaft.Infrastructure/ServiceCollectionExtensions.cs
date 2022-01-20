using System;
using Amazon;
using Amazon.SimpleEmail;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sheaft.Application.Configurations;
using Sheaft.Application.Identifiers;
using Sheaft.Application.Mediator;
using Sheaft.Application.Notifications;
using Sheaft.Application.Pdf;
using Sheaft.Application.Persistence;
using Sheaft.Application.Storage;
using Sheaft.Domain;
using Sheaft.Infrastructure.Hangfire;
using Sheaft.Infrastructure.Identifiers;
using Sheaft.Infrastructure.Mediator;
using Sheaft.Infrastructure.Notifications;
using Sheaft.Infrastructure.Pdf;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Storage;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace Sheaft.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterMediator(services);
            RegisterPdfServices(services);
            RegisterEmailServices(services, configuration);
            RegisterStorageServices(services, configuration);
            RegisterHangfire(services, configuration);
            RegisterDatabaseServices(services, configuration);
            RegisterSignalr(services);
            
            RegisterRepositories(services);

            services.AddHttpClient();
            services.AddRazorTemplating();
            services.AddScoped<IIdentifierService, IdentifierService>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<Company>, CompanyRepository>();
        }

        private static void RegisterMediator(IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator.Mediator>();
            services.AddScoped<IDispatcher, Dispatcher>();
        }

        private static void RegisterPdfServices(IServiceCollection services)
        {
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        private static void RegisterEmailServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            
            var mailerConfig = configuration.GetSection(MailerConfiguration.SETTING).Get<MailerConfiguration>();
            services.AddScoped<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>(_ =>
                new AmazonSimpleEmailServiceClient(mailerConfig.ApiId, mailerConfig.ApiKey, RegionEndpoint.EUCentral1));
        }

        private static void RegisterStorageServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFileService, FileService>();
            var storageConfig = configuration.GetSection(StorageConfiguration.SETTING).Get<StorageConfiguration>();
        }

        private static void RegisterSignalr(IServiceCollection services)
        {
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.AddSignalR();
        }

        private static void RegisterDatabaseServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDapperContext, DapperContext>();
            
            services.Configure<AppDatabaseConfiguration>(configuration.GetSection(AppDatabaseConfiguration.SETTING));
            services.Configure<JobsDatabaseConfiguration>(configuration.GetSection(JobsDatabaseConfiguration.SETTING));
            
            var databaseConfig = configuration.GetSection(AppDatabaseConfiguration.SETTING).Get<AppDatabaseConfiguration>();
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("AppMigrationTable", "ef");
                });
            });
        }

        private static void RegisterHangfire(IServiceCollection services, IConfiguration configuration)
        {
            var jobsDatabaseConfig =
                configuration.GetSection(JobsDatabaseConfiguration.SETTING).Get<JobsDatabaseConfiguration>();
            
            services.AddHangfire(hangfireConfig =>
            {
                hangfireConfig.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
                
                hangfireConfig.UseSerializerSettings(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
            });

            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());
            GlobalJobFilters.Filters.Add(new LogEverythingAttribute());
            
            RecuringJobs.Register(configuration.GetSection(RoutineConfiguration.SETTING).Get<RoutineConfiguration>());
            
            services.AddHangfireServer();
        }
    }
}