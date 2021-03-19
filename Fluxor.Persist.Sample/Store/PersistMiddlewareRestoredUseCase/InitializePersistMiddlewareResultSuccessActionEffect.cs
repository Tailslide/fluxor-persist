using System;
using System.Threading.Tasks;

namespace Fluxor.Persist.Middleware
{
    public class InitializePersistMiddlewareResultSuccessActionEffect : Effect<InitializePersistMiddlewareResultSuccessAction>
    {
        public override Task HandleAsync(InitializePersistMiddlewareResultSuccessAction action, IDispatcher dispatcher)
        {
            Console.WriteLine("State rehydrated-- you can do something here like manually rehydrate objects that are not state aware");
            
            dispatcher.Dispatch(new PersistMiddlewareRehydratedAction());
            return Task.CompletedTask;
        }
    }
}
