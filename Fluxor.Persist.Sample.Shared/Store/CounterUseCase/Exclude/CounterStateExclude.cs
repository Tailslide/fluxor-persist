using Fluxor.Persist.Storage;

namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude
{
    [SkipPersistState]
    public record CounterStateExclude(int ClickCount);
}
