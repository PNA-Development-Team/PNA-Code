using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL;

namespace OpenGLTest
{
    public partial class Form1 : Form
    {
        private GLRC glrc;
        public Form1()
        {
            InitializeComponent();

            this.Width = 400;
            this.Height = 400;

            this.Shown += Form1_Shown1;
            this.Paint += Form1_Paint;
            this.Click += Form1_Click;
            this.SizeChanged += Form1_SizeChanged;

            glrc = new GLRC(this.Handle);
            glrc.LoadExtensionFunctions();
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            Draw();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Draw();
        }

        private void Form1_Shown1(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            glrc.MakeCurrent();
            gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            gl.Clear(GL.COLOR_BUFFER_BIT);

            gl.LineWidth(5.0f);
            gl.Enable(GL.LINE_WIDTH);

            gl.LineStipple(1, 0xAAAA);
            gl.Enable(GL.LINE_STIPPLE);

            gl.Begin(GL.LINE_STRIP);
            
            gl.Color3(1, 1, 1);
            gl.Vertex2(-1, 0);
            gl.Vertex2(0, 0);
            gl.Vertex2(1, 0);
            gl.End();
            glrc.SwapBuffers();
        }
    }
}
