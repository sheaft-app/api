using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading;
using Amazon;
using Amazon.SimpleEmail;
using AutoMapper;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using IdentityModel;
using MangoPay.SDK;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using NewRelic.LogEnrichers.Serilog;
using Newtonsoft.Json;
using RazorLight;
using Serilog;
using Serilog.Events;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Handlers;
using Sheaft.Application.Interop;
using Sheaft.Application.Mappers;
using Sheaft.Core;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Services;
using Sheaft.Options;
using Sheaft.Web.Common;

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
            services.Configure<AuthOptions>(pspSettings);

            var roleSettings = Configuration.GetSection(RoleOptions.SETTING);
            services.Configure<RoleOptions>(pspSettings);

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

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(UserCommandsHandler).Assembly }.ToArray());

            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();

            var mailerConfig = mailerSettings.Get<MailerOptions>();

            var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            services.AddScoped<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>(_ => new AmazonSimpleEmailServiceClient(Configuration.GetValue<string>("Mailer:ApiId"), Configuration.GetValue<string>("Mailer:ApiKey"), RegionEndpoint.EUCentral1));

            services.AddScoped<IRazorLightEngine>(_ => {
                var rootDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                return new RazorLightEngineBuilder()
                .UseFileSystemProject($"{rootDir.Replace("file:\\", string.Empty).Replace("file:", string.Empty)}/Templates")
                .UseMemoryCachingProvider()
                .Build();
            });

            services.AddScoped<IIdentifierService, IdentifierService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPspService, PspService>();
            services.AddScoped<IFeesService, FeesService>();
            services.AddScoped<ICapingDeliveriesService, CapingDeliveriesService>();
            services.AddScoped<ISheaftMediatr, SheaftMediatr>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISheaftHangfireBridge, SheaftHangfireBridge>();
            services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();

            var storageConfig = storageSettings.Get<StorageOptions>();
            services.AddSingleton<CloudStorageAccount>(CloudStorageAccount.Parse(storageConfig.ConnectionString));

            services.AddScoped<IDapperContext, DapperContext>();

            services.AddOptions();

            var databaseConfig = appDatabaseSettings.Get<AppDatabaseOptions>();
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("AppMigrationTable", "ef");
                });
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

            services.AddLogging(config =>
            {
                config.AddSerilog(dispose: true);
            });

            services.AddHangfireServer();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsSnapshot<RoutineOptions> routineOptions)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard("", new DashboardOptions
                {
                    AppPath = Configuration.GetValue<string>("Portal:Url"),
                    Authorization = new List<IDashboardAuthorizationFilter> { new MyAuthorizationFilter() }
                });
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            RecuringJobs.Register(routineOptions.Value);
        }
    }

    public static class RecuringJobs
    {
        public static void Register(RoutineOptions options)
        {
            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("79d5e199b5ef41268fade4da1fa3f83b", mediatr =>
                mediatr.Execute(nameof(CheckOrdersCommand), new CheckOrdersCommand(new RequestUser("79d5e199b5ef41268fade4da1fa3f83b", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckOrdersCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("44d0d009c3d24cb6b05f113e49b60d35", mediatr =>
                mediatr.Execute(nameof(CheckDonationsCommand), new CheckDonationsCommand(new RequestUser("44d0d009c3d24cb6b05f113e49b60d35", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckDonationsCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("cd2bc132393f4a379f7ac44d56f84d9e", mediatr =>
                mediatr.Execute(nameof(CheckPayinsCommand), new CheckPayinsCommand(new RequestUser("cd2bc132393f4a379f7ac44d56f84d9e", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckPayinsCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("eaf648de5fe54fc1980c093fd78bb2f7", mediatr =>
                mediatr.Execute(nameof(CheckPayinRefundsCommand), new CheckPayinRefundsCommand(new RequestUser("eaf648de5fe54fc1980c093fd78bb2f7", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckPayinRefundsCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("dddd91a8fa494da3af1477d1b537fd95", mediatr =>
                mediatr.Execute(nameof(CheckPayoutsCommand), new CheckPayoutsCommand(new RequestUser("dddd91a8fa494da3af1477d1b537fd95", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckPayoutsCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("50a160aac50f480a872b04a509ef202c", mediatr =>
                mediatr.Execute(nameof(CheckTransfersCommand), new CheckTransfersCommand(new RequestUser("50a160aac50f480a872b04a509ef202c", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckTransfersCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("ae81c9c623f940b386ac9d3144147557", mediatr =>
                mediatr.Execute(nameof(CheckNewPayoutsCommand), new CheckNewPayoutsCommand(new RequestUser("ae81c9c623f940b386ac9d3144147557", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckNewPayoutsCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("0b74e15ec9de4332981a8f933377fc0a", mediatr =>
                mediatr.Execute(nameof(UpdateZonesProgressCommand), new UpdateZonesProgressCommand(new RequestUser("0b74e15ec9de4332981a8f933377fc0a", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckZonesProgressCron);

            RecurringJob.AddOrUpdate<SheaftHangfireBridge>("7236b37addc04f62ac2afef157903132", mediatr =>
                mediatr.Execute(nameof(GenerateZonesFileCommand), new GenerateZonesFileCommand(new RequestUser("7236b37addc04f62ac2afef157903132", Guid.NewGuid().ToString("N"), null)), CancellationToken.None),
                    options.CheckZonesFileCron);
        }
    }
}
