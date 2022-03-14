using Amazon;
using Amazon.SimpleEmail;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sheaft.Application;
using Sheaft.Application.ProfileManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.ProfileManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.ProfileManagement;
using Sheaft.Infrastructure.Services;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace Sheaft.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterMediator(services);
        RegisterPdfServices(services);
        RegisterEmailServices(services, configuration);
        RegisterRepositories(services, configuration);
        RegisterQueries(services, configuration);
        RegisterDatabaseServices(services, configuration);
        RegisterSignalr(services);
        RegisterServices(services);
        
        RegisterHangfire(services, configuration);

        services.AddHttpClient();
        services.AddRazorTemplating();
    }

    private static void RegisterQueries(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProfileQueries, ProfileQueries>();
    }

    private static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
    }

    private static void RegisterMediator(IServiceCollection services)
    {
        services.AddScoped<ISheaftMediator, SheaftMediator>();
        services.AddScoped<ISheaftDispatcher, SheaftDispatcher>();
    }

    private static void RegisterPdfServices(IServiceCollection services)
    {
        services.AddScoped<IPdfGenerator, PdfGenerator>();
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
    }

    private static void RegisterEmailServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();

        var mailerConfig = configuration.GetSection(MailerSettings.SETTING).Get<MailerSettings>();
        services.AddScoped<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>(_ =>
            new AmazonSimpleEmailServiceClient(mailerConfig.ApiId, mailerConfig.ApiKey, RegionEndpoint.EUCentral1));
    }

    private static void RegisterSignalr(IServiceCollection services)
    {
        services.AddScoped<ISignalrService, SignalrService>();
        services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
        services.AddSignalR();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IValidateUniqueness, ValidateUniqueness>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ISecurityTokensProvider, SecurityTokensProvider>();
        services.AddScoped<IIdentifierService, IdentifierService>();
    }

    private static void RegisterDatabaseServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var appConnectionString = configuration.GetConnectionString("AppDatabase");
        var connectionStrings = new Dictionary<DatabaseConnectionName, string>
        {
            {DatabaseConnectionName.AppDatabase, appConnectionString}
        };

        services.AddSingleton<IDictionary<DatabaseConnectionName, string>>(connectionStrings);
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        services.AddDbContext<IDbContext, AppDbContext>(c =>
        {
            c.UseSqlServer(appConnectionString, builder =>
            {
                builder.EnableRetryOnFailure(2);
                builder.CommandTimeout(30);
            });
        }, ServiceLifetime.Scoped);
    }

    private static void RegisterHangfire(IServiceCollection services, IConfiguration configuration)
    {
        var jobsDatabaseConfig =
            configuration.GetSection(JobsDatabaseSettings.SETTING).Get<JobsDatabaseSettings>();

        services.AddHangfire(hangfireConfig =>
        {
            // hangfireConfig.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString, new SqlServerStorageOptions
            // {
            //     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //     QueuePollInterval = TimeSpan.Zero,
            //     UseRecommendedIsolationLevel = true,
            //     DisableGlobalLocks = true
            // });

            hangfireConfig.UseMemoryStorage();

            hangfireConfig.UseSerializerSettings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });
        });

        GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());
        GlobalJobFilters.Filters.Add(new LogEverythingAttribute());

        RecuringJobs.Register(configuration.GetSection(JobSettings.SETTING).Get<JobSettings>());

        services.AddHangfireServer();
    }
}