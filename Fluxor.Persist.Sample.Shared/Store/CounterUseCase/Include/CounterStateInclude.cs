using Fluxor.Persist.Storage;

namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include
{
    public record CounterStateInclude(int ClickCount) : IPersistState;
}
