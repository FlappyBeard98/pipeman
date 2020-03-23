namespace Pipeman
{
    public interface IPhase
    {
        IPhaseResult Execute();
    }
    
    public interface IPhase<TDone, TNotDone> : IPhase
    {
        
    }
}