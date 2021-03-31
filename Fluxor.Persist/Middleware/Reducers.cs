using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fluxor.Persist.Middleware
{
    public static class Reducers
    {
        [ReducerMethod]
        public static PersistMiddlewareState ReduceInitializePersistMiddlewareResultSuccessAction(PersistMiddlewareState state, InitializePersistMiddlewareResultSuccessAction action) =>
        state with { Status= InitializationStatus.SUCCEEDED  }; //immutable

        [ReducerMethod]
        public static PersistMiddlewareState ReduceInitializePersistMiddlewareResultFailAction(PersistMiddlewareState state, InitializePersistMiddlewareResultFailAction action) =>
        state with { Status = InitializationStatus.FAILED }; //immutable

        // Reset all states to default
        [ReducerMethod]
        public static PersistMiddlewareState ReduceResetAllStatesAction(PersistMiddlewareState state, ResetAllStatesAction action) =>
        state with { DefaultStatus = ResetToDefaultStatus.INITIALIZING }; //immutable

        [ReducerMethod]
        public static PersistMiddlewareState ReduceResetAllStatesResultFailAction(PersistMiddlewareState state, ResetAllStatesResultFailAction action) =>
        state with { DefaultStatus = ResetToDefaultStatus.FAILED, ResetToDefaultError=action.ErrorMessage }; //immutable

        [ReducerMethod]
        public static PersistMiddlewareState ReduceResetAllStatesResultSucceedAction(PersistMiddlewareState state, ResetAllStatesResultSuccessAction action) =>
        state with { DefaultStatus = ResetToDefaultStatus.SUCCEEDED, ResetToDefaultError="" }; //immutable


    }
}
