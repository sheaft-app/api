using System.Collections.Generic;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Sheaft.Api.Authorize;
using Sheaft.Api.Extensions;
using Sheaft.Api.Security;
using Sheaft.Application;
using Sheaft.Infrastructure;

namespace Sheaft.Api
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
                .ReadFrom.Configuration(configuration);

            if (!Env.IsProduction())
                logger = logger
                    .WriteTo.Async(a => a.Console());

            Log.Logger = logger.CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            
            services.AddWebCommon();
            services.AddCaching(Configuration);
            services.AddCorsServices(Configuration);
            services.AddAuthentication(Configuration);
            services.AddAuthorization(Configuration);
            
            services.AddApplicationServices(Configuration);
            
            services.AddInfrastructureServices(Configuration);
            
            services.AddLogging(config =>
            {
                config.AddSerilog(dispose: true);
            });
            
            services.AddMvc();
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
            app.UseCors("cors");
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            
            app.UseInfrastructure();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboardWithAuthorizationPolicy(Policies.HANGFIRE, "/hangfire", new DashboardOptions
                {
                    AppPath = Configuration.GetValue<string>("Portal:Url"),
                    Authorization = new List<IDashboardAuthorizationFilter> { new HangfireAuthorizationFilter(Policies.HANGFIRE)}
                });
                
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
