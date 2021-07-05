using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using Amazon;
using Amazon.SimpleEmail;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using HotChocolate.Types.Relay;
using IdentityModel;
using MangoPay.SDK;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using NewRelic.LogEnrichers.Serilog;
using Newtonsoft.Json;
using Razor.Templating.Core;
using Serilog;
using Serilog.Events;
using Sheaft.Application.Behaviours;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Mappings;
using Sheaft.Business;
using Sheaft.Business.Factories;
using Sheaft.Business.PickingOrdersExporters;
using Sheaft.Business.ProductsImporters;
using Sheaft.Business.PurchaseOrdersExporters;
using Sheaft.Business.TransactionsExporters;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Services;
using Sheaft.Mediatr;
using Sheaft.Mediatr.Donation.Commands;
using Sheaft.Mediatr.Order.Commands;
using Sheaft.Mediatr.Payin.Commands;
using Sheaft.Mediatr.PayinRefund.Commands;
using Sheaft.Mediatr.Payout.Commands;
using Sheaft.Mediatr.PreAuthorization.Commands;
using Sheaft.Mediatr.Producer.Commands;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.PurchaseOrder.Commands;
using Sheaft.Mediatr.Store.Commands;
using Sheaft.Mediatr.Transfer.Commands;
using Sheaft.Mediatr.Zone.Commands;
using Sheaft.Options;
using Sheaft.Web.Common;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace Sheaft.Web.Jobs
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Env = environment;
            Configuration = configuration;

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithNewRelicLogsInContext();

            if (Env.IsProduction())
            {
                logger = logger
                    .WriteTo.Async(a => a.NewRelicLogs(
                        endpointUrl: Configuration.GetValue<string>("NEW_RELIC_LOG_API"),
                        applicationName: Configuration.GetValue<string>("NEW_RELIC_APP_NAME"),
                        licenseKey: Configuration.GetValue<string>("NEW_RELIC_LICENSE_KEY"),
                        insertKey: Configuration.GetValue<string>("NEW_RELIC_INSERT_KEY"),
                        restrictedToMinimumLevel: Configuration.GetValue<LogEventLevel>("NEW_RELIC_LOG_LEVEL"),
                        batchSizeLimit: Configuration.GetValue<int>("NEW_RELIC_BATCH_SIZE")
                    ));
            }
            else
            {
                logger = logger
                    .WriteTo.Async(a => a.Console());
            }

            Log.Logger = logger.CreateLogger();

            GlobalConfiguration.Configuration.UseSerilogLogProvider();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = Configuration.GetValue<bool?>("ShowPII") ?? false;

            GlobalJobFilters.Filters.Add(new ProlongExpirationTimeAttribute());
            GlobalJobFilters.Filters.Add(new LogEverythingAttribute());

            var mailerSettings = Configuration.GetSection(MailerOptions.SETTING);
            services.Configure<MailerOptions>(mailerSettings);

            var appDatabaseSettings = Configuration.GetSection(AppDatabaseOptions.SETTING);
            services.Configure<AppDatabaseOptions>(appDatabaseSettings);

            var storageSettings = Configuration.GetSection(StorageOptions.SETTING);
            services.Configure<StorageOptions>(storageSettings);

            var pspSettings = Configuration.GetSection(PspOptions.SETTING);
            services.Configure<PspOptions>(pspSettings);

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            services.Configure<AuthOptions>(authSettings);

            var roleSettings = Configuration.GetSection(RoleOptions.SETTING);
            services.Configure<RoleOptions>(roleSettings);

            var jobsDatabaseSettings = Configuration.GetSection(JobsDatabaseOptions.SETTING);
            services.Configure<JobsDatabaseOptions>(jobsDatabaseSettings);

            services.Configure<CorsOptions>(Configuration.GetSection(CorsOptions.SETTING));
            services.Configure<SearchOptions>(Configuration.GetSection(SearchOptions.SETTING));
            services.Configure<ApiOptions>(Configuration.GetSection(ApiOptions.SETTING));
            services.Configure<AppDatabaseOptions>(Configuration.GetSection(AppDatabaseOptions.SETTING));
            services.Configure<FreshdeskOptions>(Configuration.GetSection(FreshdeskOptions.SETTING));
            services.Configure<LandingOptions>(Configuration.GetSection(LandingOptions.SETTING));
            services.Configure<PortalOptions>(Configuration.GetSection(PortalOptions.SETTING));
            services.Configure<ScoringOptions>(Configuration.GetSection(ScoringOptions.SETTING));
            services.Configure<SearchOptions>(Configuration.GetSection(SearchOptions.SETTING));
            services.Configure<MailerOptions>(Configuration.GetSection(MailerOptions.SETTING));
            services.Configure<SignalrOptions>(Configuration.GetSection(SignalrOptions.SETTING));
            services.Configure<SireneOptions>(Configuration.GetSection(SireneOptions.SETTING));
            services.Configure<SponsoringOptions>(Configuration.GetSection(SponsoringOptions.SETTING));
            services.Configure<RoutineOptions>(Configuration.GetSection(RoutineOptions.SETTING));
            services.Configure<PictureOptions>(Configuration.GetSection(PictureOptions.SETTING));
            services.Configure<ImportersOptions>(Configuration.GetSection(ImportersOptions.SETTING));
            services.Configure<ExportersOptions>(Configuration.GetSection(ExportersOptions.SETTING));

            var rolesOptions = roleSettings.Get<RoleOptions>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Hangfire", cfg =>
                {
                    cfg.AddRequirements().RequireAuthenticatedUser();
                    cfg.AddAuthenticationSchemes("oidc");
                    cfg.AddRequirements().RequireRole(rolesOptions.Admin.Value, rolesOptions.Support.Value);
                });
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAssertion(c => c.User.Identity is not {IsAuthenticated: true})
                    .Build();
            });

            var authConfig = authSettings.Get<AuthOptions>();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = authConfig.Url;

                    options.ClientId = authConfig.Jobs.Id;
                    options.ClientSecret = authConfig.Jobs.Secret;

                    options.UsePkce = true;
                    options.ResponseType = "code";
                    options.SaveTokens = true;

                    options.Scope.Add("openid");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("role");
                    options.Scope.Add("email");

                    options.RequireHttpsMetadata = Env.IsProduction();

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        RoleClaimType = JwtClaimTypes.Role,
                        NameClaimType = JwtClaimTypes.Subject,
                        ValidateIssuer = authConfig.ValidateIssuer,
                        ValidIssuers = authConfig.ValidIssuers,
                    };
                });

            services.AddAutoMapper(typeof(ProductProfile).Assembly, typeof(CreateProductProfile).Assembly);
            services.AddMediatR(typeof(RefreshPayinStatusCommand).Assembly);

            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddSingleton(Configuration);
            services.AddHttpClient();

            var mailerConfig = mailerSettings.Get<MailerOptions>();
            services.AddScoped<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>(_ =>
                new AmazonSimpleEmailServiceClient(mailerConfig.ApiId, mailerConfig.ApiKey, RegionEndpoint.EUCentral1));

            services.AddScoped<IIdentifierService, IdentifierService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPspService, PspService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();

            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDeliveryBatchService, DeliveryBatchService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<IIdSerializer, IdSerializer>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScopedDynamic<IProductsFileImporter>(typeof(ExcelProductsImporter).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IProductsFileImporter))));
            services.AddScopedDynamic<IPickingOrdersFileExporter>(typeof(ExcelPickingOrdersExporter).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IPickingOrdersFileExporter))));
            services.AddScopedDynamic<IPurchaseOrdersFileExporter>(typeof(ExcelPurchaseOrdersExporter).Assembly
                .GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IPurchaseOrdersFileExporter))));
            services.AddScopedDynamic<ITransactionsFileExporter>(typeof(ExcelTransactionsExporter).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ITransactionsFileExporter))));

            services.AddScoped<IProductsImporterFactory, ProductsImporterFactory>();
            services.AddScoped<IPickingOrdersExportersFactory, PickingOrdersExportersFactory>();
            services.AddScoped<IPurchaseOrdersExportersFactory, PurchaseOrdersExportersFactory>();
            services.AddScoped<ITransactionsExportersFactory, TransactionsExportersFactory>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            var storageConfig = storageSettings.Get<StorageOptions>();
            services.AddSingleton<CloudStorageAccount>(CloudStorageAccount.Parse(storageConfig.ConnectionString));

            services.AddScoped<IDapperContext, DapperContext>();
            services.AddScoped<ISheaftMediatr, SheaftMediatr>();
            services.AddScoped<ISheaftDispatcher, SheaftDispatcher>();

            services.AddOptions();

            var databaseConfig = appDatabaseSettings.Get<AppDatabaseOptions>();
            services.AddPooledDbContextFactory<QueryDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("AppMigrationTable", "ef");
                });
            });

            services.AddDbContext<IAppDbContext, WriterDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("AppMigrationTable", "ef");
                });
            });

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

            var jobsDatabaseConfig = jobsDatabaseSettings.Get<JobsDatabaseOptions>();
            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });
                configuration.UseSerializerSettings(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            });

            services.AddLogging(config => { config.AddSerilog(dispose: true); });

            services.AddRazorTemplating();
            services.AddHangfireServer();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IOptionsSnapshot<RoutineOptions> routineOptions, IOptionsSnapshot<RoleOptions> roleOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRobotsTxt();

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                {
                    AppPath = Configuration.GetValue<string>("Portal:Url")
                }).RequireAuthorization("Hangfire");
            });

            RecuringJobs.Register(routineOptions.Value);
            RazorTemplateEngine.Initialize();
        }
    }

    public static class RecuringJobs
    {
        public static void Register(RoutineOptions options)
        {
            RecurringJob.AddOrUpdate<SheaftDispatcher>("79d5e199b5ef41268fade4da1fa3f83b", mediatr =>
                    mediatr.Execute(nameof(CheckOrdersCommand),
                        new CheckOrdersCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckOrdersCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("44d0d009c3d24cb6b05f113e49b60d35", mediatr =>
                    mediatr.Execute(nameof(CheckDonationsCommand),
                        new CheckDonationsCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckDonationsCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("cd2bc132393f4a379f7ac44d56f84d9e", mediatr =>
                    mediatr.Execute(nameof(CheckPayinsCommand),
                        new CheckPayinsCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckPayinsCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("eaf648de5fe54fc1980c093fd78bb2f7", mediatr =>
                    mediatr.Execute(nameof(CheckPayinRefundsCommand),
                        new CheckPayinRefundsCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckPayinRefundsCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("dddd91a8fa494da3af1477d1b537fd95", mediatr =>
                    mediatr.Execute(nameof(CheckPayoutsCommand),
                        new CheckPayoutsCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckPayoutsCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("50a160aac50f480a872b04a509ef202c", mediatr =>
                    mediatr.Execute(nameof(CheckTransfersCommand),
                        new CheckTransfersCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckTransfersCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("ae81c9c623f940b386ac9d3144147557", mediatr =>
                    mediatr.Execute(nameof(CheckNewPayoutsCommand),
                        new CheckNewPayoutsCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckNewPayoutsCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("0b74e15ec9de4332981a8f933377fc0a", mediatr =>
                    mediatr.Execute(nameof(UpdateZonesProgressCommand),
                        new UpdateZonesProgressCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckZonesProgressCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("7236b37addc04f62ac2afef157903132", mediatr =>
                    mediatr.Execute(nameof(GenerateZonesFileCommand),
                        new GenerateZonesFileCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckZonesFileCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("4787cf27f6bd491292014902a84a11ae", mediatr =>
                    mediatr.Execute(nameof(GenerateProducersFileCommand),
                        new GenerateProducersFileCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckProducersFileCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("6757cf27f6bd491292014902a84a11ae", mediatr =>
                    mediatr.Execute(nameof(GenerateStoresFileCommand),
                        new GenerateStoresFileCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckStoresFileCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("41739516c21a48dc907b429a96cd0edd", mediatr =>
                    mediatr.Execute(nameof(CheckExpiredPurchaseOrdersCommand),
                        new CheckExpiredPurchaseOrdersCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckExpiredPurchaseOrdersCron);

            RecurringJob.AddOrUpdate<SheaftDispatcher>("707f7ee279c14f24ad7d5f31e22f4d3c", mediatr =>
                    mediatr.Execute(nameof(CheckPreAuthorizationsCommand),
                        new CheckPreAuthorizationsCommand(new RequestUser("037e7e93c73f4406a4e31994d8686b7c",
                            Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                options.CheckPreAuthorizationsCron);
        }
    }
}