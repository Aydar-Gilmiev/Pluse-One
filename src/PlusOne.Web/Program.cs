using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PlusOne.Web;
using PlusOne.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Базовый адрес API задаётся в wwwroot/appsettings.json (ключ Api:BaseUrl).
// Если он не указан, считаем, что API доступен с того же адреса, что и приложение.
var apiBaseUrl = builder.Configuration["Api:BaseUrl"];
var apiBaseAddress = new Uri(
    new Uri(builder.HostEnvironment.BaseAddress),
    string.IsNullOrWhiteSpace(apiBaseUrl) ? "./" : apiBaseUrl.Trim().TrimEnd('/') + "/");

builder.Services.AddScoped(_ => new OffersApiClient(new HttpClient { BaseAddress = apiBaseAddress }));

await builder.Build().RunAsync();
