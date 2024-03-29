﻿using Blazored.LocalStorage;
using Fluxor.Persist.Storage;
using System.Threading.Tasks;

namespace Fluxor.Persist.Sample.Shared.Storage
{
    public class LocalStateStorage : IStringStateStorage
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
            await LocalStorage.SetItemAsStringAsync(statename, json);
        }
    }
}
