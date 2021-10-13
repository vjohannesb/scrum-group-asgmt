using BlazorApp.Services;
using BlazorApp.StateContainers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IAPIService, APIService>();

            builder.Services.AddSingleton<CompareState>();
            builder.Services.AddSingleton<ModalState>();
            builder.Services.AddSingleton<OrderState>();
            builder.Services.AddSingleton<WishlistState>();

            await builder.Build().RunAsync();
        }
    }
}
