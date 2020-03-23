namespace Pipeman
{
    public abstract class PipeResult<TDone, TNotDone>
    {
        public class DoneResult : PipeResult<TDone,TNotDone>
        {
            public DoneResult(TDone payload)
            {
                Payload = payload;
            }

            public TDone Payload { get; }
            public override bool IsDone() => true;
        }
        
        public class NotDoneResult : PipeResult<TDone,TNotDone>
        {
            public NotDoneResult(TNotDone payload)
            {
                Payload = payload;
            }

            public TNotDone Payload { get; }
            public override bool IsDone() => false;
        }
        
        public static PipeResult<TDone,TNotDone> Done(TDone payload) => new DoneResult(payload);
        public static PipeResult<TDone,TNotDone> NotDone(TNotDone payload) => new NotDoneResult(payload);

        public abstract bool IsDone();

        
    }
}