using System;

namespace Pipeman
{
    public static class Helper
    {
        public static PipeResult<TDone, TNotDone> FromPhaseResult<TDone, TNotDone>(this IPhaseResult phaseResult) =>
            phaseResult switch
            {
                DoneResult<TDone> r => PipeResult<TDone, TNotDone>.Done(r.Payload),
                NotDoneResult<TNotDone> r => PipeResult<TDone, TNotDone>.NotDone(r.Payload),
                {} r =>
                throw new ArgumentException(
                    $"expected {typeof(PipeResult<TDone, TNotDone>).Name}, passed {r.GetType().Name}",
                    nameof(phaseResult)),
                _ => throw new Exception("somethig goes wrong")
            };
        
        public static T GetResult<T>(this PipeResult<T, T> pipeResult) =>
            pipeResult switch
            {
                PipeResult<T, T>.DoneResult r => r.Payload,
                PipeResult<T, T>.NotDoneResult r =>  r.Payload,
                _ => throw new Exception("somethig goes wrong")
            };
    }
}