namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase
{
    public static class Reducers
    {
        [ReducerMethod]
        public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action) =>
            state with { ClickCount = state.ClickCount + 1 };
    }
}
