using osu.Framework.Testing;

namespace PumpItUpTrainer.Game.Tests.Visual
{
    public abstract partial class PumpItUpTrainerTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new PumpItUpTrainerTestSceneTestRunner();

        private partial class PumpItUpTrainerTestSceneTestRunner : PumpItUpTrainerGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
