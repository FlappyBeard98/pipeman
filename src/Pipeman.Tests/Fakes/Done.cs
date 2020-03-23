using System.Collections.Generic;

namespace Pipeman.Tests.Fakes
{
    public class Done:TestResult<string>
    {
       
        public int Counter { get; }

        public Done(List<string> messages, int counter, string result = default) : base(messages, result)
        {
            Counter = counter;
        }
    }
}