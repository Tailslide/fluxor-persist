namespace Fluxor.Persist.Sample.Shared.Store.CounterUseCase.Exclude
{
    public class Feature : Feature<CounterStateExclude>
    {
        public override string GetName() => "CounterExclude";
        protected override CounterStateExclude GetInitialState() =>
            new CounterStateExclude(0);
    }
}
