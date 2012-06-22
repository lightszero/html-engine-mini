using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HTMLEngine.XNA4
{
    internal class HtmlFont : HtFont
    {
        private int wsize = -1;
        private readonly SpriteFont font;

        public HtmlFont(string face, int size, bool bold, bool italic) : base(face, size, bold, italic)
        {
            string key = string.Format("{0}{1}{2}{3}", face, size, bold ? "b" : "", italic ? "i" : "");
            this.font = HtmlDevice.Content.Load<SpriteFont>("fonts/" + key);
        }

        public override int LineSpacing { get { return this.font.LineSpacing; } }

        public override int WhiteSize
        {
            get
            {
                if (this.wsize < 0)
                {
                    this.wsize = this.Measure(" ").Width;
                }
                return this.wsize;
            }
        }

        public override HtSize Measure(string text)
        {
            Vector2 tmp = this.font.MeasureString(text);
            return new HtSize((int) tmp.X, (int) tmp.Y);
        }

        public override void Draw(HtRect rect, HtColor color, string text)
        {
            if (this.font != null)
                HtmlDevice.Context.DrawString(this.font, text, new Vector2(rect.X, rect.Y),
                                              new Color(color.R, color.G, color.B, color.A));
        }
    }
}