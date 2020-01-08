using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawTool
{
    public class Element2D : ElementBase
    {
        private Point2D m_position;
        public Point2D Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
            }
        }

        private Matrix2D m_transformMatrix;
        public Matrix2D TransformMatrix
        {
            get { return m_transformMatrix; }
            set
            {
                m_transformMatrix = value;
            }
        }

        public Element2D(string name) : base(name)
        {

        }
        public Element2D(string name,RGB color) : base(name,color)
        {

        }

        public bool Move(Point2D offset)
        {
            throw new NotImplementedException();
        }

        public bool Rotate(Matrix2D matrix)
        {
            return Rotate(matrix, this.Position);
        }

        public bool Rotate(Matrix2D matrix,Point2D refPoint)
        {
            throw new NotImplementedException();
        }
    }
}
