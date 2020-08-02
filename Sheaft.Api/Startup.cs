using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sheaft.Application.Commands;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Interop;
using System;
using System.IdentityModel.Tokens.Jwt;
using Sheaft.Services;
using Microsoft.Azure.Search;
using System.Reflection;
using System.Collections.Generic;
using Sheaft.Application.Events;
using Sheaft.Application.Handlers;
using Sheaft.Application.Queries;
using Sheaft.Services.Interop;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using HotChocolate.Execution.Configuration;
using AutoMapper;
using Sheaft.Mappers;
using Sheaft.Core.Security;
using SendGrid;
using Sheaft.Options;
using System.Linq;
using Sheaft.GraphQL;
using Sheaft.GraphQL.Types;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Logging;

namespace Sheaft.Api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            var corsSettings = Configuration.GetSection(CorsOptions.SETTING);
            var sendgridSettings = Configuration.GetSection(SendgridOptions.SETTING);
            var searchSettings = Configuration.GetSection(SearchOptions.SETTING);
            var databaseSettings = Configuration.GetSection(DatabaseOptions.SETTING);

            services.Configure<AuthOptions>(authSettings);
            services.Configure<CorsOptions>(corsSettings);
            services.Configure<SendgridOptions>(sendgridSettings);
            services.Configure<SearchOptions>(searchSettings);
            services.Configure<DatabaseOptions>(databaseSettings);

            services.Configure<ApiOptions>(Configuration.GetSection(ApiOptions.SETTING));
            services.Configure<FreshdeskOptions>(Configuration.GetSection(FreshdeskOptions.SETTING));
            services.Configure<LandingOptions>(Configuration.GetSection(LandingOptions.SETTING));
            services.Configure<PortalOptions>(Configuration.GetSection(PortalOptions.SETTING));
            services.Configure<ScoringOptions>(Configuration.GetSection(ScoringOptions.SETTING));
            services.Configure<SearchOptions>(Configuration.GetSection(SearchOptions.SETTING));
            services.Configure<SendgridOptions>(Configuration.GetSection(SendgridOptions.SETTING));
            services.Configure<SignalrOptions>(Configuration.GetSection(SignalrOptions.SETTING));
            services.Configure<SireneOptions>(Configuration.GetSection(SireneOptions.SETTING));
            services.Configure<SponsoringOptions>(Configuration.GetSection(SponsoringOptions.SETTING));
            services.Configure<StorageOptions>(Configuration.GetSection(StorageOptions.SETTING));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.API, builder =>
                {
                    builder.RequireScope("api.all");
                    builder.RequireAuthenticatedUser();
                });
                options.AddPolicy(Policies.AUTHENTICATED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                });
                options.AddPolicy(Policies.REGISTERED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.PRODUCER, RoleNames.STORE, RoleNames.CONSUMER);
                });
                options.AddPolicy(Policies.STORE_OR_PRODUCER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.PRODUCER, RoleNames.STORE);
                });
                options.AddPolicy(Policies.UNREGISTERED, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.ANONYMOUS);
                });
                options.AddPolicy(Policies.OWNER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.OWNER);
                });
                options.AddPolicy(Policies.PRODUCER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.PRODUCER);
                });
                options.AddPolicy(Policies.STORE, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.STORE);
                });
                options.AddPolicy(Policies.CONSUMER, builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole(RoleNames.CONSUMER);
                });
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var authConfig = authSettings.Get<AuthOptions>();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = authConfig.Url;
                    options.ApiName = authConfig.ApiName;
                    options.EnableCaching = authConfig.Caching;
                    options.CacheDuration = TimeSpan.FromSeconds(authConfig.CacheDurationInSeconds);
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

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

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddMediatR(new List<Assembly>() { typeof(RegisterCompanyCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(AccountCommandsHandler).Assembly }.ToArray());
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

            services.AddScoped<IAgreementQueries, AgreementQueries>();
            services.AddScoped<ICompanyQueries, CompanyQueries>();
            services.AddScoped<IDeliveryQueries, DeliveryQueries>();
            services.AddScoped<IDepartmentQueries, DepartmentQueries>();
            services.AddScoped<IJobQueries, JobQueries>();
            services.AddScoped<ILeaderboardQueries, LeaderboardQueries>();
            services.AddScoped<INotificationQueries, NotificationQueries>();
            services.AddScoped<IPackagingQueries, PackagingQueries>();
            services.AddScoped<IProductQueries, ProductQueries>();
            services.AddScoped<IPurchaseOrderQueries, PurchaseOrderQueries>();
            services.AddScoped<IQuickOrderQueries, QuickOrderQueries>();
            services.AddScoped<IRegionQueries, RegionQueries>();
            services.AddScoped<ITagQueries, TagQueries>();
            services.AddScoped<IUserQueries, UserQueries>();

            services.AddScoped<IDapperContext, DapperContext>();
            services.AddScoped<IAppDbContext>(c => c.GetRequiredService<AppDbContext>());

            var searchConfig = searchSettings.Get<SearchOptions>();
            services.AddScoped<ISearchServiceClient, SearchServiceClient>(c => new SearchServiceClient(searchConfig.Name, new SearchCredentials(searchConfig.ApiKey)));

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            services.AddScoped<ISendGridClient, SendGridClient>(c => new SendGridClient(sendgridConfig.ApiKey));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddGraphQL(sp => SchemaBuilder.New()
                .AddServices(sp)
                .AddAuthorizeDirectiveType()
                .ModifyOptions(c => c.DefaultBindingBehavior = BindingBehavior.Explicit)
                .AddType<TimeSpanType>()
                .AddMutationType<SheaftMutationType>()
                .AddQueryType<SheaftQueryType>()
                .RegisterTypes()
                .Create(), new QueryExecutionOptions {IncludeExceptionDetails = true});

            services.AddErrorFilter<SheaftErrorFilter>();

            services.AddApplicationInsightsTelemetry();

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

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

            services.AddOptions();
            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddLogging(config =>
            {
                config.ClearProviders();

                config.AddConfiguration(Configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddEventSourceLogger();
                config.AddApplicationInsights();

                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
                {
                    config.AddConsole();
                }
            });
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
