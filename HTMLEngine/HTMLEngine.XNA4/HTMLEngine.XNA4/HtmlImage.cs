using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HTMLEngine.XNA4
{
    internal class HtmlImage : HtImage
    {
        private readonly Texture2D texture;

        public HtmlImage(string src) { this.texture = HtmlDevice.Content.Load<Texture2D>(src); }

        public override int Width { get { return this.texture == null ? 0 : this.texture.Width; } }

        public override int Height { get { return this.texture == null ? 0 : this.texture.Height; } }

        public override void Draw(HtRect rect, HtColor color)
        {
            Debug.Assert(HtmlDevice.Context != null);
            if (this.texture != null)
            {
                HtmlDevice.Context.Draw(this.texture, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height),
                                        Color.White);
            }
        }
    }
}