using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HTMLEngine.XNA4
{
    internal class HtmlDevice : HtDevice
    {
        public static SpriteBatch Context;
        public static ContentManager Content;
        public static Texture2D WhiteTexture;

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
            if (this.fonts.TryGetValue(key, out ret)) return ret;
            // fail with cache, so create new and store into cache
            ret = new HtmlFont(face, size, bold, italic);
            this.fonts[key] = ret;
            return ret;
        }

        public override HtImage LoadImage(string src)
        {
            if (string.IsNullOrEmpty(src)) src = "error";
            // try get from cache
            HtmlImage ret;
            if (this.images.TryGetValue(src, out ret)) return ret;
            // fail with cache, so create new and store into cache
            ret = new HtmlImage(src);
            this.images[src] = ret;
            return ret;
        }

        public override void FillRect(HtRect rect, HtColor color)
        {
            Debug.Assert(Context != null);
            if (WhiteTexture == null)
            {
                WhiteTexture = new Texture2D(Context.GraphicsDevice, 1, 1);
                WhiteTexture.SetData(new[] {Color.White});
            }
            Context.Draw(WhiteTexture,
                         new Rectangle(rect.X, rect.Y, rect.Width, rect.Height),
                         new Color(color.R, color.G, color.B, color.A));
        }
    }
}