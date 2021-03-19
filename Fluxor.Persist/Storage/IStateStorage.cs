using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxor.Persist.Storage
{
    public interface IStateStorage
    {
        public ValueTask<string> GetStateJsonAsync(string statename);
        public ValueTask StoreStateJsonAsync(string statename, string json);
    }
}
