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
            NoteScrollPlayer notePlayer;

            AddInternal(notePlayer = new NoteScrollPlayer());
            AddInternal(new NoteScrollOptionsPanel(notePlayer)
            {
                X = 100,
                Y = 200,
                Scale = new(2, 2),
            });
        }
    }
}
