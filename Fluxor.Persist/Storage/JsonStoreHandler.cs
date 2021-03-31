using Fluxor.Persist.Middleware;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public class JsonStoreHandler : IStoreHandler
    {
        private IStringStateStorage LocalStorage;
        private ILogger<PersistMiddleware> Logger;
        public JsonStoreHandler(IStringStateStorage localStorage, ILogger<PersistMiddleware> logger)
        {
            LocalStorage = localStorage;
            Logger = logger;
        }

        public async Task<object> GetState(IFeature feature)
        {
            Logger?.LogDebug($"Rehydrating state {feature.GetName()}");
            string json = await LocalStorage.GetStateJsonAsync(feature.GetName());
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
                        return stronglyTypedFeatureState;
                }
                catch (Exception ex)
                {
                    Logger?.LogError("Failed to deserialize state. Skipping. Error:" + ex.ToString());
                }
            }
            return feature.GetState(); //get initial state
        }

        public async Task SetState(IFeature feature)
        {
            var state = feature.GetState();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string serializedState = JsonSerializer.Serialize(state, options);
            await LocalStorage.StoreStateJsonAsync(feature.GetName(), serializedState);
        }
    }
}
