using Fluxor.Persist.Storage;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Shared.Storage
{
    public class InMemoryStateStorage : IObjectStateStorage
    {
        private ConcurrentDictionary<string, object> _store = new();

        public void ClearStore() => _store.Clear();

        public Task<object> GetStateAsync(string statename)
        {
            if (_store.ContainsKey(statename))
                return Task.FromResult(_store[statename]);
            return default;
        }

        public Task StoreStateAsync(string statename, object state)
        {
            if (_store.ContainsKey(statename))
                _store.TryRemove(statename, out _);
            _store.TryAdd(statename, state);
            return Task.CompletedTask;
        }
    }
}
