using System;
using System.Linq;
using Pipeman.Tests.Fakes;
using Xunit;
using Xunit.Abstractions;

namespace Pipeman.Tests
{
    public class PipeTests
    {
        [Fact]
        public void PipeCreated()
        {
            var pipe = GetEmptyPipeSut();
            Assert.NotNull(pipe);
        }
        
        [Fact]
        public void WhenItemRegisteredByTypePipeHoldsIt()
        {
            var pipe = GetEmptyPipeSut();
            pipe.Pass<Done>();
            var act = pipe.Items.SingleOrDefault();
            Assert.Equal(typeof(Done),act);
        }
        
        [Fact]
        public void WhenItemRegisteredByValuePipeHoldsIt()
        {
            var pipe = GetEmptyPipeSut();
            pipe.Pass(new PassedItem("",false));
            var act = pipe.Items.SingleOrDefault();
            Assert.Equal(typeof(PassedItem),act);
        }
        
        [Fact]
        public void WhenPhaseRegisteredByTypePipeHoldsIt()
        {
            var pipe = GetEmptyPipeSut();
            pipe.Next<RegularPhase>();
            var act = pipe.Phases.SingleOrDefault();
            Assert.Equal(typeof(RegularPhase),act);
        }
        
        [Fact]
        public void WhenPhaseRegisteredByValuePipeHoldsIt()
        {
            var pipe = GetEmptyPipeSut();
            pipe.Next(new RegularPhase(GetLogger()));
            var act = pipe.Phases.SingleOrDefault();
            Assert.Equal(typeof(RegularPhase),act);
        }
        
        [Fact]
        public void WhenPipeHasCompletePhaseThenPipeExecutionSuccessful()
        {
            var pipe = GetEmptyPipeSut();
            pipe.Pass(GetLogger()).Pass<DummyDependency>().Complete<FinalDummyPhase>();
            var act = pipe.Execute();
            Assert.True(act.IsDone());
        }
        
        [Fact]
        public void WhenPassSameItemMultipleTimesThenOk()
        {
            var pipe = GetEmptyPipeSut();
            pipe.Pass(GetLogger()).Pass<DummyDependency>().Pass<DummyDependency>().Complete<FinalDummyPhase>();
            var act = pipe.Execute();
            Assert.True(act.IsDone());
        }
        
        [Fact]
        public void WhenPipeHasNotCompletePhaseThenPipeExecutionFailed()
        {
            var pipe = GetEmptyPipeSut();
            Assert.ThrowsAny<Exception>(()=>pipe.Execute());
        }
        
        [Fact(Skip = "not implemented yet")]
        public void WhenPipeHasCompleteWhenDonePhaseThenPipeExecutionBreaks()
        {
            var pipe = GetWhenDonePipeSut(true);
            Assert.Null(pipe);
        }
        
        [Fact(Skip = "not implemented yet")]
        public void WhenPipeHasCompleteWhenNotDonePhaseThenPipeExecutionBreaks()
        {
            var pipe = GetWhenDonePipeSut(false);
            Assert.Null(pipe);
        }
        
        [Fact(Skip = "not implemented yet")]
        public void WhenPipeHasCompleteWhenDonePhaseThenPipeExecutionNotBreaks()
        {
            var pipe = GetWhenNotDonePipeSut(true);
            Assert.Null(pipe);
        }
        
        [Fact(Skip = "not implemented yet")]
        public void WhenPipeHasCompleteWhenNotDonePhaseThenPipeExecutionNotBreaks()
        {
            var pipe = GetWhenNotDonePipeSut(false);
            Assert.Null(pipe);
        }
        
        [Fact]
        public void ComplexPipeTest()
        {
            var pipe = GetPipeSut();
            var act = pipe.Execute();
            Assert.NotNull(act);
            Assert.IsType<PipeResult<Done, NotDone>.DoneResult>(act);
        }
        
        private readonly ITestOutputHelper _outputHelper;

        public PipeTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        private Logger GetLogger()=>new Logger(_outputHelper.WriteLine);
        private PassedItem GetItem(bool needBreak) => new PassedItem($"{(needBreak ?  "break" : "pass") }",needBreak);
        private Pipe<Done,NotDone> GetEmptyPipeSut() => new Pipe<Done, NotDone>();
        private IPipe<Done,NotDone> GetPipeSut() => 
            new Pipe<Done, NotDone>()
                .Pass<DummyDependency>()
                .Pass(GetLogger())
                .Pass(GetItem(false))
                .Next<RegularPhase>()
                .CompleteWhenDone<CompleteWhenDonePhase>()
                .CompleteWhenNotDone<CompleteWhenNotDonePhase>()
                .Next<RegularPhase>()
                .Complete<FinalPhase>();
        
        
        private IPipe<Done,NotDone> GetWhenDonePipeSut(bool needBreak) =>
            new Pipe<Done, NotDone>()
                .Pass<DummyDependency>()
                .Pass(GetLogger())
                .Pass(GetItem(needBreak))
                .Next<RegularPhase>()
                .CompleteWhenDone<CompleteWhenDonePhase>()
                .Complete<FinalPhase>();

        private IPipe<Done,NotDone> GetWhenNotDonePipeSut(bool needBreak) =>
            new Pipe<Done, NotDone>()
                .Pass<DummyDependency>()
                .Pass(GetLogger())
                .Pass(GetItem(needBreak))
                .Next<RegularPhase>()
                .CompleteWhenNotDone<CompleteWhenNotDonePhase>()
                .Complete<FinalPhase>();

    }
}