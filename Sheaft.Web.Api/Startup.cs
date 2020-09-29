﻿using AspNetCoreRateLimit;
using AutoMapper;
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
using SendGrid;
using Sheaft.Api.Authorize;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Handlers;
using Sheaft.Application.Interop;
using Sheaft.Application.Mappers;
using Sheaft.Application.Queries;
using Sheaft.Core.Security;
using Sheaft.GraphQL.Services;
using Sheaft.GraphQL.Types;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Services;
using Sheaft.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;

namespace Sheaft.Api
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
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //IdentityModelEventSource.ShowPII = true;
            services.AddSingleton<IConfiguration>(Configuration);

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            var corsSettings = Configuration.GetSection(CorsOptions.SETTING);
            var sendgridSettings = Configuration.GetSection(SendgridOptions.SETTING);
            var searchSettings = Configuration.GetSection(SearchOptions.SETTING);
            var databaseSettings = Configuration.GetSection(DatabaseOptions.SETTING);
            var roleSettings = Configuration.GetSection(RoleOptions.SETTING);
            var cacheSettings = Configuration.GetSection(CacheOptions.SETTING);
            var pspSettings = Configuration.GetSection(PspOptions.SETTING);

            services.Configure<AuthOptions>(authSettings);
            services.Configure<CorsOptions>(corsSettings);
            services.Configure<SendgridOptions>(sendgridSettings);
            services.Configure<SearchOptions>(searchSettings);
            services.Configure<DatabaseOptions>(databaseSettings);
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
            services.Configure<ServiceBusOptions>(Configuration.GetSection(ServiceBusOptions.SETTING));
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
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        RoleClaimType = JwtClaimTypes.Role,
                        NameClaimType = JwtClaimTypes.Subject,
                        AuthenticationType = JwtBearerDefaults.AuthenticationScheme
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

            var databaseConfig = databaseSettings.Get<DatabaseOptions>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x => x.UseNetTopologySuite());//.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null));
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
            services.AddScoped<IUboQueries, UboQueries>();

            services.AddScoped<IDapperContext, DapperContext>();
            services.AddScoped<IAppDbContext>(c => c.GetRequiredService<AppDbContext>());

            var searchConfig = searchSettings.Get<SearchOptions>();
            services.AddScoped<ISearchServiceClient, SearchServiceClient>(_ => new SearchServiceClient(searchConfig.Name, new SearchCredentials(searchConfig.ApiKey)));

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            services.AddScoped<ISendGridClient, SendGridClient>(_ => new SendGridClient(sendgridConfig.ApiKey));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

            services.AddApplicationInsightsTelemetry();

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
                config.ClearProviders();

                config.AddConfiguration(Configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddEventSourceLogger();
                config.AddApplicationInsights();

                if (Env.IsDevelopment())
                {
                    config.AddConsole();
                }
            });

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<IAppDbContext>();
                    if (!context.AllMigrationsApplied())
                    {
                        context.Database.Migrate();
                    }
                }
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIpRateLimiting();

            app.UseStaticFiles();
            app.UseMvc();

            app.UseWebSockets();
            app.UseGraphQL("/graphql");
        }
    }
}