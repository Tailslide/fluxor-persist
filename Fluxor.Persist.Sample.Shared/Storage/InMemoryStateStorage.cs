using Fluxor.Persist.Storage;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Shared.Storage
{
    public class InMemoryStateStorage : IObjectStateStorage
    {
        private ConcurrentDictionary<string, object> _store = new();

        public void ClearStore() => _store.Clear();

        public ValueTask<object> GetStateAsync(string statename)
        {
            if (_store.ContainsKey(statename))
                return ValueTask.FromResult(_store[statename]);
            return default;
        }

        public ValueTask StoreStateAsync(string statename, object state)
        {
            if (_store.ContainsKey(statename))
                _store.TryRemove(statename, out _);
            _store.TryAdd(statename, state);
            return ValueTask.CompletedTask;
        }
    }
}
