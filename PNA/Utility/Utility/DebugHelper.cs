using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Utility
{
    public class DebugHelper
    {
        public static string LogFilePath
        {
            get { return Path.Combine(System.Windows.Forms.Application.StartupPath, "log.txt"); }
        }

        public static void Log(string logMessage)
        {
            FileStream logFile = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(logFile);
            string str = string.Format("[{0}] : {1}",DateTime.Now.ToString(),logMessage);
            sw.WriteLine(str);
            sw.Close();
        }
    }
}
