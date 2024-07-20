using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using PumpItUpTrainer.Game.Drawables;
using PumpItUpTrainer.Game.Notes;

namespace PumpItUpTrainer.Game
{
    public partial class MainScreen : Screen
    {
        private NoteScrollPlayer notePlayer = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            AddInternal(notePlayer = new NoteScrollPlayer());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            notePlayer.GenerateAndPlayNotes(100, 1000, 32, Foot.Right, [Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C]);
        }
    }
}
