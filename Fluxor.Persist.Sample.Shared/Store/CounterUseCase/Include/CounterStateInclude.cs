using Fluxor.Persist.Storage;

namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include
{
    [PersistState, PriorityLoad]
    public record CounterStateInclude(int ClickCount);
}
