using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class NoteScrollPlayer : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            AddInternal(new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Colour = Colour4.Gray,
                Children =
                [
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -180, X = 40 - 80 * 5 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -90,  X = 40 - 80 * 4 },
                    new DrawableSquareNote { Anchor = Anchor.Centre, X = 40 - 80 * 3 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 0, X = 40 - 80 * 2 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 90, X = 40 - 80 * 1 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -180, X = 40 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = -90, X = 40 + 80 * 1 },
                    new DrawableSquareNote { Anchor = Anchor.Centre, X = 40 + 80 * 2 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 0, X = 40 + 80 * 3 },
                    new DrawableArrowNote { Anchor = Anchor.Centre, Rotation = 90, X = 40 + 80 * 4 },
                ],
            });
        }

        public void GenerateAndPlayNotes(double bpm, double scrollSpeed, int noteCount, List<DrawableArrowNote> allowedNotes)
        {

        }
    }
}
