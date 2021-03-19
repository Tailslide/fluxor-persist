using Fluxor;
//using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Fluxor.Persist.Storage;
//using AngleSharp.Text;

namespace Fluxor.Persist.Middleware
{
    public class PersistMiddleware : Fluxor.Middleware
    {
        private IStore Store;
        private PersistMiddlewareOptions Options;

        ILogger<PersistMiddleware> Logger { get; set; }
        /// <summary>
        /// Creates a new instance of the middleware
        /// </summary>
        public PersistMiddleware(PersistMiddlewareOptions options, ILogger<PersistMiddleware> logger)
        {
            Options = options;
            Logger = logger;
        }

        private IStateStorage localStorage { get; set; }
        /// <see cref="IMiddleware.InitializeAsync(IStore)"/>
        public override async Task InitializeAsync(IStore store)
        {
            Store = store;

            store.SubscribeToAction<ResetAllStatesAction>(this,action =>
            {
                string ErrMsg = "";
                foreach (IFeature feature in Store.Features.Values.OrderBy(x => x.GetName()))
                {
                    if (! Options.ShouldPersistState(feature.GetName()))
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

            store.SubscribeToAction<InitializePersistMiddlewareAction>(this, async(action) =>
            {
                localStorage = action.StorageService;
                if (action.RehydrateStatesFromStorage)
                {
                    foreach (IFeature feature in Store.Features.Values.OrderBy(x => x.GetName()))
                    {
                        if (! Options.ShouldPersistState(feature.GetName()))
                        {
                            Logger?.LogDebug($"Don't persist {feature.GetName()} state");
                        }
                        else
                        {
                            Logger?.LogDebug($"Rehydrating state {feature.GetName()}");
                            string json = await localStorage.GetStateJsonAsync(feature.GetName());
                            if (json == null)
                            {
                                Logger?.LogDebug($"No saved state for {feature.GetName()}, skipping");
                            }
                            else
                            {
                                ////Logger?.LogDebug($"Deserializing type {feature.GetStateType().ToString()} from json {json}");
                                Logger?.LogDebug($"Deserializing type {feature.GetStateType().ToString()}");
                                try
                                {
                                    object stronglyTypedFeatureState = JsonSerializer.Deserialize(
                                        json,
                                        feature.GetStateType());
                                    if (stronglyTypedFeatureState == null)
                                    {
                                        Logger?.LogError($"Deserialize returned null");
                                    }
                                    else
                                        // Now set the feature's state to the deserialized object
                                        feature.RestoreState(stronglyTypedFeatureState);
                                }
                                catch (Exception ex)
                                {
                                    Logger?.LogError("Failed to deserialize state. Skipping. Error:" + ex.ToString());
                                }
                            }
                        }
                    }
                }
                //Logger?.LogDebug("Initialized Persist Middleware");
                if (Store != null && !IsInsideMiddlewareChange)
                    Store.Dispatch(new InitializePersistMiddlewareResultSuccessAction());
                else
                    Store.Dispatch(new InitializePersistMiddlewareResultFailAction());
            });

            await base.InitializeAsync(store);
            foreach (IFeature feature in Store.Features.Values.OrderBy(x => x.GetName()))
            {
                Logger?.LogDebug($"Wiring up event for feature {feature.GetName()}");
                feature.StateChanged += Feature_StateChanged;
            }
        }

        private void Feature_StateChanged(object sender, EventArgs e)
        {
            Logger?.LogDebug($"Feature_StateChanged(): sender {sender.ToString()}");
            if (sender is IFeature f)
            {
                if (!Options.ShouldPersistState(f.GetName()))
                {
                    Logger?.LogDebug($"Don't persist {f.GetName()} state");
                }
                else
                {
                    var state = f.GetState();
                    //Logger?.LogDebug($"Storing Feature State:{state.ToString()}");
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };
                    string serializedState = JsonSerializer.Serialize(state, options);
                    localStorage.StoreStateJsonAsync(f.GetName(), serializedState);
                }
            }
        }

    }
}
