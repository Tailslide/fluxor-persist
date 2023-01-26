namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Include;

public class Feature : Feature<CounterStateInclude>
{
    public override string GetName() => "CounterInclude";
    protected override CounterStateInclude GetInitialState() =>
        new CounterStateInclude(0);
}