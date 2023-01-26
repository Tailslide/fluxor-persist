using Fluxor.Persist.Storage;

namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Priority;

[FeatureState(Name = "CounterStatePriority")]
[PersistState]
[PriorityLoad(PriorityLevel.Highest)]
public record CounterStatePriority
{
    public int ClickCount { get; init; }
}