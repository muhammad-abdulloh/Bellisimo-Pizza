using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace BellisimoPizza.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Logs\\log.txt";
            Log.Logger = new LoggerConfiguration().WriteTo.File(
                    path: path,
                    outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss } " +
                    "[{Level:u3}] {Message} {NewLine} {Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();
            try
            {
                Log.Information("Bellisimo is active");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "BellisimoPizza is fatal error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
