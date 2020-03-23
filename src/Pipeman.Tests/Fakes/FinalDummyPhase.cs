using System.Linq;

namespace Pipeman.Tests.Fakes
{
    public class FinalDummyPhase : CustomPhase<Done,NotDone>
    {
        private readonly DummyDependency _dummyDependency;

        public FinalDummyPhase(Logger logger,DummyDependency dummyDependency) : base(logger)
        {
            _dummyDependency = dummyDependency;
        }

        public override IPhaseResult Execute()
        {
            Logger.Log($"final phase ({_dummyDependency.Text})");
            return Done(new Done(Logger.Logs.ToList(), 1,"done"));
        }
    }
}