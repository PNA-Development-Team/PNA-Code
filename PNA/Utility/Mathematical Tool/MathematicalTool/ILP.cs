using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using LpSolveDotNet;

namespace MathematicalTool
{
    public class ILP
    {
        private string m_filePath = string.Empty;
        private List<string> m_variate = new List<string>();
        private Dictionary<string, double> m_result = new Dictionary<string, double>();
        public bool Load(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool Caculate()
        {
            throw new NotImplementedException();
        } 

        public Dictionary<string,double> GetResult()
        {
            throw new NotImplementedException();
        }
    }
}
