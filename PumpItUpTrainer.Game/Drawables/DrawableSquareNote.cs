using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class DrawableSquareNote : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.Centre;
            Masking = true;
            CornerRadius = 10;

            AddInternal(new Box
            {
                Size = new(75, 75),
                Colour = Colour4.Yellow,
            });
        }
    }
}
