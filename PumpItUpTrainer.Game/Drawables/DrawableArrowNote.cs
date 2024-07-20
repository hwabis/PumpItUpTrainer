using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class DrawableArrowNote : CompositeDrawable
    {
        public DrawableArrowNote()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.Centre;
            Size = new(75, 75);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            AddInternal(new Sprite
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Texture = store.Get("arrow.png"),
                Scale = new(2, 2)
            });
        }
    }
}
