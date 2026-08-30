using WeatherForecast.Server.Components;
using WeatherForecast.Server.Options;

namespace WeatherForecast.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddHttpClient();
        builder.Services.AddLogging();
        builder.Services.AddTransient<APIClientFactory>();
        builder.Configuration.AddJsonFile("defaults.json");
        builder.Configuration.AddJsonFile("grpc_options.json");
        builder.Services.Configure<Defaults>(builder.Configuration.GetSection("defaults"));
        builder.Services.Configure<GrpcOptions>(builder.Configuration.GetSection("grpc_options"));

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