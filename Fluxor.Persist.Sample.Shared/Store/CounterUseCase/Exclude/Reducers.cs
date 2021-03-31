namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude
{
    public static class Reducers
    {
        [ReducerMethod]
        public static CounterStateExclude ReduceIncrementCounterAction(CounterStateExclude state, IncrementCounterExcludeAction action) =>
            state with { ClickCount = state.ClickCount + 1 };
    }
}
