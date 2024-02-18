using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddTransient<TimeOfDayService>();
                });
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", async context =>
                        {
                            var timeOfDayService = context.RequestServices.GetService<TimeOfDayService>();
                            var timeOfDay = timeOfDayService.GetTimeOfDay(DateTime.Now);
                            context.Response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                            await context.Response.WriteAsync($"Зараз {timeOfDay}", Encoding.UTF8);
                        });
                    });
                });
            });
}

public class TimeOfDayService
{
    public string GetTimeOfDay(DateTime currentTime)
    {
        if (currentTime.Hour >= 12 && currentTime.Hour < 18)
        {
            return "день";
        }
        else if (currentTime.Hour >= 18 && currentTime.Hour < 24)
        {
            return "вечір";
        }
        else if (currentTime.Hour >= 0 && currentTime.Hour < 6)
        {
            return "ніч";
        }
        else
        {
            return "ранок";
        }
    }
}