using BlazorWindowResize.Models;

namespace BlazorWindowResize.Interfaces;

public interface IBrowserService : IDisposable
{
    Task Init();
    Task<BrowserDimensions> GetDimensions();
    BrowserDimensions? Dimensions { get; }
    event Action<BrowserDimensions>? OnDimensionsChanged;
}