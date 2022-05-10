using BlazorWindowResize;
using BlazorWindowResize.Interfaces;
using BlazorWindowResize.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IBrowserService, BrowserService>();
var host = builder.Build();
await host.Services.GetRequiredService<IBrowserService>().Init();
await host.RunAsync();
