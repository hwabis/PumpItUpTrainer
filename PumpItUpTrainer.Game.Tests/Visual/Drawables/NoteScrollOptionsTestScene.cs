using NUnit.Framework;
using PumpItUpTrainer.Game.Drawables;

namespace PumpItUpTrainer.Game.Tests.Visual.Drawables
{
    [TestFixture]
    public partial class NoteScrollOptionsTestScene : PumpItUpTrainerTestScene
    {
        public NoteScrollOptionsTestScene()
        {
            NoteScrollPlayer notePlayer;

            Add(notePlayer = new NoteScrollPlayer());
            Add(new NoteScrollOptionsPanel(notePlayer)
            {
                X = 100,
                Y = 200
            });
        }
    }
}
