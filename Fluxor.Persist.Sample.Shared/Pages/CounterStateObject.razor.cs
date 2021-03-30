using Fluxor.Blazor.Web.Components;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Sample.Shared.Storage;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Shared.Pages
{
    public partial class CounterStateObject: FluxorComponent
    {
        [Inject] private IState<CounterState> CounterState { get; set; }
        [Inject] private IState<CounterStateExclude> CounterStateExclude { get; set; }
        [Inject] private IState<CounterStateInclude> CounterStateInclude { get; set; }
        [Inject] public IDispatcher Dispatcher { get; set; }

        private Dictionary<string, string> CounterFromStore = new()
        {
            ["Counter"] = "",
            ["CounterExclude"] = "",
            ["CounterInclude"] = ""
        };

        private void IncrementCount()
        {
            var action = new IncrementCounterAction();
            Dispatcher.Dispatch(action);
        }

        private void IncrementCountExclude()
        {
            var action = new IncrementCounterExcludeAction();
            Dispatcher.Dispatch(action);
        }

        private void IncrementCountInclude()
        {
            var action = new IncrementCounterIncludeAction();
            Dispatcher.Dispatch(action);
        }

        protected void ResetStateAndStorage()
        {
            (memoryStorage as InMemoryStateStorage).ClearStore();
            Dispatcher.Dispatch(new ResetAllStatesAction());
        }

        protected void ReloadPage()
        {
            NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {                
                CounterFromStore["Counter"] = JsonSerializer.Serialize(await memoryStorage.GetStateAsync("Counter"));
                CounterFromStore["CounterExclude"] = JsonSerializer.Serialize(await memoryStorage.GetStateAsync("CounterExclude"));
                CounterFromStore["CounterInclude"] = JsonSerializer.Serialize(await memoryStorage.GetStateAsync("CounterInclude"));

                SubscribeToAction((Action<IncrementCounterAction>)(result => PrintStateDirectly("Counter")));
                SubscribeToAction((Action<IncrementCounterExcludeAction>)(result => PrintStateDirectly("CounterExclude")));
                SubscribeToAction((Action<IncrementCounterIncludeAction>)(result => PrintStateDirectly("CounterInclude")));
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void PrintStateDirectly(string featureName)
        {
            var timer = new Timer(new TimerCallback(async _ =>
            {
                var state = await memoryStorage.GetStateAsync(featureName);
                string json = System.Text.Json.JsonSerializer.Serialize(state);
                if (json != CounterFromStore[featureName])
                {
                    CounterFromStore[featureName] = json;
                    await InvokeAsync(() => StateHasChanged());
                }
            }), null, 200, 200);
        }

        protected override async Task OnInitializedAsync()
        {


            await base.OnInitializedAsync();
        }

    }
}
