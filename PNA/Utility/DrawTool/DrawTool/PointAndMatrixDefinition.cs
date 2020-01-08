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
    }

    public class Matrix2D
    {
        public Point2D Row1;
        public Point2D Row2;

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
        public Matrix2D Matrix;
        public Point2D Position;

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
        public Point3D Row1;
        public Point3D Row2;
        public Point3D Row3;

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
        Matrix3D Matrix;
        Point3D Position;

        public Transform3D()
        {

        }
        public Transform3D(Matrix3D matrix, Point3D position)
        {
            this.Matrix = matrix;
            this.Position = position;
        }
    }

    public class RGB
    {
        public double R;
        public double G;
        public double B;

        public RGB()
        {
            R = 0; G = 0; B = 0;
        }

        public RGB(double r,double g,double b)
        {
            R = r;  G = g; B = b;
        }

        public RGB(RGB color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public enum Color
        {
            RED,BLUE,YELLOW,GREEN,BLACK,WHITE,PURPLE
        }

        public static RGB GetRGB(RGB.Color color)
        {
            switch (color)
            {
                case Color.RED :    return new RGB();
                case Color.BLUE:    return new RGB();
                case Color.YELLOW:  return new RGB();
                case Color.GREEN:   return new RGB();
                case Color.BLACK:   return new RGB();
                case Color.WHITE:   return new RGB();
                case Color.PURPLE:  return new RGB();
                default: return new RGB();
            }
        }
    }
}
