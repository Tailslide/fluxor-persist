using Fluxor.Persist.Middleware;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public class DictionaryStoreHandler : IStoreHandler
    {
        private readonly IObjectStateStorage _store;
        private ILogger<PersistMiddleware> _logger;


        public DictionaryStoreHandler(IObjectStateStorage store, ILogger<PersistMiddleware> logger)
        {
            _store = store;
            _logger = logger;
        }

        public async Task<object> GetState(IFeature feature)
        {
            _logger?.LogDebug($"Rehydrating state {feature.GetName()}");
            var state = await _store.GetStateAsync(feature.GetName());
            if (state == null)
            {
                _logger?.LogDebug($"No saved state for {feature.GetName()}, skipping");
                return feature.GetState(); //get initial state
            }
            return state;
        }
        public async Task SetState(IFeature feature) => await _store.StoreStateAsync(feature.GetName(), feature.GetState());
    }
}
