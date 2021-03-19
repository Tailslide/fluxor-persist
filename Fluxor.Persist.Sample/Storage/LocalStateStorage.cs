using Blazored.LocalStorage;
using Fluxor.Persist.Storage;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Storage
{
    public class LocalStateStorage :IStateStorage
    {

        private ILocalStorageService LocalStorage { get; set; }

        public LocalStateStorage(ILocalStorageService localStorage)
        {
            LocalStorage = localStorage;
        }

        public async ValueTask<string> GetStateJsonAsync(string statename)
        {
            return await LocalStorage.GetItemAsStringAsync(statename);
        }

        public async ValueTask StoreStateJsonAsync(string statename, string json)
        {
            await LocalStorage.SetItemAsync(statename, json);
        }
    }
}
