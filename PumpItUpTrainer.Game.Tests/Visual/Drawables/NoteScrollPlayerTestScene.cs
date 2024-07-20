using NUnit.Framework;
using PumpItUpTrainer.Game.Drawables;
using PumpItUpTrainer.Game.Notes;

namespace PumpItUpTrainer.Game.Tests.Visual.Drawables
{
    [TestFixture]
    public partial class NoteScrollPlayerTestScene : PumpItUpTrainerTestScene
    {
        private NoteScrollPlayer notePlayer;

        public NoteScrollPlayerTestScene()
        {
            Add(notePlayer = new NoteScrollPlayer());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            notePlayer.GenerateAndPlayNotes(100, 1000, 32, Foot.Right, [Note.P1C, Note.P1UR, Note.P1DR, Note.P2DL, Note.P2UL, Note.P2C]);
        }
    }
}
