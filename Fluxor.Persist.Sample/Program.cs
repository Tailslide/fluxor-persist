using Blazored.LocalStorage;
using Fluxor.Persist.Middleware;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            var currentAssembly = typeof(Program).Assembly;
            builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = false);
            builder.Services.AddLogging(builder => builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                //.AddFilter("Fluxor", LogLevel.Warning)
                );

            builder.Services.AddFluxor(options => options
                .ScanAssemblies(currentAssembly)
                .UsePersist()
                //.UsePersist(x => x.StateBlackList= "mystate1,mystate2")
                .UseReduxDevTools()
                );
            await builder.Build().RunAsync();
        }
    }
}
