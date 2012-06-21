/* Copyright (C) 2012 Ruslan A. Abdrashitov

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE. */

using System.Diagnostics;
using System.Drawing;

namespace HTMLEngine.WinForms
{
    internal class HtmlFont : HtFont
    {
        private readonly Font font;
        private int wsize=-1;

        public HtmlFont(string face, int size, bool bold, bool italic)
            : base(face, size, bold, italic)
        {
            FontStyle style = FontStyle.Regular;
            if (bold) style |= FontStyle.Bold;
            if (italic) style |= FontStyle.Italic;
            this.font = new Font(face, size, style, GraphicsUnit.Pixel);
        }

        public override int LineSpacing { get { return this.font.Height; } }

        public override int WhiteSize
        {
            get
            {
                if (wsize<0)
                {
                    wsize = this.Measure(". .").Width - this.Measure("..").Width + 1;
                }
                return wsize;
            }
        }

        public override HtSize Measure(string text)
        {
            Debug.Assert(HtmlDevice.Context != null);
            var tmp = HtmlDevice.Context.MeasureString(text, this.font, 0, StringFormat.GenericTypographic);
            return new HtSize((int) tmp.Width, (int) tmp.Height);
        }

        public override void Draw(HtRect rect, HtColor color, string text)
        {
            Debug.Assert(HtmlDevice.Context != null);
            var c = Color.FromArgb(color.A, color.R, color.G, color.B);
            HtmlDevice.Context.DrawString(text, this.font, new SolidBrush(c), rect.X, rect.Y,StringFormat.GenericTypographic);
        }
    }
}