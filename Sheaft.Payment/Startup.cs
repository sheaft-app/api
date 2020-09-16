using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using MangoPay.SDK;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SendGrid;
using Sheaft.Application.Commands;
using Sheaft.Application.Events;
using Sheaft.Application.Handlers;
using Sheaft.Infrastructure;
using Sheaft.Mappers;
using Sheaft.Options;
using Sheaft.Services;
using Sheaft.Services.Interop;

namespace Sheaft.Payment
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Env = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            var sendgridSettings = Configuration.GetSection(SendgridOptions.SETTING);
            var databaseSettings = Configuration.GetSection(DatabaseOptions.SETTING);
            var pspSettings = Configuration.GetSection(PspOptions.SETTING);

            services.Configure<SendgridOptions>(sendgridSettings);
            services.Configure<DatabaseOptions>(databaseSettings);
            services.Configure<PspOptions>(pspSettings);

            services.Configure<ApiOptions>(Configuration.GetSection(ApiOptions.SETTING));
            services.Configure<StorageOptions>(Configuration.GetSection(StorageOptions.SETTING));
            services.Configure<ServiceBusOptions>(Configuration.GetSection(ServiceBusOptions.SETTING));

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

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

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddMediatR(new List<Assembly>() { typeof(RegisterStoreCommand).Assembly, typeof(UserPointsCreatedEvent).Assembly, typeof(UserCommandsHandler).Assembly }.ToArray());
            services.AddHttpClient();

            var databaseConfig = databaseSettings.Get<DatabaseOptions>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(databaseConfig.ConnectionString, x => x.UseNetTopologySuite());//.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null));
            }, ServiceLifetime.Scoped);

            var sendgridConfig = sendgridSettings.Get<SendgridOptions>();
            services.AddScoped<ISendGridClient, SendGridClient>(c => new SendGridClient(sendgridConfig.ApiKey));

            services.AddScoped<IIdentifierService, IdentifierService>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignalrService, SignalrService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPspService, PspService>();


            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddApplicationInsightsTelemetry();
            services.AddOptions();

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
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
