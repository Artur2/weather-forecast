using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using WeatherForecast.Models;
using WeatherForecast.Server.Options;

namespace WeatherForecast.Server.Components;

public class WeatherEntriesListComponent : ComponentBase
{
    [Inject]
    protected IOptions<Defaults> Defaults { get; set; }

    [Inject]
    protected APIClientFactory Factory { get; set; }

    protected WeatherForecastEntry[]? _entries;

    protected WeatherForecastEntry? _selectedItem;

    protected bool HoursVisible { get; set; }

    protected void DisplayHours(WeatherForecastEntry indexSelected)
    {
        _selectedItem = indexSelected;
        HoursVisible = true;
    }

    protected override async Task OnInitializedAsync()
    {
        var client = Factory.Create();
        // TODO: Automapper
        var response = await client.GetWeatherForecastAsync(new API.WeatherForecastRequest
        {
            Latitude = Defaults.Value.Latitude,
            Longitude = Defaults.Value.Longitude,
            Days = Defaults.Value.Days
        });

        _entries = response.Entries.Select(x =>
        new WeatherForecastEntry(x.MaxTempC, x.MinTempC, x.Date.ToDateTime(),
            x.Hours.Select(h => new WeatherForecastHourly
            {
                Time = h.Time.ToTimeSpan(),
                TempC = h.TempC
            }).ToArray())
        ).ToArray();
    }
}