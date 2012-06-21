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

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HTMLEngine.WinForms
{
    internal class HtmlImage : HtImage
    {
        private readonly Image image;

        public HtmlImage(string src)
        {
            src = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? ".", src);
            try
            {
                this.image = Image.FromFile(src);
                
            }
            catch
            {
                Console.WriteLine("Could not load image: {0}", src);
                this.image = new Bitmap(1, 1);
            }
        }

        public override int Width
        {
            get
            {
                Debug.Assert(this.image != null);
                return this.image.Width;
            }
        }

        public override int Height
        {
            get
            {
                Debug.Assert(this.image != null);
                return this.image.Height;
            }
        }

        public override void Draw(HtRect rect, HtColor color)
        {
            Debug.Assert(HtmlDevice.Context != null);
            HtmlDevice.Context.DrawImage(this.image, rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}