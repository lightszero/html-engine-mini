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

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace HTMLEngine.WinForms
{
    class HtmlDevice : HtDevice
    {
        public static Graphics Context = null;

        /// <summary>
        /// Fonts cache (to do not load every time from resouces)
        /// </summary>
        private readonly Dictionary<string, HtmlFont> fonts = new Dictionary<string, HtmlFont>();
        /// <summary>
        /// Image cache (same thing)
        /// </summary>
        private readonly Dictionary<string, HtmlImage> images = new Dictionary<string, HtmlImage>();

        public override HtFont LoadFont(string face, int size, bool bold, bool italic)
        {
            // try get from cache
            string key = string.Format("{0}{1}{2}{3}", face, size, bold ? "b" : "", italic ? "i" : "");
            HtmlFont ret;
            if (fonts.TryGetValue(key, out ret)) return ret;
            // fail with cache, so create new and store into cache
            ret = new HtmlFont(face, size, bold, italic);
            fonts[key] = ret;
            return ret;            
        }

        public override HtImage LoadImage(string src)
        {
            // try get from cache
            HtmlImage ret;
            if (images.TryGetValue(src, out ret)) return ret;
            // fail with cache, so create new and store into cache
            ret = new HtmlImage(src);
            images[src] = ret;
            return ret;
        }

        public override void FillRect(HtRect rect, HtColor color)
        {
            Debug.Assert(Context != null);
            var c = Color.FromArgb(color.A, color.R, color.G, color.B);
            Context.FillRectangle(new SolidBrush(c), rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
