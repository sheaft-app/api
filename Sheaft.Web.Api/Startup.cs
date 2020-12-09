using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using AspNetCoreRateLimit;
using AutoMapper;
using Hangfire;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using IdentityModel;
using MangoPay.SDK;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
using Sheaft.Application.Queries;
using Sheaft.Core.Security;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Models;
using Sheaft.GraphQL.Services;
using Sheaft.GraphQL.Types;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Services;
using Sheaft.Options;
using Sheaft.Web.Api.Authorize;
using Sheaft.Web.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;

namespace Sheaft.Web.Api
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

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

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = Configuration.GetValue<bool?>("ShowPII") ?? false;
            services.AddSingleton<IConfiguration>(Configuration);

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            var corsSettings = Configuration.GetSection(CorsOptions.SETTING);
            var mailerSettings = Configuration.GetSection(MailerOptions.SETTING);
            var searchSettings = Configuration.GetSection(SearchOptions.SETTING);
            var appDatabaseSettings = Configuration.GetSection(AppDatabaseOptions.SETTING);
            var jobsDatabaseSettings = Configuration.GetSection(JobsDatabaseOptions.SETTING);
            var roleSettings = Configuration.GetSection(RoleOptions.SETTING);
            var cacheSettings = Configuration.GetSection(CacheOptions.SETTING);
            var pspSettings = Configuration.GetSection(PspOptions.SETTING);

            services.Configure<AuthOptions>(authSettings);
            services.Configure<CorsOptions>(corsSettings);
            services.Configure<MailerOptions>(mailerSettings);
            services.Configure<SearchOptions>(searchSettings);
            services.Configure<AppDatabaseOptions>(appDatabaseSettings);
            services.Configure<JobsDatabaseOptions>(jobsDatabaseSettings);
            services.Configure<RoleOptions>(roleSettings);
            services.Configure<CacheOptions>(cacheSettings);
            services.Configure<PspOptions>(pspSettings);

            services.Configure<ApiOptions>(Configuration.GetSection(ApiOptions.SETTING));
            services.Configure<FreshdeskOptions>(Configuration.GetSection(FreshdeskOptions.SETTING));
            services.Configure<LandingOptions>(Configuration.GetSection(LandingOptions.SETTING));
            services.Configure<PortalOptions>(Configuration.GetSection(PortalOptions.SETTING));
            services.Configure<ScoringOptions>(Configuration.GetSection(ScoringOptions.SETTING));
            services.Configure<SignalrOptions>(Configuration.GetSection(SignalrOptions.SETTING));
            services.Configure<SireneOptions>(Configuration.GetSection(SireneOptions.SETTING));
            services.Configure<SponsoringOptions>(Configuration.GetSection(SponsoringOptions.SETTING));
            services.Configure<StorageOptions>(Configuration.GetSection(StorageOptions.SETTING));
            services.Configure<RoutineOptions>(Configuration.GetSection(RoutineOptions.SETTING));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var rolesOptions = roleSettings.Get<RoleOptions>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.AUTHENTICATED, builder => builder.RequireAuthenticatedUser());
                options.AddPolicy(Policies.REGISTERED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Producer.Value, rolesOptions.Store.Value, rolesOptions.Consumer.Value);
                });
                options.AddPolicy(Policies.STORE_OR_PRODUCER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Producer.Value, rolesOptions.Store.Value);
                });
                options.AddPolicy(Policies.UNREGISTERED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Anonymous.Value);
                });
                options.AddPolicy(Policies.OWNER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Owner.Value);
                });
                options.AddPolicy(Policies.PRODUCER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Producer.Value);
                });
                options.AddPolicy(Policies.STORE, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Store.Value);
                });
                options.AddPolicy(Policies.CONSUMER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Consumer.Value);
                });
            });

            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

            var authConfig = authSettings.Get<AuthOptions>();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authConfig.Url;
                    options.Audience = authConfig.App.Audience;
                    options.RequireHttpsMetadata = Env.IsProduction();
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        RoleClaimType = JwtClaimTypes.Role,
                        NameClaimType = JwtClaimTypes.Subject,
                        AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
                        ValidateIssuer = authConfig.ValidateIssuer,
                        ValidIssuers = authConfig.ValidIssuers,
                    };
                });

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            var corsConfig = corsSettings.Get<CorsOptions>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(corsConfig.Origins.ToArray())
                        .WithHeaders(corsConfig.Headers.ToArray())
                        .WithMethods(corsConfig.Methods.ToArray());
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

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(UserCommandsHandler).Assembly }.ToArray());
            services.AddHttpClient();

            var databaseConfig = appDatabaseSettings.Get<AppDatabaseOptions>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.MigrationsHistoryTable("AppMigrationTable", "ef");
                });
            }, ServiceLifetime.Scoped);

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

            services.AddScoped<IAgreementQueries, AgreementQueries>();
            services.AddScoped<IBusinessQueries, BusinessQueries>();
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
            services.AddScoped<ITransactionQueries, TransactionQueries>();
            services.AddScoped<IDocumentQueries, DocumentQueries>();
            services.AddScoped<ILegalQueries, LegalQueries>();

            services.AddScoped<IDapperContext, DapperContext>();
            services.AddScoped<IAppDbContext>(c => c.GetRequiredService<AppDbContext>());

            var searchConfig = searchSettings.Get<SearchOptions>();
            services.AddScoped<ISearchServiceClient, SearchServiceClient>(_ => new SearchServiceClient(searchConfig.Name, new SearchCredentials(searchConfig.ApiKey)));

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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();
            services.AddScoped<ISheaftHangfireBridge, SheaftHangfireBridge>();

            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddAuthorizeDirectiveType()
                .ModifyOptions(c => c.DefaultBindingBehavior = BindingBehavior.Explicit)
                .AddMutationType<SheaftMutationType>()
                .AddQueryType<SheaftQueryType>()
                .BindClrType<TimeSpan, SheaftTimeSpanType>()
                .RegisterTypes()
                .Create(), new QueryExecutionOptions { IncludeExceptionDetails = true });

            services.AddErrorFilter<SheaftErrorFilter>();

            var cacheConfig = cacheSettings.Get<CacheOptions>();
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = databaseConfig.ConnectionString;
                options.SchemaName = cacheConfig.SchemaName;
                options.TableName = cacheConfig.TableName;
            });

            services.AddOptions();

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

            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddSingleton<IAuthorizationService, SheaftIdentityAuthorizeService>();

            services.AddLogging(config =>
            {
                config.AddSerilog(dispose: true);
            });

            var jobsDatabaseConfig = jobsDatabaseSettings.Get<JobsDatabaseOptions>();
            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString);
                configuration.UseSerializerSettings(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            });

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<IAppDbContext>();
                if (!context.AllMigrationsApplied())
                {
                    context.Migrate();
                }

                var adminId = configuration.GetValue<Guid>("Users:admin:id");
                var admin = context.Users.FirstOrDefault(u => u.Id == adminId);
                if (admin == null)
                {
                    var firstname = configuration.GetValue<string>("Users:admin:firstname");
                    var lastname = configuration.GetValue<string>("Users:admin:lastname");
                    var email = configuration.GetValue<string>("Users:admin:email");

                    admin = new Admin(adminId, $"{firstname} {lastname}", firstname, lastname, email);
                    admin.SetIdentifier(configuration.GetValue<string>("Psp:UserId"));
                    context.Add(admin);
                    context.SaveChanges();
                }

                var donationWalletId = configuration.GetValue<string>("Psp:WalletId");
                if (context.Wallets.FirstOrDefault(u => u.Identifier == donationWalletId) == null)
                {
                    var donationWallet = new Wallet(Guid.NewGuid(), "Donation", WalletKind.Donations, admin);
                    donationWallet.SetIdentifier(donationWalletId);
                    context.Add(donationWallet);
                    context.SaveChanges();
                }

                var documentWalletId = configuration.GetValue<string>("Psp:DocumentWalletId");
                if (context.Wallets.FirstOrDefault(u => u.Identifier == documentWalletId) == null)
                {
                    var documentWallet = new Wallet(Guid.NewGuid(), "Document", WalletKind.Documents, admin);
                    documentWallet.SetIdentifier(documentWalletId);
                    context.Add(documentWallet);
                    context.SaveChanges();
                }

                if (context.BankAccounts.FirstOrDefault(c => c.User.Id == admin.Id) == null)
                {
                    var bankAccount = new BankAccount(Guid.NewGuid(), "Dons", "Sheaft", configuration.GetValue<string>("Psp:Bank:Iban"), configuration.GetValue<string>("Psp:Bank:Bic"),
                        new BankAddress(
                            configuration.GetValue<string>("Psp:Bank:Address:Line1"),
                            configuration.GetValue<string>("Psp:Bank:Address:Line2"),
                            configuration.GetValue<string>("Psp:Bank:Address:Zipcode"),
                            configuration.GetValue<string>("Psp:Bank:Address:City"),
                            configuration.GetValue<CountryIsoCode>("Psp:Bank:Address:Country")), admin);

                    bankAccount.SetIdentifier(configuration.GetValue<string>("Psp:Bank:Id"));
                    context.Add(bankAccount);
                    context.SaveChanges();
                }

                var supportId = configuration.GetValue<Guid>("Users:support:id");
                if (context.Users.FirstOrDefault(u => u.Id == supportId) == null)
                {
                    var firstname = configuration.GetValue<string>("Users:support:firstname");
                    var lastname = configuration.GetValue<string>("Users:support:lastname");
                    var email = configuration.GetValue<string>("Users:support:email");

                    context.Add(new Support(supportId, $"{firstname} {lastname}", firstname, lastname, email));
                    context.SaveChanges();
                }
            }

            app.UseRobotsTxt();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIpRateLimiting();

            app.UseMvc(endpoints =>
            {
                endpoints.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWebSockets();
            app.UseGraphQL("/graphql");
        }
    }
}
