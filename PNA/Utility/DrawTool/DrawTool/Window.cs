using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL;

namespace DrawTool
{
    public class Window
    {
        private static IntPtr m_WinHandle;
        public static IntPtr WinHandle
        {
            get { return m_WinHandle; }
        }

        private static GLRC m_glrc = null;
        public static GLRC Glrc
        {
            get { return m_glrc; }
        }

        private static int m_windowWidth;
        private static int m_windowHeight;

        private static Matrix3D m_lookAtMatrix;

        private static Point3D m_scale;

        static Window()
        {
            m_WinHandle = IntPtr.Zero;
            m_windowWidth = 0;
            m_windowHeight = 0;
            m_lookAtMatrix = new Matrix3D(0, 0, 1, 0, 0, 0, 0, 1, 0);
            m_scale = new Point3D(1.0, 1.0, 1.0);
        }

        public static bool LoadWindow(System.Windows.Forms.Form window)
        {
            if (window == null || window.IsDisposed || window.Handle == IntPtr.Zero)
                return false;

            if (IsLoadedWindow())
                throw new NotSupportedException("Please unload window before load new window.");

            m_WinHandle = window.Handle;
            m_windowWidth = window.Width;
            m_windowHeight = window.Height;

            m_glrc = new GLRC(m_WinHandle);
            m_glrc.LoadExtensionFunctions();

            gl.Enable(GL.DEPTH_TEST);
            gl.LoadIdentity();

            SetViewDistance();

            return true;
        }

        public static bool UnLoadWindow()
        {
            m_WinHandle = IntPtr.Zero;
            m_glrc = null;
            m_windowWidth = 0;
            m_windowHeight = 0;
            return true;
        }

        public static bool ReLoadWindow()
        {      
            Form window = System.Windows.Forms.Form.FromHandle(m_WinHandle) as Form;
            UnLoadWindow();
            LoadWindow(window);
            return true;
        }

        public static bool IsLoadedWindow()
        {
            return m_WinHandle != IntPtr.Zero;
        }

        public static bool IsLoadedThisWindow(System.Windows.Forms.Form window)
        {
            return m_WinHandle == window.Handle;
        }

        public static bool SetBackgroundColor(Color color)
        {
            RGB tempColor = RGB2OpenGLRGB(new RGB(color));
            //m_glrc.MakeCurrent();
            
            gl.ClearColor(tempColor.R, tempColor.G, tempColor.B, 1);
            gl.Clear(GL.COLOR_BUFFER_BIT);
            gl.Clear(GL.DEPTH_BUFFER_BIT);

            

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

        public static bool ShowGrid(int accuracy)
        {
            return true;
        }

        public static bool Move()
        {
            throw new NotImplementedException();
        }

        public static void ResetViewPoint()
        {
            m_scale.X = 1.0;
            m_scale.Y = 1.0;
            m_scale.Z = 1.0;
            ReLoadWindow();
        }

        private static void SetViewDistance()
        {
            gl.Scale(m_scale.X, m_scale.Y, m_scale.Z);
        }

        public static void ViewLarger()
        {
            m_scale.X++;
            m_scale.Y++;
            m_scale.Z++;
            ReLoadWindow();
        }

        public static void ViewSmaller()
        {
            if (m_scale.X <= 1)
                return;

            m_scale.X--;
            m_scale.Y--;
            m_scale.Z--;
            ReLoadWindow();
        }

        public static bool SetGrid(bool isShow,int unitLength)
        {
            throw new NotImplementedException();
        }

        private static RGB RGB2OpenGLRGB(RGB rgb)
        {
            return new RGB(rgb.R / 255, rgb.G / 255, rgb.B / 255);
        }

        private static Point2D Point2D2OpenGLPoint2D(Point2D position)
        {
            return Point2D2OpenGLPoint2D(position.X, position.Y);
        }

        private static Point2D Point2D2OpenGLPoint2D(double x ,double y)
        {
            if (m_windowHeight == 0 || m_windowWidth == 0)
                throw new NotSupportedException("Please load a window first!");

            if (x < 0 || y < 0)
                throw new NotSupportedException("Position can not be negetive number!");

            return new Point2D(x / m_windowWidth, y / m_windowHeight);
        }

    }
}
