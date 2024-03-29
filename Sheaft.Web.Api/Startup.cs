﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using Amazon;
using Amazon.SimpleEmail;
using AspNetCoreRateLimit;
using Hangfire;
using HotChocolate.Types;
using IdentityModel;
using MangoPay.SDK;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using NewRelic.LogEnrichers.Serilog;
using Newtonsoft.Json;
using Razor.Templating.Core;
using Serilog;
using Serilog.Events;
using Sheaft.Application.Behaviours;
using Sheaft.Application.Exporters;
using Sheaft.Application.Factories;
using Sheaft.Application.Importers;
using Sheaft.Application.Interfaces.Exporters;
using Sheaft.Application.Interfaces.Factories;
using Sheaft.Application.Interfaces.Importers;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Application.Services;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Persistence.Extensions;
using Sheaft.Infrastructure.Services;
using Sheaft.Mediatr;
using Sheaft.Mediatr.Product.Commands;
using Sheaft.Mediatr.Store.Commands;
using Sheaft.Web.Api.Authorize;
using Sheaft.Web.Api.Extensions;
using Sheaft.Web.Common;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

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
            var storageSettings = Configuration.GetSection(StorageOptions.SETTING);

            services.Configure<AuthOptions>(authSettings);
            services.Configure<CorsOptions>(corsSettings);
            services.Configure<MailerOptions>(mailerSettings);
            services.Configure<SearchOptions>(searchSettings);
            services.Configure<AppDatabaseOptions>(appDatabaseSettings);
            services.Configure<JobsDatabaseOptions>(jobsDatabaseSettings);
            services.Configure<RoleOptions>(roleSettings);
            services.Configure<CacheOptions>(cacheSettings);
            services.Configure<PspOptions>(pspSettings);
            services.Configure<StorageOptions>(storageSettings);

            services.Configure<ApiOptions>(Configuration.GetSection(ApiOptions.SETTING));
            services.Configure<FreshdeskOptions>(Configuration.GetSection(FreshdeskOptions.SETTING));
            services.Configure<LandingOptions>(Configuration.GetSection(LandingOptions.SETTING));
            services.Configure<PortalOptions>(Configuration.GetSection(PortalOptions.SETTING));
            services.Configure<ScoringOptions>(Configuration.GetSection(ScoringOptions.SETTING));
            services.Configure<SignalrOptions>(Configuration.GetSection(SignalrOptions.SETTING));
            services.Configure<SireneOptions>(Configuration.GetSection(SireneOptions.SETTING));
            services.Configure<SponsoringOptions>(Configuration.GetSection(SponsoringOptions.SETTING));
            services.Configure<RoutineOptions>(Configuration.GetSection(RoutineOptions.SETTING));
            services.Configure<PictureOptions>(Configuration.GetSection(PictureOptions.SETTING));
            services.Configure<ImportersOptions>(Configuration.GetSection(ImportersOptions.SETTING));
            services.Configure<ExportersOptions>(Configuration.GetSection(ExportersOptions.SETTING));

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
                options.AddPolicy(Policies.STORE_OR_CONSUMER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(rolesOptions.Consumer.Value, rolesOptions.Store.Value);
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
                options.AddPolicy(Policies.ANONYMOUS_OR_CONNECTED, builder =>
                {
                    builder.RequireAssertion(c => c.User.Identity is not {IsAuthenticated: true} || c.User.Identity.IsAuthenticated);
                });
                
                options.DefaultPolicy = options.GetPolicy(Policies.ANONYMOUS_OR_CONNECTED);
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

            services.AddAutoMapper(typeof(CreateProductProfile).Assembly); 
            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly }.ToArray());
            services.AddHttpClient();

            var databaseConfig = appDatabaseSettings.Get<AppDatabaseOptions>();
            services.AddPooledDbContextFactory<QueryDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x =>
                {
                    x.UseNetTopologySuite();
                    x.CommandTimeout(60);
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

            services.AddScoped<IIdentifierService, IdentifierService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPspService, PspService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPdfGenerator, PdfGenerator>();
            
            services.AddScoped<ISheaftMediatr, SheaftMediatr>();
            services.AddScoped<ISheaftDispatcher, SheaftDispatcher>();
            
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IOrderService, OrderService>();
            
            services.AddScopedDynamic<IProductsFileImporter>(typeof(ExcelProductsImporter).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IProductsFileImporter))));
            services.AddScopedDynamic<IPickingOrdersFileExporter>(typeof(ExcelPickingOrdersExporter).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IPickingOrdersFileExporter))));
            services.AddScopedDynamic<IPurchaseOrdersFileExporter>(typeof(ExcelPurchaseOrdersExporter).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IPurchaseOrdersFileExporter))));
            services.AddScopedDynamic<ITransactionsFileExporter>(typeof(ExcelTransactionsExporter).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITransactionsFileExporter))));
            services.AddScopedDynamic<IBillingsFileExporter>(typeof(ExcelBillingsExporter).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IBillingsFileExporter))));

            services.AddScoped<IProductsImporterFactory, ProductsImporterFactory>();
            services.AddScoped<IPickingOrdersExportersFactory, PickingOrdersExportersFactory>();
            services.AddScoped<IPurchaseOrdersExportersFactory, PurchaseOrdersExportersFactory>();
            services.AddScoped<ITransactionsExportersFactory, TransactionsExportersFactory>();
            services.AddScoped<IBillingsExportersFactory, BillingsExportersFactory>();
            
            services.AddScoped<IDapperContext, DapperContext>();

            var searchConfig = searchSettings.Get<SearchOptions>();
            services.AddScoped<ISearchServiceClient, SearchServiceClient>(_ => new SearchServiceClient(searchConfig.Name, new SearchCredentials(searchConfig.ApiKey)));

            var mailerConfig = mailerSettings.Get<MailerOptions>();
            services.AddScoped<IAmazonSimpleEmailService, AmazonSimpleEmailServiceClient>(_ => new AmazonSimpleEmailServiceClient(mailerConfig.ApiId, mailerConfig.ApiKey, RegionEndpoint.EUCentral1));

            var storageConfig = storageSettings.Get<StorageOptions>();
            services.AddSingleton<CloudStorageAccount>(CloudStorageAccount.Parse(storageConfig.ConnectionString));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            services.AddMemoryCache();

            services.AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(c => c.Name("Query"))
                .AddMutationType(c => c.Name("Mutation"))
                .RegisterGraphQlTypes()
                .RegisterGraphQlQueries()
                .RegisterGraphQlMutations()
                .RegisterGraphQlDataLoaders()
                .AddFiltering()
                .AddSorting()
                .AddType<UploadType>();;
                //.EnableRelaySupport();

            services.AddErrorFilter<SheaftErrorFilter>();

            var cacheConfig = cacheSettings.Get<CacheOptions>();
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = databaseConfig.ConnectionString;
                options.SchemaName = cacheConfig.SchemaName;
                options.TableName = cacheConfig.TableName;
            });

            services.AddOptions();

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

            services.AddRazorTemplating();
            services.AddMvc();
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
                var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<QueryDbContext>>();
                var context = contextFactory.CreateDbContext();
                
                if (!context.AllMigrationsApplied())
                {
                    context.Database.Migrate();
                }

                if (configuration.GetValue<bool?>("InsertDevData") ?? true)
                {
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

                    if (context.BankAccounts.FirstOrDefault(c => c.UserId == admin.Id) == null)
                    {
                        var bankAccount = new BankAccount(Guid.NewGuid(), "Dons", "Sheaft",
                            configuration.GetValue<string>("Psp:Bank:Iban"),
                            configuration.GetValue<string>("Psp:Bank:Bic"),
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

                    var settingsEnum = Enum.GetValues(typeof(SettingKind)).Cast<SettingKind>().ToList();
                    var settings = context.Settings.ToList();
                    var missingSettings = settingsEnum.Except(settings.Select(s => s.Kind));
                    foreach (var missingSetting in missingSettings)
                    {
                        context.Add(new Setting(Guid.NewGuid(), missingSetting.ToString("G"), missingSetting));
                        context.SaveChanges();
                    }

                    var removedSettings = settings.Select(s => s.Kind).Except(settingsEnum);
                    foreach (var removedSetting in removedSettings)
                    {
                        var setting = settings.FirstOrDefault(s => s.Kind == removedSetting);
                        if (setting == null)
                            continue;

                        context.Remove(setting);
                        context.SaveChanges();
                    }
                }
            }

            app.UseRobotsTxt();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIpRateLimiting();
            app.UseWebSockets();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapGraphQL()
                    .RequireAuthorization(Policies.ANONYMOUS_OR_CONNECTED);
            });
            
            RazorTemplateEngine.Initialize();
        }
    }
}
