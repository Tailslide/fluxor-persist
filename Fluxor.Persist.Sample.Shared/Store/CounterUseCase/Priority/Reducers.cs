namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Priority;

public static class Reducers
{
    [ReducerMethod]
    public static CounterStatePriority ReduceIncrementCounterAction(CounterStatePriority state, IncrementCounterPriorityAction action) =>
        state with { ClickCount = state.ClickCount + 1 };
}