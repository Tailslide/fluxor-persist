using Fluxor.Blazor.Web.Components;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Shared.Pages
{
    public partial class CounterStateJson: FluxorComponent
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
            localStorage.Clear();
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
                CounterFromStore["Counter"] = localStorage.GetItemAsString("Counter");
                CounterFromStore["CounterExclude"] = localStorage.GetItemAsString("CounterExclude");
                CounterFromStore["CounterInclude"] = localStorage.GetItemAsString("CounterInclude");

                SubscribeToAction((Action<IncrementCounterAction>)(result => PrintStateDirectly("Counter")));
                SubscribeToAction((Action<IncrementCounterExcludeAction>)(result => PrintStateDirectly("CounterExclude")));
                SubscribeToAction((Action<IncrementCounterIncludeAction>)(result => PrintStateDirectly("CounterInclude")));
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void PrintStateDirectly(string featureName)
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                string json = localStorage.GetItemAsString(featureName);  // poll for localstorage changes for demo purposes.. don't poll get state directly in real applications
                if (json != CounterFromStore[featureName])
                {
                    CounterFromStore[featureName] = json;
                    this.StateHasChanged();
                }
            }), null, 200, 200);
        }
  
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

    }
}
