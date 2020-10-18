using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System.Configuration;

namespace Sheaft.Web.Api
{
    public class Program
    {
        public int MyProperty { get; set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}