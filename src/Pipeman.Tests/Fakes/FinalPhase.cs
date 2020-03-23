using System.Linq;

namespace Pipeman.Tests.Fakes
{
    public class FinalPhase : CustomPhase<Done,NotDone>
    {
        private readonly TestResult<int> _counter;
        private readonly TestResult<string> _text;
        private readonly DummyDependency _dummyDependency;

        public FinalPhase(Logger logger,TestResult<int> counter,TestResult<string> text,DummyDependency dummyDependency) : base(logger)
        {
            _counter = counter;
            _text = text;
            _dummyDependency = dummyDependency;
        }

        public override IPhaseResult Execute()
        {
            Logger.Log($"final phase ({_dummyDependency.Text}), state - done");
            var msgs = _counter.Messages.Concat(_text.Messages).Concat(Logger.Logs).ToList();
            return Done(new Done(msgs, 1,"done"));
        }
    }
}