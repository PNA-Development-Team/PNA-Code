using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenGL;

namespace DrawTool
{
    public static class Window
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

        private static int m_windowWidth = 0;
        private static int m_windowHeight = 0;

        private static Matrix3D m_lookAtMatrix = new Matrix3D(0,0,1,
                                                              0,0,0,
                                                              0,1,0);

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

            //选择投影矩阵
            gl.MatrixMode(GL.PROJECTION);
            //载入单位矩阵到投影矩阵
            gl.LoadIdentity();

            //设置透视投影
            gl.Frustum(-50, 50, -50, 50, 50, 100);
            //设置正交投影
            //GL.glOrtho(-50, 50, -50, 50, 0.1, 50);
            //生成观察矩阵
  
            //当前矩阵乘以观察矩阵
            gl.MultMatrix(GetLookAtMatrix().ToArray1D<float>());

            //切换回模型矩阵
            gl.MatrixMode(GL.MODELVIEW);

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
            UnLoadWindow();
            Form window = System.Windows.Forms.Form.FromHandle(m_WinHandle) as Form;
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
            m_glrc.MakeCurrent();
            
            gl.ClearColor(tempColor.R, tempColor.G, tempColor.B, 1);
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
            m_lookAtMatrix = new Matrix3D(0, 0, 1, 0, 0, 0, 0, 1, 0);
        }

        public static bool ChangeViewPoint(int lookHeight)
        {
            m_lookAtMatrix.Row1.Z = lookHeight;
            ReLoadWindow();
            return true;
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

        private static MAT4<float> Matrix3D2OpenGlLookAtMatrix(Matrix3D matrix)
        {
            MAT4<float> lookAtMatrix = GLMATH.LookAtMatrix(new VEC3<float>((float)matrix.Row1.X, (float)matrix.Row1.Y, (float)matrix.Row1.Z),
                                                           new VEC3<float>((float)matrix.Row2.X, (float)matrix.Row2.Y, (float)matrix.Row2.Z),
                                                           new VEC3<float>((float)matrix.Row3.X, (float)matrix.Row3.Y, (float)matrix.Row3.Z));
            return lookAtMatrix;
        }

        private static MAT4<float> GetLookAtMatrix()
        {
            return Matrix3D2OpenGlLookAtMatrix(m_lookAtMatrix);
        }
    }
}
