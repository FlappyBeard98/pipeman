namespace Pipeman.Tests.Fakes
{
    public class RegularPhase: CustomPhase<TestResult<int>,int>
    {
        public RegularPhase(Logger logger) : base(logger)
        {
        }

        public override IPhaseResult Execute()
        {
            Logger.Log("regular phase, state - done");
            return Done(GetResult(int.MaxValue));
        }
    }
}