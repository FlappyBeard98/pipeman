namespace Pipeman.Tests.Fakes
{
    public class CompleteWhenDonePhase: CustomPhase<Done,TestResult<string>>
    {
        private readonly PassedItem _item;
        private readonly TestResult<int> _regularResult;

        public CompleteWhenDonePhase(Logger logger,PassedItem item,TestResult<int> regularResult) : base(logger)
        {
            _item = item;
            _regularResult = regularResult;
        }

        public override IPhaseResult Execute()
        {
            Logger.Log($"complete when done phase, state - {(_item.Break? "done":"not done") }");
            var r = GetResult(_regularResult.Result);
            return _item.Break 
                ? Done(new Done(r.Messages, r.Result, _item.Text)) 
                : NotDone(GetResult(_item.Text));

        }
    }
}