using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Fluxor.Persist.Storage;

public sealed class DictionaryStoreHandler : IStoreHandler
{
    private readonly IObjectStateStorage _store;
    private readonly ILogger _logger;

    public DictionaryStoreHandler(IObjectStateStorage store, ILogger<DictionaryStoreHandler> logger)
    {
        _store = store;
        _logger = logger;
    }

    public async Task<object> GetState(IFeature feature)
    {
        _logger?.LogDebug("Rehydrating state {FeatureName}", feature.GetName());
        var state = await _store.GetStateAsync(feature.GetName());
        if (state == null)
        {
            _logger?.LogDebug("No saved state for {FeatureName}, skipping", feature.GetName());
            return feature.GetState(); //get initial state
        }

        return state;
    }

    public async Task SetState(IFeature feature) => await _store.StoreStateAsync(feature.GetName(), feature.GetState());
}