using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Sheaft.Api
{
    public class Program
    {
        public int MyProperty { get; set; }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}