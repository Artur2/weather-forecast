using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using WeatherForecast.Commands;
using WeatherForecast.Models;
using WeatherForecast.Server.Options;

namespace WeatherForecast.Server.Components;

public class WeatherEntriesListComponent : ComponentBase
{
    [Inject] public IOptions<Defaults> Defaults { get; set; }

    [Inject] public IMediator Mediator { get; set; }

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
        _entries = await Mediator.Send(
            new GetWeatherForecastCommand(
                Defaults.Value.Latitude,
                Defaults.Value.Longitude,
                Defaults.Value.Days));
    }
}