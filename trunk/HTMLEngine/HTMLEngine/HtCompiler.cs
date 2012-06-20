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

using HTMLEngine.Core;

namespace HTMLEngine
{
    public class HtCompiler : PoolableObject
    {
        internal override void OnAcquire()
        {
            this.d = OP<DeviceChunkCollection>.Acquire();
        }
        internal override void OnRelease()
        {
            this.d.Dispose();
            this.d = null;
        }

        private readonly Reader reader = new Reader();
        
        private DeviceChunkCollection d;

        public int CompiledWidth { get; private set; }
        public int CompiledHeight { get; private set; }

        public string GetLink(int x, int y)
        {
            if (this.d != null)
            {
                for (int link = 0; link < this.d.Links.Count; link++)
                {
                    var data = d.Links[link];
                    if (data.Key.Contains(x, y)) return data.Value;
                }
            }
            return null;
        }

        public void Compile(string source, int width)
        {
            d.Clear();
//            bool debug = HtEngine.LogLevel == HtLogLevel.Debug;
//            DateTime t = default(DateTime);
//            if (debug)
//            {
//                t = DateTime.Now;
//            }
//            HtEngine.Log(HtLogLevel.Debug, "Compiling html...");
            CompiledWidth = width;
            this.reader.SetSource(source);
            {
                using (var h = OP<HtmlChunkCollection>.Acquire())
                {
                    h.Read(reader);
                    this.d.Parse(h, width);

                    CompiledHeight = 0;
                    // get height
                    if (d.Lines.Count > 0)
                    {
                        var line = this.d.Lines[d.Lines.Count - 1];
                        CompiledHeight = line.Y + line.Height;
                    }
                    else
                    {
                        CompiledHeight = 0;
                    }
//                    if (debug)
//                    {
//                        HtEngine.Log(HtLogLevel.Debug, "Compiler time: {0}ms", (DateTime.Now - t).TotalMilliseconds);
//                    }
                }
            }
        }

        public void Draw(float deltaTime)
        {
            if (this.d != null)
            {
                for (int lineIndex = 0; lineIndex < this.d.Lines.Count; lineIndex++)
                {
                    var line = this.d.Lines[lineIndex];
                    for (int chunkIndex = 0; chunkIndex < line.Chunks.Count; chunkIndex++)
                    {
                        var chunk = line.Chunks[chunkIndex];
                        chunk.Draw(deltaTime);
                    }
                }
            }
        }
    }
}