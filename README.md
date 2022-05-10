# Blazor Browser Get Size & Resize Event

A very simple and lightweight example to show how the JavaScript Interop can be used to get the initial size of the browser and then to be notified in any page/component when the browser size changes.

It has been created as a service and would be simple to create as NuGet package.
Although, as there are effectively only four files (and three of them could be combined) I would suggest they are just copied into any project requiring them.

## Setup
The service should be added at start up (in program.cs) and initialized.
*Initialization is required if resize events are required.*

### Setup without events
```csharp
builder.Services.AddScoped<IBrowserService, BrowserService>();
await builder.Build().RunAsync();
```

### Setup with events
```csharp
builder.Services.AddScoped<IBrowserService, BrowserService>();
var host = builder.Build();
await host.Services.GetRequiredService<IBrowserService>().Init();
await host.RunAsync();
```

### Add the JavaScript to the web page

Update index.html (or any other page you are using) to add

```html
    <script src="browser-service.js"></script>
```

## Usage

### Get the current browser windows size

Inject the service into the require component (or class)

```csharp
var dimensions = await BrowserService.GetDimensions()
```

### Handle the resize event

In a component use the following example

```csharp
@inject IBrowserService BrowserService

@code
{
    private BrowserDimensions? _dimensions = null;

    protected override async Task OnInitializedAsync()
    {
        // Note that the event does not automatically fire so you should initialise 
        // using either of the following lines
        _dimensions = BrowserService.Dimensions;                // Read existing/last value
        _dimensions = await BrowserService.GetDimensions();     // Force read the browser size

        // Add the event handler
        BrowserService.OnDimensionsChanged += OnDimensionsChanged;
    }

    private void OnDimensionsChanged(BrowserDimensions dimensions)
    {
        _dimensions = dimensions;
        StateHasChanged();
    }

    public void Dispose()
    {
        // Make sure you dispose of any handlers created
        BrowserService.OnDimensionsChanged -= OnDimensionsChanged;
    }
}
```

## The Model

```csharp
public class BrowserDimensions
{
    public int Width { get; set; }
    public int Height { get; set; }
}
```
