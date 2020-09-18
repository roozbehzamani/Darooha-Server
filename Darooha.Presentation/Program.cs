using Darooha.Data.DatabaseContext;
using Darooha.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using ZNetCS.AspNetCore.Logging.EntityFrameworkCore;

namespace Darooha.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddNLog();
                    logging.AddEntityFramework<LogDbContext, ExtendLog>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
