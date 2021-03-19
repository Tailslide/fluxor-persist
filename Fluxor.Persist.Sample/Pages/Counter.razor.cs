using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Fluxor.Persist.Sample.Store.CounterUseCase;
using Fluxor.Persist.Middleware;
using System.Threading;

namespace Fluxor.Persist.Sample.Pages
{
	public partial class Counter
	{
		[Inject]
		private IState<CounterState> CounterState { get; set; }

		[Inject]
		public IDispatcher Dispatcher { get; set; }

		private string CounterJson { get; set; }

		private void IncrementCount()
		{
			var action = new IncrementCounterAction();
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

		protected override async Task OnInitializedAsync()
		{
			CounterJson = localStorage.GetItemAsString("Counter");
			this.SubscribeToAction<IncrementCounterAction>(result =>
			{
				var timer = new Timer(new TimerCallback(_ =>
				{
					string json = localStorage.GetItemAsString("Counter");  // poll for localstorage changes for demo purposes.. don't poll get state directly in real applications
                    if (json != CounterJson)
                    {
                        CounterJson = json;
                        this.StateHasChanged();
                    }
				}), null, 200, 200);
			});
			await base.OnInitializedAsync();
        }

	}
}
