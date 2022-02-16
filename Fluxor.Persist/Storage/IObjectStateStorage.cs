using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public interface IObjectStateStorage
    {
        public Task<object> GetStateAsync(string statename);
        public Task StoreStateAsync(string statename, object state);
    }
}
