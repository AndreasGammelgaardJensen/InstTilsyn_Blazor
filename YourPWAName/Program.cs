using Blazored.LocalStorage;
using Gravam.Blazor.DeviceInterop.Geolocation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.NetworkInformation;
using YourPWAName;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IGeolocationService, GeolocationService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddBlazorBootstrap();
await builder.Build().RunAsync();
