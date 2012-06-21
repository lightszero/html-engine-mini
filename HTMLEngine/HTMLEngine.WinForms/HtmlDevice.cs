using System;
using System.Collections.Generic;
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
                image = Image.FromFile(src);
                
            }
            catch
            {
                Console.WriteLine("Could not load image: {0}", src);
                image = new Bitmap(1, 1);
            }
        }

        public override int Width
        {
            get
            {
                Debug.Assert(image != null);
                return image.Width;
            }
        }

        public override int Height
        {
            get
            {
                Debug.Assert(image != null);
                return image.Height;
            }
        }

        public override void Draw(HtRect rect, HtColor color)
        {
            Debug.Assert(HtmlDevice.Context != null);
            HtmlDevice.Context.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        }
    }

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
