namespace Pipeman
{
    public interface IPipe<TDone,TNotDone>
    {
        IPipe<TDone,TNotDone> Pass<TItem>(TItem item=default,bool refresh=false)where TItem : class;

        IPipe<TDone, TNotDone> Pass<TItem, TImplementation>(TImplementation item = default,bool refresh=false)
            where TItem : class where TImplementation : class, TItem;
        IPipe<TDone,TNotDone> Next<TPhase>(TPhase phase=default) where TPhase : class,IPhase;
        IPipe<TDone,TNotDone> CompleteWhenDone<TPhase>(TPhase phase=default) where TPhase : class,IPhaseDone<TDone>;
        IPipe<TDone,TNotDone> CompleteWhenNotDone<TPhase>(TPhase phase=default) where TPhase : class,IPhaseNotDone<TNotDone>;
        IPipe<TDone,TNotDone> Complete<TPhase>(TPhase phase=default) where TPhase : class,IPhase<TDone,TNotDone>;
        PipeResult<TDone,TNotDone> Execute();
    }
}