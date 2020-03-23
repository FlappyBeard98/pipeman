namespace Pipeman
{
    public class NotDoneResult<T> : IPhaseResult
    {
        public NotDoneResult(T payload)
        {
            Payload = payload;
        }

        public T Payload { get; }
        public object GetPayload() => Payload;
    }
}