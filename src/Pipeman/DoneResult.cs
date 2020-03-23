namespace Pipeman
{
    public class DoneResult<T> : IPhaseResult
    {
        public DoneResult(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; }
        public object GetPayload() => Payload;
    }
}