using System.Linq;

namespace Pipeman.Tests.Fakes
{
    public abstract class CustomPhase<TDone, TNotDone> : Phase<TDone,TNotDone>
    {
        protected readonly Logger Logger;

        protected CustomPhase(Logger logger)
        {
            Logger = logger;
        }
        
        protected TestResult<T> GetResult<T>(T payload) => new TestResult<T>(Logger.Logs.ToList(),payload);

    }
}