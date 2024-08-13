using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using PumpItUpTrainer.Game.Drawables;

namespace PumpItUpTrainer.Game
{
    public partial class MainScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            NoteVisualizer notePlayer;

            AddInternal(notePlayer = new NoteVisualizer());
            AddInternal(new NoteVisualizerOptionsPanel(notePlayer)
            {
                X = 100,
                Y = 100,
                Scale = new(1.5f, 1.5f),
            });
        }
    }
}
