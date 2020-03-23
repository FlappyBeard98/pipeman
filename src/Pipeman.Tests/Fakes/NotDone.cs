using System.Collections.Generic;

namespace Pipeman.Tests.Fakes
{
    public class NotDone : TestResult<string>
    {
        public NotDone(List<string> messages, string result = default) : base(messages, result)
        {
        }
    }
}