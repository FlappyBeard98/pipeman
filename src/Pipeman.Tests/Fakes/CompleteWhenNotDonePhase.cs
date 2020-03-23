namespace Pipeman.Tests.Fakes
{
    public class CompleteWhenNotDonePhase: CustomPhase<TestResult<string>,NotDone>
    {
        private readonly PassedItem _item;

        public CompleteWhenNotDonePhase(Logger logger,PassedItem item) : base(logger)
        {
            _item = item;
        }

        public override IPhaseResult Execute()
        {
            Logger.Log($"complete when not done phase, state - {(_item.Break? "done":"not done") }");
            var r = GetResult(_item.Text);
            return _item.Break 
                ? NotDone(new NotDone(r.Messages,  _item.Text))
                : Done(r);
        }
    }
}