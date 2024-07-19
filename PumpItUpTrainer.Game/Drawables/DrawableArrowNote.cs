using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace PumpItUpTrainer.Game.Drawables
{
    public partial class DrawableArrowNote : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load(TextureStore store)
        {
            Origin = Anchor.Centre;
            Size = new(75, 75);

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
