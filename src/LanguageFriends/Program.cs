using LanguageFriends;
using LanguageFriends.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GameData>();
builder.Services.AddScoped<SoundService>();
builder.Services.AddScoped<Settings>();

await builder.Build().RunAsync();
