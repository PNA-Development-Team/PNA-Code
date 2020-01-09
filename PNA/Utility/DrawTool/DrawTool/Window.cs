using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace DrawTool
{
    public class Window
    {
        private static IntPtr m_WinHandle = IntPtr.Zero;
        public static IntPtr WinHandle
        {
            get { return m_WinHandle; }
        }

        private static GLRC m_glrc = null;
        public static GLRC Glrc
        {
            get { return m_glrc; }
        }

        public static bool LoadWindow(System.Windows.Forms.Form window,Color color)
        {
            if (window == null || window.IsDisposed || window.Handle == IntPtr.Zero)
                return false;

            if (IsLoadedWindow())
                throw new NotSupportedException("Please unload window before load new window.");

            m_WinHandle = window.Handle;
            m_glrc = new GLRC(m_WinHandle);
            m_glrc.LoadExtensionFunctions();

            return true;
        }

        public static bool UnLoadWindow()
        {
            m_WinHandle = IntPtr.Zero;
            m_glrc = null;
            return true;
        }

        private static bool IsLoadedWindow()
        {
            return m_WinHandle != IntPtr.Zero;
        }

        public static bool SetBackgroundColor(Color color)
        {
            RGB tempColor = RGB2OpenGLRGB(new RGB(color));

            gl.Clear(GL.COLOR_BUFFER_BIT);
            gl.Color3(1f, 0f, 0f);
            gl.Begin(GL.TRIANGLES);
            gl.Vertex2(-0.5f, -0.5f);
            gl.Vertex2(0f, 0.5f);
            gl.Vertex2(0.5f, -0.5f);
            gl.End();
            gl.Flush();
            m_glrc.SwapBuffers();

            return true;
        }

        public static bool Move()
        {
            throw new NotImplementedException();
        }

        public static bool ChangeViewPoint()
        {
            throw new NotImplementedException();
        }

        public static bool SetGrid(bool isShow,int unitLength)
        {
            throw new NotImplementedException();
        }

        private static RGB RGB2OpenGLRGB(RGB rgb)
        {
            return new RGB(rgb.R / 255, rgb.G / 255, rgb.B / 255);
        }
    }
}
