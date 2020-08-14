using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
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
using Microsoft.Extensions.Logging;
using SendGrid;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.Interop;
using Sheaft.Options;

namespace Sheaft.Manage
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Env = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            var databaseSettings = Configuration.GetSection(DatabaseOptions.SETTING);
            var sendgridSettings = Configuration.GetSection(SendgridOptions.SETTING);
            var roleSettings = Configuration.GetSection(RoleOptions.SETTING);

            services.Configure<RoleOptions>(roleSettings);
            services.Configure<AuthOptions>(authSettings);
            services.Configure<DatabaseOptions>(databaseSettings);
            services.Configure<SendgridOptions>(sendgridSettings);

            services.Configure<StorageOptions>(Configuration.GetSection(StorageOptions.SETTING));
            services.Configure<ServiceBusOptions>(Configuration.GetSection(ServiceBusOptions.SETTING));

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
                options.AddPolicy("AdminOrSupport", o =>
                {
                    o.RequireAuthenticatedUser();
                    o.RequireRole(rolesOptions.Admin.Value, rolesOptions.Support.Value);
                });
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var databaseConfig = databaseSettings.Get<DatabaseOptions>();
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x => x.UseNetTopologySuite());//.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null));
            }, ServiceLifetime.Scoped);

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            services.AddScoped<ISendGridClient, SendGridClient>(c => new SendGridClient(sendgridConfig.ApiKey));

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

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        RoleClaimType = JwtClaimTypes.Role,
                        NameClaimType = JwtClaimTypes.Subject
                    };
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();

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
                config.ClearProviders();

                config.AddConfiguration(Configuration.GetSection("Logging"));
                config.AddDebug();
                config.AddEventSourceLogger();

                if (Env.IsDevelopment())
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
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
