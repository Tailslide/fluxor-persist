using Fluxor.Persist.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//using AngleSharp.Text;

namespace Fluxor.Persist.Middleware
{
    public class PersistMiddleware : Fluxor.Middleware
    {
        private IStore Store;
        private PersistMiddlewareOptions Options;
        private IStoreHandler StoreHandler;

        ILogger<PersistMiddleware> Logger { get; set; }
        /// <summary>
        /// Creates a new instance of the middleware
        /// </summary>
        public PersistMiddleware(PersistMiddlewareOptions options, ILogger<PersistMiddleware> logger, IStoreHandler storeHandler)
        {
            Options = options;
            Logger = logger;
            StoreHandler = storeHandler;
        }

        //private IStateStorage localStorage { get; set; }
        /// <see cref="IMiddleware.InitializeAsync(IStore)"/>
        public override async Task InitializeAsync(IStore store)
        {
            Store = store;

            store.SubscribeToAction<ResetAllStatesAction>(this,action =>
            {
                string ErrMsg = "";
                foreach (IFeature feature in Store.Features.Values.OrderBy(x => x.GetName()))
                {
                    if (! Options.ShouldPersistState(feature))
                    {
                        Logger?.LogDebug($"Don't reset {feature.GetName()} state");
                    }
                    else
                    {
                        try
                        {
                            var GetInitialState = feature.GetType().GetMethod("GetInitialState", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                                            null,
                                            CallingConventions.Any,
                                            new Type[] { },
                                            null);
                            var obj = GetInitialState.Invoke(feature, null);
                            feature.RestoreState(obj);
                        }
                        catch (Exception ex)
                        {
                            ErrMsg += ex.ToString() + "\r\n";
                        }
                        if (Store == null || IsInsideMiddlewareChange) ErrMsg += "Store is null or inside middleware change\r\n";

                    }
                }
                if (ErrMsg == "")
                    Store.Dispatch(new ResetAllStatesResultSuccessAction());
                else
                    Store.Dispatch(new ResetAllStatesResultFailAction() { ErrorMessage = ErrMsg });

            });

            await base.InitializeAsync(store);
            
            foreach (IFeature feature in Store.Features.Values.OrderBy(x => x.GetName()))
            {
                if (!Options.ShouldPersistState(feature))
                {
                    Logger?.LogDebug($"Don't persist {feature.GetName()} state");
                    continue;
                }
                else 
                {
                    var restoredState = await StoreHandler.GetState(feature);
                    feature.RestoreState(restoredState);
                }

                Logger?.LogDebug($"Wiring up event for feature {feature.GetName()}");
                feature.StateChanged += Feature_StateChanged;
            }
            //Logger?.LogDebug("Initialized Persist Middleware");
            if (Store != null && !IsInsideMiddlewareChange)
                Store.Dispatch(new InitializePersistMiddlewareResultSuccessAction());
            else
                Store.Dispatch(new InitializePersistMiddlewareResultFailAction());
        }

        private void Feature_StateChanged(object sender, EventArgs e)
        {
            Logger?.LogDebug($"Feature_StateChanged(): sender {sender.ToString()}");
            if (sender is IFeature f)
            {
                if (!Options.ShouldPersistState(f))
                    Logger?.LogDebug($"Don't persist {f.GetName()} state");
                else
                    StoreHandler.SetState(f);
            }
        }
    }
}
