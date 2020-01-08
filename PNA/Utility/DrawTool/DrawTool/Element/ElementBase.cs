using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace DrawTool
{
    public abstract class ElementBase
    {
        private static int m_globalId = 0;
        private static int GetNewId()
        {
            return ++m_globalId;
        }

        private int m_Id;
        public int Id { get { return m_Id; } }

        private string m_name = string.Empty;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private RGB m_color;
        public RGB Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        public ElementBase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new NotSupportedException("Name can not be empty when create ElementBase.");
            
            m_Id = GetNewId();
            m_name = name;
        }
        public ElementBase(string name, RGB color)
        {
            if (string.IsNullOrEmpty(name))
                throw new NotSupportedException("Name can not be empty when create ElementBase.");

            m_Id = GetNewId();
            m_color = color;
            m_name = name;
        }

        public virtual bool CreateElement()
        {
            throw new NotImplementedException();
        }
        public virtual bool CreateElement(string xmlData)
        {
            throw new NotImplementedException();
        }
        public virtual bool Draw()
        {
            throw new NotImplementedException();
        }
        public virtual bool Updata()
        {
            throw new NotImplementedException();
        }
        public virtual bool Delete()
        {
            throw new NotImplementedException();
        }
        public virtual bool ToXmlData()
        {
            throw new NotImplementedException();
        }
    }
}
