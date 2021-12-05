using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Crude.Demo.Wasm.Services;
using Crude.Demo.Wasm.ViewModel;

namespace Crude.Demo.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<DummyDataService>();

            builder.Services.AddScoped<ProductTable>();
            builder.Services.AddScoped<UserProfileViewModel>();

            await builder.Build().RunAsync();
        }
    }
}