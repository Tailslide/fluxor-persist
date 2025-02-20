using Blazored.LocalStorage;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Sample.Shared.Storage;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase;
using Fluxor.Persist.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fluxor.Persist.Sample.Server.State
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<IObjectStateStorage, InMemoryStateStorage>();
            services.AddSingleton<IStoreHandler, DictionaryStoreHandler>();

            services.AddFluxor(options => options
                .ScanAssemblies(typeof(Program).Assembly, typeof(CounterState).Assembly)
                //******************** UsePersist section ***********************
                //default: will accept all except black listed feature names or states with annotation [SkipPersistState];
                //in the sample, will only persist state for CounterState and CounterStateInclude
                //.UsePersist() 

                //will look either for white listed feature names or states with annotation [PersistState]
                //in the sample, will only persist state for CounterStateInclude
                //.UsePersist(options => options.UseInclusionApproach())

                //setting white list will automatically switch to UseInclusionApprach
                //in the sample, will only persist state for CounterStateInclude (has [PersistState])
                //and CounterStateExclude (in white list)
                .UsePersist(options =>
                    options.SetWhiteList("CounterExclude"))
                //******************** End UsePersist section ***********************
                .UseReduxDevTools()
                ); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
