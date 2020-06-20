using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawTool
{
    public class Point2D
    {
        public double X;
        public double Y;

        public Point2D()
        {
            X = 0;
            Y = 0;
        }
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point2D(Point2D pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        public Point2D(string xmlData)
        {
            xmlData = xmlData.Trim();
            xmlData = xmlData.Replace("(", "");
            xmlData = xmlData.Replace(")", "");
            List<string> position = xmlData.Split(',').ToList();
            if (position.Count != 2)
                throw new NotSupportedException(xmlData + " is not the string's format of Point2D. Please follow such format \"(X, Y )\".");
            this.X = Convert.ToDouble(position[0]);
            this.Y = Convert.ToDouble(position[1]);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", this.X.ToString(), this.Y.ToString());
        }
    }

    public class Point3D
    {
        public double X;
        public double Y;
        public double Z;

        public Point3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point3D(Point3D pt)
        {
            this.X = pt.X;

            this.Y = pt.Y;
            this.Z = pt.Z;
        }
        public Point2D ToXYPoint2D()
        {
            return new Point2D(this.X, this.Y);
        }

        public Point3D(string xmlData)
        {
            xmlData = xmlData.Trim();
            xmlData = xmlData.Replace("(", "");
            xmlData = xmlData.Replace(")", "");
            List<string> position = xmlData.Split(',').ToList();
            if (position.Count != 3)
                throw new NotSupportedException(xmlData + " is not the string's format of Point3D. Please follow such format \"(X, Y, Z)\".");
            this.X = Convert.ToDouble(position[0]);
            this.Y = Convert.ToDouble(position[1]);
            this.Z = Convert.ToDouble(position[2]);
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", this.X.ToString(), this.Y.ToString(),this.Z.ToString());
        }
    }

    public class Matrix2D
    {
        public Point2D Row1 = new Point2D();
        public Point2D Row2 = new Point2D();

        public Matrix2D()
        {
            Row1.X = 1; Row1.Y = 0;
            Row2.X = 0; Row2.Y = 1;
        }
        public Matrix2D(double row11, double row12, double row21, double row22)
        {
            Row1.X = row11; Row1.Y = row12;
            Row2.X = row21; Row2.Y = row22;
        }

        public Matrix2D(Point2D row1, Point2D row2)
        {
            Row1 = row1;
            Row2 = row2;
        }
    }

    public class Transform2D
    {
        public Matrix2D Matrix = new Matrix2D();
        public Point2D Position = new Point2D();

        public Transform2D()
        {

        }
        public Transform2D(Matrix2D matrix,Point2D position)
        {
            this.Matrix = matrix;
            this.Position = position;
        }
        public Matrix3D ToMatrix3D()
        {
            return new Matrix3D(Matrix.Row1.X, Matrix.Row1.Y, Position.X,
                                Matrix.Row2.X, Matrix.Row2.Y, Position.Y,
                                0, 0, 1);
        }
    }

    public class Matrix3D
    {
        public Point3D Row1 = new Point3D();
        public Point3D Row2 = new Point3D();
        public Point3D Row3 = new Point3D();

        public Matrix3D()
        {
            Row1.X = 1; Row1.Y = 0; Row1.Z = 0;
            Row2.X = 0; Row2.Y = 1; Row2.Z = 0;
            Row3.X = 0; Row3.Y = 0; Row3.Z = 1;
        }
        public Matrix3D(double row11, double row12, double row13,
                        double row21, double row22, double row23,
                        double row31, double row32, double row33)
        {
            Row1.X = row11; Row1.Y = row12; Row1.Z = row13;
            Row2.X = row21; Row2.Y = row22; Row2.Z = row23;
            Row3.X = row31; Row3.Y = row32; Row3.Z = row33;
        }

        public Matrix3D(Point3D row1, Point3D row2, Point3D row3)
        {
            Row1 = row1;
            Row2 = row2;
            Row3 = row3;
        }

        public Matrix3D(Matrix3D matrix)
        {
            this.Row1 = matrix.Row1;
            this.Row2 = matrix.Row2;
            this.Row3 = matrix.Row3;
        }

        public Matrix3D(Transform2D transfrom)
        {
            Matrix3D matrix = transfrom.ToMatrix3D();
            this.Row1 = matrix.Row1;
            this.Row2 = matrix.Row2;
            this.Row3 = matrix.Row3;
        }

        public Matrix2D ToXYMatrix2D()
        {
            return new Matrix2D(this.Row1.ToXYPoint2D(), this.Row2.ToXYPoint2D());
        }

        public Transform2D ToTransform2D()
        {
            return new Transform2D(this.ToXYMatrix2D(),new Point2D(this.Row1.Z,this.Row2.Z));
        }
    }

    public class Transform3D
    {
        Matrix3D Matrix = new Matrix3D();
        Point3D Position = new Point3D();

        public Transform3D()
        {

        }
        public Transform3D(Matrix3D matrix, Point3D position)
        {
            this.Matrix = matrix;
            this.Position = position;
        }
    }

    public enum Color
    {
        RED, BLUE, YELLOW, GREEN, BLACK, WHITE, PURPLE, Grey, Orange,
        SkyBlue, PeachPuff, LemonChiffon, Ivory, LavenderBlush
    }

    public class RGB
    {
        public float R;
        public float G;
        public float B;

        public RGB()
        {
            R = 0; G = 0; B = 0;
        }

        public RGB(float r, float g, float b)
        {
            R = r;  G = g; B = b;
        }

        public RGB(RGB color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public RGB(Color color)
        {
            RGB temp = Helper.GetRGB(color);
            this.R = temp.R;
            this.G = temp.G;
            this.B = temp.B;
        }       
    }

    public partial class Helper
    {
        public static Point2D Point2DZero()
        {
            return new Point2D();
        }

        public static Point3D Point3DZero()
        {
            return new Point3D();
        }

        public static Matrix2D Matrix2DIdentity()
        {
            return new Matrix2D();
        }
        public static Matrix3D Matrix3DIdentity()
        {
            return new Matrix3D();
        }

        public static RGB GetRGB(Color color)
        {
            switch (color)
            {
                case Color.RED: return new RGB(255, 0, 0);
                case Color.BLUE: return new RGB(0, 0, 255);
                case Color.YELLOW: return new RGB(255, 255, 0);
                case Color.GREEN: return new RGB(0, 255, 0);
                case Color.BLACK: return new RGB(0, 0, 0);
                case Color.WHITE: return new RGB(255, 255, 255);
                case Color.PURPLE: return new RGB(155, 48, 255);
                case Color.Grey: return new RGB(190, 190, 190);
                case Color.Orange: return new RGB(255, 165, 0);
                case Color.SkyBlue: return new RGB(135, 206, 255);
                case Color.PeachPuff: return new RGB(205, 175, 149);
                case Color.LemonChiffon: return new RGB(139, 137, 112);
                case Color.Ivory: return new RGB(255, 255, 240);
                case Color.LavenderBlush: return new RGB(255, 240, 245);
                default: return new RGB();
            }
        }
    }
}
