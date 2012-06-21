using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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

";

        private readonly HtmlDevice htDevice;
        private HtCompiler htCompiler;
        private bool textChanged;

        public Form1()
        {
            InitializeComponent();
            htDevice = new HtmlDevice();
            htCompiler = HtEngine.GetCompiler();
            HtEngine.RegisterDevice(htDevice);
            HtEngine.DefaultFontSize = 14;
            HtEngine.DefaultFontFace = "Arial";
            pictureBox1.BackColor = Color.RoyalBlue;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = demo1;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            HtmlDevice.Context = e.Graphics;
            try
            {
                if (textChanged)
                {
                    htCompiler.Compile(textBox1.Text, pictureBox1.Width);
                    textChanged = false;
                }
                htCompiler.Draw(0);
            }
            finally
            {
                HtmlDevice.Context = null;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (htCompiler != null)
            {
                htCompiler.Dispose();
                htCompiler = null;
            }
            base.OnClosed(e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textChanged = true;
            pictureBox1.Invalidate();
        }
    }
}
