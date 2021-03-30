using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public interface IStringStateStorage
    {
        public ValueTask<string> GetStateJsonAsync(string statename);
        public ValueTask StoreStateJsonAsync(string statename, string json);
    }
}
