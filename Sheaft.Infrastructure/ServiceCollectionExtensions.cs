using Amazon;
using Amazon.SimpleEmail;
using DataModel;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.OrderManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.BatchManagement;
using Sheaft.Domain.BillingManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.DocumentManagement;
using Sheaft.Domain.OrderManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.AgreementManagement;
using Sheaft.Infrastructure.BatchManagement;
using Sheaft.Infrastructure.BillingManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.ProductManagement;
using Sheaft.Infrastructure.CustomerManagement;
using Sheaft.Infrastructure.DocumentManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Services;
using Sheaft.Infrastructure.SupplierManagement;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace Sheaft.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration,
        bool isInMemory)
    {
        RegisterMediator(services);
        RegisterPdfServices(services);
        RegisterEmailServices(services, configuration);
        RegisterRepositories(services, configuration);
        RegisterQueries(services, configuration);
        RegisterDatabaseServices(services, configuration, isInMemory);
        RegisterSignalr(services);
        RegisterServices(services);

        RegisterHangfire(services, configuration, isInMemory);

        services.AddHttpClient();
        services.AddRazorTemplating();
    }

    private static void RegisterQueries(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProductQueries, ProductQueries>();
        services.AddScoped<IReturnableQueries, ReturnableQueries>();
        services.AddScoped<IAgreementQueries, AgreementQueries>();
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<IBatchQueries, BatchQueries>();
    }

    private static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<ICatalogRepository, CatalogRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IReturnableRepository, ReturnableRepository>();

        services.AddScoped<IAgreementRepository, AgreementRepository>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();

        services.AddScoped<IBatchRepository, BatchRepository>();

        services.AddScoped<IDocumentRepository, DocumentRepository>();
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
        services.AddScoped<IEmailingService, EmailingService>();

        var mailerConfig = configuration.GetSection(EmailingSettings.SETTING).Get<EmailingSettings>();
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
        services.AddScoped<IRetrieveProfile, RetrieveProfile>();

        services.AddScoped<IUniquenessValidator, UniquenessValidator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ISecurityTokensProvider, SecurityTokensProvider>();

        services.AddScoped<IValidateSupplierRegistration, ValidateSupplierRegistration>();
        services.AddScoped<IValidateCustomerRegistration, ValidateCustomerRegistration>();

        services.AddScoped<IGenerateProductCode, GenerateProductCode>();
        services.AddScoped<IGenerateReturnableCode, GenerateReturnableCode>();

        services.AddScoped<IValidateAgreementProposal, ValidateAgreementProposal>();

        services.AddScoped<IGenerateDeliveryCode, GenerateDeliveryCode>();
        services.AddScoped<IGenerateOrderCode, GenerateOrderCode>();
        services.AddScoped<IRetrieveAgreementForOrder, RetrieveAgreementForOrder>();
        services.AddScoped<IRetrieveDeliveryDays, RetrieveDeliveryDays>();
        services.AddScoped<IRetrieveOrderCustomer, RetrieveOrderCustomer>();
        services.AddScoped<ITransformProductsToOrderLines, TransformProductsToOrderLines>();
        services.AddScoped<ICreateDeliveryReturnedReturnables, CreateDeliveryReturnedReturnables>();
        services.AddScoped<ICreateDeliveryProductAdjustments, CreateDeliveryProductAdjustments>();
        services.AddScoped<ICreateDeliveryLines, CreateDeliveryLines>();

        services.AddScoped<IGenerateCreditNoteCode, GenerateCreditNoteCode>();
        services.AddScoped<IGenerateInvoiceCode, GenerateInvoiceCode>();
        services.AddScoped<IRetrieveBillingInformation, RetrieveBillingInformation>();

        services.AddScoped<IValidateAlteringBatchCapability, ValidateAlteringBatchCapability>();

        services.AddScoped<IDocumentParamsHandler, DocumentParamsHandler>();
        services.AddScoped<IFileStorage, FileStorage>();
        services.AddScoped<IPreparationFileGenerator, PreparationFileGenerator>();
    }

    private static void RegisterDatabaseServices(IServiceCollection services, IConfiguration configuration,
        bool isInMemory)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var appDatabaseConfig =
            configuration.GetSection(AppDatabaseSettings.SETTING).Get<AppDatabaseSettings>();

        var connectionStrings = new Dictionary<DatabaseConnectionName, string>
        {
            {DatabaseConnectionName.AppDatabase, appDatabaseConfig.ConnectionString}
        };

        services.AddSingleton<IDictionary<DatabaseConnectionName, string>>(connectionStrings);
        services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();

        services.AddDbContext<IDbContext, AppDbContext>(optionsBuilder =>
        {
            if (isInMemory)
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
            else
                optionsBuilder.UseSqlServer(appDatabaseConfig.ConnectionString);
        });

        services.AddLinqToDBContext<AppDb>((provider, options) =>
        {
            if (isInMemory)
                options
                    .UseSQLite("Data Source=:memory:")
                    .UseDefaultLogging(provider);
            else
                options
                    .UseSqlServer(appDatabaseConfig.ConnectionString)
                    .UseDefaultLogging(provider);
        });
    }

    private static void RegisterHangfire(IServiceCollection services, IConfiguration configuration, bool isInMemory)
    {
        var jobsDatabaseConfig =
            configuration.GetSection(JobsDatabaseSettings.SETTING).Get<JobsDatabaseSettings>();

        services.AddHangfire(hangfireConfig =>
        {
            if (isInMemory)
                hangfireConfig.UseMemoryStorage(new MemoryStorageOptions());
            else
                hangfireConfig.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                    SchemaName = "hf"
                });

            //hangfireConfig.UseMemoryStorage();

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

public static class WebApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app, ILogger? logger)
    {
        try
        {
            using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!context.AllMigrationsApplied())
                context.Database.Migrate();
        }
        catch (Exception e)
        {
            logger?.LogCritical(e, e.Message);
        }
    }
}