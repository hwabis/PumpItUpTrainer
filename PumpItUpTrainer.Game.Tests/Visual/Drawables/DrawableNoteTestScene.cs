using NUnit.Framework;
using osu.Framework.Graphics;
using PumpItUpTrainer.Game.Drawables;

namespace PumpItUpTrainer.Game.Tests.Visual.Drawables
{
    [TestFixture]
    public partial class DrawableNoteTestScene : PumpItUpTrainerTestScene
    {
        public DrawableNoteTestScene()
        {
            Add(new DrawableArrowNote
            {
                Anchor = Anchor.Centre,
                X = -100,
            });

            Add(new DrawableSquareNote
            {
                Anchor = Anchor.Centre,
                X = 100
            });
        }
    }
}
