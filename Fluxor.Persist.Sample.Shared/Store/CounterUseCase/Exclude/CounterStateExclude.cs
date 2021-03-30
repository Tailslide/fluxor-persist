using Fluxor.Persist.Storage;

namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude
{
    public record CounterStateExclude(int ClickCount) : ISkipPersistState;
}
