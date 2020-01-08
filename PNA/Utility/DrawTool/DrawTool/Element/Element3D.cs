using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawTool
{
    public class Element3D : ElementBase
    {
        public Element3D(string name) : base(name)
        {

        }
        public Element3D(string name, RGB color) : base(name, color)
        {

        }
    }
}
