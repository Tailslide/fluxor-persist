using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fluxor.Persist.Storage;

public sealed class JsonStoreHandler : IStoreHandler
{
    private readonly IStringStateStorage _localStorage;
    private readonly ILogger _logger;

    public JsonStoreHandler(IStringStateStorage localStorage, ILogger<JsonStoreHandler> logger)
    {
        _localStorage = localStorage;
        _logger = logger;
    }

    public async Task<object> GetState(IFeature feature)
    {
        _logger?.LogDebug("Rehydrating state {FeatureName}", feature.GetName());
        string json = await _localStorage.GetStateJsonAsync(feature.GetName());
        if (string.IsNullOrEmpty(json))
        {
            _logger?.LogDebug("No saved state for {FeatureName}, skipping", feature.GetName());
        }
        else
        {
            ////Logger?.LogDebug($"Deserializing type {feature.GetStateType().ToString()} from json {json}");
            _logger?.LogDebug("Deserializing type {StateType}", feature.GetStateType());
            try
            {
                object stronglyTypedFeatureState = JsonSerializer.Deserialize(
                    json,
                    feature.GetStateType());
                if (stronglyTypedFeatureState == null)
                {
                    _logger?.LogError($"Deserialize returned null");
                }
                else
                    // Now set the feature's state to the deserialized object
                    return stronglyTypedFeatureState;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to deserialize state. Skipping.");
            }
        }

        return feature.GetState(); //get initial state
    }

    public async Task SetState(IFeature feature)
    {
        try
        {
            var state = feature.GetState();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string serializedState = JsonSerializer.Serialize(state, options);
            await _localStorage.StoreStateJsonAsync(feature.GetName(), serializedState);
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to serialize state. Skipping.");
        }
    }
}