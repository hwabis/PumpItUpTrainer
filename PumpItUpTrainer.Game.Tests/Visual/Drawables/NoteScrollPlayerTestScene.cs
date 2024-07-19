using NUnit.Framework;
using PumpItUpTrainer.Game.Drawables;

namespace PumpItUpTrainer.Game.Tests.Visual.Drawables
{
    [TestFixture]
    public partial class NoteScrollPlayerTestScene : PumpItUpTrainerTestScene
    {
        public NoteScrollPlayerTestScene()
        {
            Add(new NoteScrollPlayer());
        }
    }
}
