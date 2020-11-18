using Hangfire;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using NewRelic.LogEnrichers.Serilog;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Sheaft.Application.Commands;
using Sheaft.Application.Interop;
using Sheaft.Options;
using Sheaft.Web.Common;
using Sheaft.Web.Signalr.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sheaft.Web.Signalr
{
    public partial class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = Configuration.GetValue<bool?>("ShowPII") ?? false;

            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly }.ToArray());
            services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();
            services.AddScoped<ISheaftHangfireBridge, SheaftHangfireBridge>();
            services.AddScoped<ISheaftMediatr, SheaftMediatr>();

            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddLogging(config =>
            {
                config.AddSerilog(dispose: true);
            });

            var jobsDatabaseSettings = Configuration.GetSection(JobsDatabaseOptions.SETTING);
            services.Configure<JobsDatabaseOptions>(jobsDatabaseSettings);

            var jobsDatabaseConfig = jobsDatabaseSettings.Get<JobsDatabaseOptions>();
            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(jobsDatabaseConfig.ConnectionString);
                configuration.UseSerializerSettings(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
            });

            var corsSettings = Configuration.GetSection(CorsOptions.SETTING);
            services.Configure<CorsOptions>(corsSettings);

            var corsConfig = corsSettings.Get<CorsOptions>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(corsConfig.Origins.ToArray())
                        .WithHeaders(corsConfig.Headers.ToArray())
                        .WithMethods(corsConfig.Methods.ToArray())
                        .AllowCredentials();
                });
            });
                        
            services.AddAuthorization();

            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            services.Configure<AuthOptions>(authSettings);

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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs/sheaft")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddOptions();
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRobotsTxt();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseWebSockets();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SheaftHub>("/hubs/sheaft", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
