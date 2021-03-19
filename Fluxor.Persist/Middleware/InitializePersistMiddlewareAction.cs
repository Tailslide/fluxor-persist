using Fluxor.Persist.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluxor.Persist.Middleware
{
    public class InitializePersistMiddlewareAction
    {
        public IStateStorage StorageService { get; set; } // in the case of localStorage, has to be initialized after we have acccess to JS runtime

        public bool RehydrateStatesFromStorage { get; set; }
    }
}
