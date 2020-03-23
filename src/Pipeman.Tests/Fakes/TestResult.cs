using System.Collections.Generic;

namespace Pipeman.Tests.Fakes
{
    public class TestResult<T> 
    {
        public T Result { get; }
        public  List<string> Messages { get; }

        public TestResult(List<string> messages,T result=default )
        {
            Result = result;
            Messages = messages;
        }
    }
}