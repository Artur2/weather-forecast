using Bunit;
using WeatherForecast.Server.Components.Modal;
using WeatherForecast.Server.Components.Pages;

namespace WeatherForecast.Server.Tests;

public class WeatherPageTests : BunitContext
{
    [Fact]
    public void Should_Display_Daily_Forecasts()
    {
        ServicesConfiguration.ConfigureServices(Services);
        TestsServicesConfiguration.ConfigureServices(Services);

        var component = Render<WeatherPage>();

        var forecastsTable = component.Find(".table");
        Assert.NotNull(forecastsTable);
    }

    [Fact]
    public async Task Should_Display_Hourly_Forecast()
    {
        ServicesConfiguration.ConfigureServices(Services);
        TestsServicesConfiguration.ConfigureServices(Services);

        var component = Render<WeatherPage>();

        var forecastsTable = component.Find(".table");
        Assert.NotNull(forecastsTable);

        var buttons = forecastsTable.GetElementsByTagName("button");
        Assert.NotNull(buttons);

        await buttons[0].ClickAsync();

        var modal = component.FindComponent<ModalComponent>();
        Assert.NotNull(modal);

        var tds = modal.FindAll("td");
        foreach (var i in Enumerable.Range(0, 23))
        {
            var time = $"{i:00}:00";

            Assert.Contains(tds, (td) => td.InnerHtml == time);
        }
    }

    [Fact]
    public async Task Should_Hide_Modal_On_Close_Click()
    {
        ServicesConfiguration.ConfigureServices(Services);
        TestsServicesConfiguration.ConfigureServices(Services);

        var component = Render<WeatherPage>();

        var forecastsTable = component.Find(".table");
        Assert.NotNull(forecastsTable);

        var buttons = forecastsTable.GetElementsByTagName("button");
        Assert.NotNull(buttons);

        await buttons[0].ClickAsync();

        var modal = component.FindComponent<ModalComponent>();
        Assert.NotNull(modal);

        var closeButton = modal.Find("button");
        await closeButton.ClickAsync();

        Assert.Contains("display: none", modal.Markup);
        Assert.Equal(2, modal.RenderCount); // Fired event of StateHasChanged
    }
}