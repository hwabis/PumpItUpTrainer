using NUnit.Framework;
using PumpItUpTrainer.Game.Drawables;

namespace PumpItUpTrainer.Game.Tests.Visual.Drawables
{
    [TestFixture]
    public partial class NoteScrollOptionsTestScene : PumpItUpTrainerTestScene
    {
        public NoteScrollOptionsTestScene()
        {
            NoteVisualizer notePlayer;

            Add(notePlayer = new NoteVisualizer());
            Add(new NoteVisualizerOptionsPanel(notePlayer)
            {
                X = 100,
                Y = 200
            });
        }
    }
}
