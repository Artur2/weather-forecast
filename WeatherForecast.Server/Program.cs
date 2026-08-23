using WeatherForecast.Abstractions;
using WeatherForecast.Commands;
using WeatherForecast.Server.Components;
using WeatherForecast.Server.Internal;
using WeatherForecast.Server.Options;

namespace WeatherForecast.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Configuration.AddJsonFile("defaults.json");
        builder.Configuration.AddJsonFile("weather_forecast.json");
        builder.Services.Configure<Defaults>(builder.Configuration.GetSection("defaults"));
        builder.Services.Configure<WeatherForecastDefaults>(builder.Configuration.GetSection("weather_forecast"));
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddHttpClient();

        ServicesConfiguration.ConfigureServices(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}