
namespace Aya.Render.Draw
{
    public enum TextureDrawRepeatMode
    {
        InRange = 0,
        RepeatX = 1,
        RepeatY = 2,
        RepeatXY = 3,
    }

    public static partial class TextureDraw
    {
        public static TextureDrawRepeatMode RepeatMode = TextureDrawRepeatMode.InRange;
    }
}
