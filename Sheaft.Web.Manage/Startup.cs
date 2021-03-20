using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using AutoMapper;
using IdentityModel;
using MangoPay.SDK;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Services;
using Microsoft.Azure.Search;
using Hangfire;
using Newtonsoft.Json;
using Amazon.SimpleEmail;
using RazorLight;
using Amazon;
using MediatR;
using Serilog;
using Serilog.Events;
using NewRelic.LogEnrichers.Serilog;
using Sheaft.Web.Common;
using Microsoft.IdentityModel.Logging;
using Microsoft.Azure.Cosmos.Table;
using Sheaft.Application;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Queries;
using Sheaft.Application.Mappings;
using Sheaft.Business;
using Sheaft.Mediatr.Agreement.Queries;
using Sheaft.Mediatr.Consumer.Queries;
using Sheaft.Mediatr.Country.Queries;
using Sheaft.Mediatr.DeliveryMode.Queries;
using Sheaft.Mediatr.Department.Queries;
using Sheaft.Mediatr.Document.Queries;
using Sheaft.Mediatr.Job.Queries;
using Sheaft.Mediatr.Leaderboard.Queries;
using Sheaft.Mediatr.Legal.Queries;
using Sheaft.Mediatr.Nationality.Queries;
using Sheaft.Mediatr.Notification.Queries;
using Sheaft.Mediatr.Order.Queries;
using Sheaft.Mediatr.Payin.Queries;
using Sheaft.Mediatr.Producer.Queries;
using Sheaft.Mediatr.Product.Queries;
using Sheaft.Mediatr.PurchaseOrder.Queries;
using Sheaft.Mediatr.QuickOrder.Queries;
using Sheaft.Mediatr.Region.Queries;
using Sheaft.Mediatr.Returnable.Queries;
using Sheaft.Mediatr.Store.Commands;
using Sheaft.Mediatr.Tag.Queries;
using Sheaft.Mediatr.User.Queries;
using Sheaft.Options;
using Sheaft.Web.Manage.Mappings;

namespace Sheaft.Web.Manage
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
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = Configuration.GetValue<bool?>("ShowPII") ?? false;

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            var databaseSettings = Configuration.GetSection(AppDatabaseOptions.SETTING);
            var jobsDatabaseSettings = Configuration.GetSection(JobsDatabaseOptions.SETTING);
            var mailerSettings = Configuration.GetSection(MailerOptions.SETTING);
            var roleSettings = Configuration.GetSection(RoleOptions.SETTING);
            var pspSettings = Configuration.GetSection(PspOptions.SETTING);
            var searchSettings = Configuration.GetSection(SearchOptions.SETTING);
            var storageSettings = Configuration.GetSection(StorageOptions.SETTING);

            services.Configure<RoleOptions>(roleSettings);
            services.Configure<AuthOptions>(authSettings);
            services.Configure<AppDatabaseOptions>(databaseSettings);
            services.Configure<JobsDatabaseOptions>(jobsDatabaseSettings);
            services.Configure<MailerOptions>(mailerSettings);
            services.Configure<PspOptions>(pspSettings);
            services.Configure<StorageOptions>(storageSettings);
            services.Configure<ImportersOptions>(Configuration.GetSection(ImportersOptions.SETTING));
            services.Configure<ExportersOptions>(Configuration.GetSection(ExportersOptions.SETTING));

            var rolesOptions = roleSettings.Get<RoleOptions>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", o =>
                {
                    o.RequireAuthenticatedUser();
                    o.RequireRole(rolesOptions.Admin.Value);
                });
                options.AddPolicy("Support", o =>
                {
                    o.RequireAuthenticatedUser();
                    o.RequireRole(rolesOptions.Support.Value);
                });
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(rolesOptions.Admin.Value, rolesOptions.Support.Value)
                    .Build();
            });

            services.AddAutoMapper(new []{typeof(ProductProfile).Assembly, typeof(ProductViewProfile).Assembly});

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var databaseConfig = databaseSettings.Get<AppDatabaseOptions>();
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("AppMigrationTable", "ef");
                });
            }, ServiceLifetime.Scoped);

            var mailerConfig = mailerSettings.Get<MailerOptions>();

            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            services.AddScoped<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>(_ => new AmazonSimpleEmailServiceClient(Configuration.GetValue<string>("Mailer:ApiId"), Configuration.GetValue<string>("Mailer:ApiKey"), RegionEndpoint.EUCentral1));

            services.AddScoped<IRazorLightEngine>(_ => {
                var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                return new RazorLightEngineBuilder()
                .UseFileSystemProject($"{rootDir.Replace("file:\\", string.Empty).Replace("file:", string.Empty)}/Mailings/Templates")
                .UseMemoryCachingProvider()
                .Build();
            });

            var authConfig = authSettings.Get<AuthOptions>();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = authConfig.Url;

                    options.ClientId = authConfig.Manage.Id;
                    options.ClientSecret = authConfig.Manage.Secret;

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

            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly }.ToArray());
            
            var jobsDatabaseConfig = jobsDatabaseSettings.Get<JobsDatabaseOptions>();
            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString);
                configuration.UseSerializerSettings(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();

            var pspOptions = pspSettings.Get<PspOptions>();
            services.AddScoped<MangoPayApi>(c => new MangoPayApi
            {
                Config = new MangoPay.SDK.Core.Configuration
                {
                    BaseUrl = pspOptions.ApiUrl,
                    ClientId = pspOptions.ClientId,
                    ClientPassword = pspOptions.ApiKey
                }
            });

            services.AddScoped<IDapperContext, DapperContext>();
            services.AddScoped<IIdentifierService, IdentifierService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPspService, PspService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITableService, TableService>();
            services.AddSingleton<IBackgroundJobClient, BackgroundJobClient>();
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<ISheaftMediatr, SheaftMediatr>();
            
            services.AddScoped<IFeesCalculator, FeesCalculator>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IProductsFileImporter, ExcelProductsImporter>();
            services.AddScoped<IPurchaseOrdersFileExporter, ExcelPurchaseOrdersExporter>();
            services.AddScoped<IPickingOrdersFileExporter, ExcelPickingOrdersExporter>();
            services.AddScoped<ITransactionsFileExporter, ExcelTransactionsExporter>();
            
            services.AddScoped<IProductsImporterFactory, ProductsImporterFactory>();
            services.AddScoped<IPickingOrdersExportersFactory, PickingOrdersExportersFactory>();
            services.AddScoped<IPurchaseOrdersExportersFactory, PurchaseOrdersExportersFactory>();
            services.AddScoped<ITransactionsExportersFactory, TransactionsExportersFactory>();

            services.AddScoped<IAgreementQueries, AgreementQueries>();
            services.AddScoped<IProducerQueries, ProducerQueries>();
            services.AddScoped<IDeliveryQueries, DeliveryQueries>();
            services.AddScoped<IDepartmentQueries, DepartmentQueries>();
            services.AddScoped<IJobQueries, JobQueries>();
            services.AddScoped<ILeaderboardQueries, LeaderboardQueries>();
            services.AddScoped<INotificationQueries, NotificationQueries>();
            services.AddScoped<IReturnableQueries, ReturnableQueries>();
            services.AddScoped<IProductQueries, ProductQueries>();
            services.AddScoped<IPurchaseOrderQueries, PurchaseOrderQueries>();
            services.AddScoped<IQuickOrderQueries, QuickOrderQueries>();
            services.AddScoped<IRegionQueries, RegionQueries>();
            services.AddScoped<ITagQueries, TagQueries>();
            services.AddScoped<IConsumerQueries, ConsumerQueries>();
            services.AddScoped<IUserQueries, UserQueries>();
            services.AddScoped<INationalityQueries, NationalityQueries>();
            services.AddScoped<ICountryQueries, CountryQueries>();
            services.AddScoped<IOrderQueries, OrderQueries>();
            services.AddScoped<IPayinQueries, PayinQueries>();
            services.AddScoped<IDocumentQueries, DocumentQueries>();
            services.AddScoped<ILegalQueries, LegalQueries>();

            var storageConfig = storageSettings.Get<StorageOptions>();
            services.AddSingleton<CloudStorageAccount>(CloudStorageAccount.Parse(storageConfig.ConnectionString));

            var searchConfig = searchSettings.Get<SearchOptions>();
            services.AddScoped<ISearchServiceClient, SearchServiceClient>(_ => new SearchServiceClient(searchConfig.Name, new SearchCredentials(searchConfig.ApiKey)));

            services.AddOptions();
            services.AddControllersWithViews();

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

            services.AddLogging(config =>
            {
                config.AddSerilog(dispose: true);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            if (!env.IsDevelopment())
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
