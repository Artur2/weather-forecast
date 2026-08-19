using Microsoft.AspNetCore.Components;

namespace WeatherForecast.Server.Components.Modal;

public class ModalComponentBase : ComponentBase
{
    [Parameter]
    public RenderFragment Body { get; set; }
    
    [Parameter]
    public RenderFragment Title { get; set; }
    
    [Parameter]
    public bool Display { get; set; }

    public void ToggleDisplay()
    {
        this.Display = !this.Display;
    }
}