using Fluxor.Blazor.Web.Components;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include;
using Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Priority;
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
        [Inject] private IState<CounterStatePriority> CounterStatePriority { get; set; }

        [Inject] public IDispatcher Dispatcher { get; set; }

        private Dictionary<string, string> CounterFromStore = new() 
        { 
            ["Counter"] = "",
            ["CounterExclude"] = "",
            ["CounterInclude"] = "",
            ["CounterPriority"] = "",
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

        private void IncrementCountPriority()
        {
            Dispatcher.Dispatch(new IncrementCounterPriorityAction());
        }

        protected async Task ResetStateAndStorage()
        {
            await localStorage.ClearAsync();
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
                CounterFromStore["Counter"] = await localStorage.GetItemAsStringAsync("Counter");
                CounterFromStore["CounterExclude"] = await localStorage.GetItemAsStringAsync("CounterExclude");
                CounterFromStore["CounterInclude"] = await localStorage.GetItemAsStringAsync("CounterInclude");

                SubscribeToAction((Action<IncrementCounterAction>)(result => PrintStateDirectly("Counter")));
                SubscribeToAction((Action<IncrementCounterExcludeAction>)(result => PrintStateDirectly("CounterExclude")));
                SubscribeToAction((Action<IncrementCounterIncludeAction>)(result => PrintStateDirectly("CounterInclude")));
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void PrintStateDirectly(string featureName)
        {

            var timer = new Timer(new TimerCallback(async _ =>
            {
                try
                {
                    string json = await localStorage.GetItemAsStringAsync(featureName);  // poll for localstorage changes for demo purposes.. don't poll get state directly in real applications
                    if (json != CounterFromStore[featureName])
                    {
                        CounterFromStore[featureName] = json;
                        await InvokeAsync(() => StateHasChanged());
                    }
                }
                catch (Microsoft.JSInterop.JSDisconnectedException )
                {
                    /// Started in .NET 6 .. ignore ?? https://github.com/dotnet/aspnetcore/issues/38822
                }
            }), null, 200, 200);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

    }
}
