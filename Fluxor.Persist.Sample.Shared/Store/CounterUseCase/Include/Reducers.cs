namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include
{
    public static class Reducers
    {
        [ReducerMethod]
        public static CounterStateInclude ReduceIncrementCounterAction(CounterStateInclude state, IncrementCounterIncludeAction action) =>
            state with { ClickCount = state.ClickCount + 1 };
    }
}
