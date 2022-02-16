using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public interface IStringStateStorage
    {
        public Task<string> GetStateJsonAsync(string statename);
        public Task StoreStateJsonAsync(string statename, string json);
    }
}
