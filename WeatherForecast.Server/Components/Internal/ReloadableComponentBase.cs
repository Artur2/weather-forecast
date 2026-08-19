using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace WeatherForecast.Server.Components.Internal;

public class ReloadableComponentBase : ComponentBase
{
    protected ErrorBoundary? errorBoundary;

    [Parameter] public RenderFragment? RenderComponent { get; set; }

    public void ReloadComponent()
    {
        errorBoundary?.Recover();
    }
}