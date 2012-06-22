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
using System.Windows.Forms;

namespace HTMLEngine.WinForms
{
    public partial class Form1 : Form
    {
        private const string demo1 =
            @"<p align=center><font face='Arial Black' size=18><font color=yellow>HTMLEngine</font>for<font color=lime>Windows.Forms</font><br>(c) 2012 Profixy</font></p>
<p align=left>Without effect:</p>
<p align=center>Normal text<u>underlined</u><s>striked</s></p>
<p align=center><b>Bold text<u>underlined</u><s>striked</s></b></p>
<p align=center><i>Italic text<u>underlined</u><s>striked</s></i></p>
<p align=center><b><i>Bold and italic text<u>underlined</u><s>striked</s></i></b></p>
<p align=left>Shadow effect:</p>
<effect name=shadow color=black>
<p align=center>Normal text<u>underlined</u><s>striked</s></p>
<p align=center><b>Bold text<u>underlined</u><s>striked</s></b></p>
<p align=center><i>Italic text<u>underlined</u><s>striked</s></i></p>
<p align=center><b><i>Bold and italic text<u>underlined</u><s>striked</s></i></b></p>
</effect>
<p align=left>Outline effect:</p>
<effect name=outline color=black>
<p align=center>Normal text <u>underlined</u><s>striked</s></p>
<p align=center><b>Bold text<u>underlined</u><s>striked</s></b></p>
<p align=center><i>Italic text<u>underlined</u><s>striked</s></i></p>
<p align=center><b><i>Bold and italic text<u>underlined</u><s>striked</s></i></b></p>
</effect>
<br><p align=center valign=top>Picture <img src='smiles/sad.gif'> with &lt;p valign=top&gt;</p>
<br><p align=center valign=middle>Picture <img src='smiles/smile.gif'> with &lt;p valign=middle&gt; much better than others in this case <img src='smiles/cool'></p>
<br><p align=center valign=bottom>Picture <img src='smiles/sad.gif'> with &lt;p valign=bottom&gt;</p>
<br><p align=center valign=middle><a href='textandimage'>Simple text and <img src='smiles/wink.gif'> image link.</a></p>
";

        private const string demo2 = @"
<div valign=middle>
<div width=200><img src='logos/html.png'></div>
<div>Another Hello World!!! Another Hello World!!! Another Hello World!!! Another Hello World!!! Another Hello World!!! Another Hello World!!! Another Hello World!!! Another Hello World!!!</div>
<div width=100><img src='logos/html.png' width=100 height=100></div>
<div>Yet another Hello World!!! Yet another Hello World!!! Yet another Hello World!!! Yet another Hello World!!! <img src='smiles/cool.gif'></div>
</div>
";

        private const string demo = demo2;

        private readonly HtmlDevice htDevice;
        private HtCompiler htCompiler;
        private bool textChanged;

        public Form1()
        {
            this.InitializeComponent();
            this.htDevice = new HtmlDevice();
            this.htCompiler = HtEngine.GetCompiler();
            HtEngine.RegisterDevice(this.htDevice);
            HtEngine.DefaultFontSize = 14;
            HtEngine.DefaultFontFace = "Arial";
            this.pictureBox1.BackColor = Color.RoyalBlue;

            this.pictureBox1.MouseMove +=
                (sender, args) =>
                {
                    this.pictureBox1.Cursor = this.htCompiler.GetLink(args.X, args.Y) == null
                                                  ? Cursors.Default
                                                  : Cursors.Hand;
                };

            this.pictureBox1.MouseClick += (sender, args) =>
            {
                string link = this.htCompiler.GetLink(args.X, args.Y);
                if (link != null)
                    MessageBox.Show("Link clicked: " + link);
            };
        }

        private void Form1_Load(object sender, EventArgs e) { this.textBox1.Text = demo; }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            string tmp = string.Empty;
            Stopwatch sw;
            HtmlDevice.Context = e.Graphics;
            try
            {
                if (this.textChanged)
                {
                    sw = Stopwatch.StartNew();
                    this.htCompiler.Compile(this.textBox1.Text, this.pictureBox1.Width);
                    sw.Stop();
                    tmp = string.Format("Compile time: {0}ms ", sw.ElapsedMilliseconds);
                    this.textChanged = false;
                }
                sw = Stopwatch.StartNew();
                this.htCompiler.Draw(0);
                sw.Stop();
                tmp += string.Format("Draw time: {0}ms ", sw.ElapsedMilliseconds);
            }
            finally
            {
                HtmlDevice.Context = null;
                this.Text = tmp;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.textChanged = true;
            this.pictureBox1.Invalidate();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (this.htCompiler != null)
            {
                this.htCompiler.Dispose();
                this.htCompiler = null;
            }
            base.OnClosed(e);
        }
    }
}