using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Sheaft.Api;
using Sheaft.Application;
using Sheaft.Domain;
using Sheaft.Infrastructure;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) =>
        lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
    
    builder.Services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
    builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

    builder.Services.AddWebCommon(builder.Configuration);
    builder.Services.AddCaching(builder.Configuration);
    builder.Services.AddCorsServices(builder.Configuration);
    builder.Services.AddAuthentication(builder.Configuration);
    builder.Services.AddAuthorization(builder.Configuration);

    builder.Services.AddDomain();
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddLogging(config => { config.AddSerilog(dispose: true); });

    builder.Services.RegisterSwagger();
    
    builder.Services.AddRazorPages();
    builder.Services.AddMvc();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHttpsRedirection();
        app.UseHsts();
    }

    app.ApplyMigrations();

    app.UseRobotsTxt();
    app.UseCors("CORS");
    app.UseSerilogRequestLogging();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseWebSockets();
    
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
        {
            AppPath = app.Configuration.GetValue<string>("Portal:Url"),
            Authorization = new List<IDashboardAuthorizationFilter>
                {new HangfireAuthorizationFilter(Policies.AUTHENTICATED)}
        }).RequireAuthorization(Policies.AUTHENTICATED);

        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
    });

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}