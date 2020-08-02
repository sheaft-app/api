using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SendGrid;
using Sheaft.Options;
using Sheaft.Services;
using Sheaft.Services.Interop;
using Sheaft.Signalr.Controllers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sheaft.Signalr
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authSettings = Configuration.GetSection(AuthOptions.SETTING);
            services.Configure<AuthOptions>(authSettings);

            var corsSettings = Configuration.GetSection(CorsOptions.SETTING);
            services.Configure<CorsOptions>(corsSettings);

            services.Configure<StorageOptions>(Configuration.GetSection(StorageOptions.SETTING));

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

            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IQueueService, QueueService>();

            services.AddHttpClient();
            services.AddAuthorization();

            var authConfig = authSettings.Get<AuthOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = authConfig.Url;
                options.Audience = authConfig.ApiName;
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
                //.AddIdentityServerAuthentication(options =>
                //{
                //    options.Authority = Configuration.GetValue<string>("Urls:Auth");
                //    options.ApiName = "api";
                //    options.EnableCaching = true;
                //    options.CacheDuration = TimeSpan.FromMinutes(5);
                //    options.Events = new JwtBearerEvents
                //    {
                //        OnMessageReceived = context =>
                //        {
                //            var accessToken = context.Request.Query["access_token"];

                //            // If the request is for our hub...
                //            var path = context.HttpContext.Request.Path;
                //            if (!string.IsNullOrEmpty(accessToken) &&
                //                (path.StartsWithSegments("/hubs/sheaft")))
                //            {
                //                // Read the token out of the query string
                //                context.Token = accessToken;
                //            }
                //            return Task.CompletedTask;
                //        },
                //        OnAuthenticationFailed = context =>
                //        {
                //            return Task.CompletedTask;
                //        },
                //        OnChallenge = context =>
                //        {
                //            return Task.CompletedTask;
                //        },
                //        OnForbidden = context =>
                //        {
                //            return Task.CompletedTask;
                //        },
                //        OnTokenValidated = context =>
                //        {
                //            return Task.CompletedTask;
                //        }
                //    };
                //});

            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            services.AddApplicationInsightsTelemetry();

            services.AddMvc();
            services.AddSignalR();

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHttpsRedirection();
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

        public class NameUserIdProvider : IUserIdProvider
        {
            public string GetUserId(HubConnectionContext connection)
            {
                var userId = connection.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return userId;
            }
        }
    }
}
