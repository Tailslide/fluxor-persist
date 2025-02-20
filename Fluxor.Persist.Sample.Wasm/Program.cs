using Blazored.LocalStorage;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Sample.Shared.Storage;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase;
using Fluxor.Persist.Storage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fluxor.Blazor.Web.ReduxDevTools;

namespace Fluxor.Persist.Sample.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage(config => config.JsonSerializerOptions.WriteIndented = false);
            builder.Services.AddScoped<IStringStateStorage, LocalStateStorage>();
            builder.Services.AddScoped<IStoreHandler, JsonStoreHandler>();

            builder.Services.AddLogging(builder => builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                //.AddFilter("Fluxor", LogLevel.Warning)
                );

            builder.Services.AddFluxor(options => options
                .ScanAssemblies(typeof(Program).Assembly, typeof(CounterState).Assembly)
                //******************** UsePersist section ***********************
                //default: will accept all except black listed feature names or states with annotation [SkipPersistState];
                //in the sample, will only persist state for CounterState and CounterStateInclude
                //.UsePersist() 

                //will look either for white listed feature names or states with annotation [PersistState]
                //in the sample, will only persist state for CounterStateInclude
                .UsePersist(options => options.UseInclusionApproach()) 

                //setting white list will automatically switch to UseInclusionApprach
                //in the sample, will only persist state for CounterStateInclude (has [PersistState])
                //and CounterStateExclude (in white list)
                //.UsePersist(options =>
                //    options.SetWhiteList("CounterExclude"))
                //******************** End UsePersist section ***********************
                .UseReduxDevTools()
                );
            await builder.Build().RunAsync();
        }
    }
}
