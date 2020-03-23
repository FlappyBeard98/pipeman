namespace Pipeman
{
    public abstract class Phase<TDone,TNotDone> : IPhaseDone<TDone>,IPhaseNotDone<TNotDone>,IPhase<TDone, TNotDone>
    {
        public abstract IPhaseResult Execute();
        
        protected virtual IPhaseResult Done(TDone payload) => new DoneResult<TDone>(payload);
        protected virtual IPhaseResult NotDone(TNotDone payload) => new NotDoneResult<TNotDone>(payload);
    }
}