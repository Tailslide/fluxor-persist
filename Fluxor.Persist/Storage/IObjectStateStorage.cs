using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public interface IObjectStateStorage
    {
        public ValueTask<object> GetStateAsync(string statename);
        public ValueTask StoreStateAsync(string statename, object state);
    }
}
