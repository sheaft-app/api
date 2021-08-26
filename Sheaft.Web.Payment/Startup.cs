using Hangfire;
using MangoPay.SDK;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewRelic.LogEnrichers.Serilog;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Services;
using Sheaft.Core.Options;
using Sheaft.Infrastructure.Services;
using Sheaft.Mediatr;
using Sheaft.Web.Common;

namespace Sheaft.Web.Payment
{
    public class Startup
    {
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
            var pspSettings = Configuration.GetSection(PspOptions.SETTING);
            var jobsDatabaseSettings = Configuration.GetSection(JobsDatabaseOptions.SETTING);

            services.Configure<JobsDatabaseOptions>(jobsDatabaseSettings);
            services.Configure<PspOptions>(pspSettings);

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
                        
            services.AddScoped<IPspService, PspService>();
            services.AddScoped<ISheaftMediatr, SheaftMediatr>();

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddScoped<IBackgroundJobClient, BackgroundJobClient>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddOptions();

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRobotsTxt();

            app.UseStaticFiles();
            app.UseMvc(endpoints =>
                {
                    endpoints.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
