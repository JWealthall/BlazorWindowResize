using BlazorWindowResize.Interfaces;
using BlazorWindowResize.Models;
using Microsoft.JSInterop;

namespace BlazorWindowResize.Services;

public class BrowserService : IBrowserService
{
    private readonly IJSRuntime _js;
    private readonly ILogger<BrowserService> _logger;
    private DotNetObjectReference<BrowserService>? _dotNetHelper;
    private BrowserDimensions? _dimensions;

    public event Action<BrowserDimensions>? OnDimensionsChanged;

    public BrowserService(IJSRuntime js, ILogger<BrowserService> logger)
    {
        _js = js;
        _logger = logger;
    }

    public async Task<BrowserDimensions> GetDimensions()
    {
        _dimensions = await _js.InvokeAsync<BrowserDimensions>("getDimensions");
        OnDimensionsChanged?.Invoke(_dimensions);
        return _dimensions;
    }

    public BrowserDimensions? Dimensions => _dimensions;

    public async Task Init()
    {
        _logger.LogInformation("Initializing Browser Service");
        _dimensions = await _js.InvokeAsync<BrowserDimensions>("getDimensions");
        _dotNetHelper = DotNetObjectReference.Create(this);
        await _js.InvokeAsync<BrowserDimensions>("startResizeHandler", _dotNetHelper);
    }

    [JSInvokable]
    public void BrowserResized(BrowserDimensions dimensions)
    {
        if (_logger.IsEnabled(LogLevel.Debug)) _logger.LogDebug($"Browser Resize: {dimensions.Width}x{dimensions.Height}");
        _dimensions = dimensions;
        OnDimensionsChanged?.Invoke(dimensions);
    }

    public void Dispose()
    {
        _dotNetHelper?.Dispose();
    }
}