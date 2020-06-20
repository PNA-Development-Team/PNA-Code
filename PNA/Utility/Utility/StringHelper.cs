using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class StringHelper
    {
        public static List<string> GetFrontNumAndLastString(string s)
        {
            List<string> strList = new List<string>();
            if (s.Length == 0)
                return strList;

            int i = 0;
            while ((s[i] < '0' || s[i] > '9') && i < s.Length)
                i++;
            int j = i;
            while ((s[j] >= '0' && s[j] <= '9') && j < s.Length)
                j++;
            if (j == s.Length)
                return strList;

            string firstNum = s.Substring(i, j - i);
            string lastStr = s.Substring(j, s.Length - j);
            strList.Add(firstNum);
            strList.Add(lastStr);
            return strList;
        }
    }
}
