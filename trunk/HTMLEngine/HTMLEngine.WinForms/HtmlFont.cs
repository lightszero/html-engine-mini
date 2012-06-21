using System;
using System.Diagnostics;
using System.Drawing;

namespace HTMLEngine.WinForms
{
    internal class HtmlFont : HtFont
    {
        private readonly Font font;
        private int whiteSize=0;

        public HtmlFont(string face, int size, bool bold, bool italic)
            : base(face, size, bold, italic)
        {
            FontStyle style = FontStyle.Regular;
            if (bold) style |= FontStyle.Bold;
            if (italic) style |= FontStyle.Italic;
            this.font = new Font(face, size, style,GraphicsUnit.Pixel);
        }

        public override int LineSpacing { get { return this.font.Height; } }

        public override int WhiteSize { get
        {
            if (this.whiteSize==0)
            {
                this.whiteSize = Measure(" ").Width;
            }
            return this.whiteSize;
        } }

        public override HtSize Measure(string text)
        {
            Debug.Assert(HtmlDevice.Context != null);
            var tmp = HtmlDevice.Context.MeasureString(text, this.font);
            return new HtSize((int) tmp.Width, (int) tmp.Height);
        }

        public override void Draw(HtRect rect, HtColor color, string text)
        {
            Debug.Assert(HtmlDevice.Context != null);
            var c = Color.FromArgb(color.A, color.R, color.G, color.B);
            HtmlDevice.Context.DrawString(text, this.font, new SolidBrush(c), rect.X, rect.Y);
        }
    }
}