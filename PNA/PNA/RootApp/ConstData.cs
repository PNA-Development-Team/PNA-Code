using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace RootApp
{
    public class ConstData
    {
        public static string AppPath
        {
            get { return System.Windows.Forms.Application.StartupPath; }
        }

        public static string AppRootMenuPath
        {
            get
            {
                return Path.Combine(AppPath,"Menu");
            }
        }

        public static string AppIconPath
        {
            get
            {
                return Path.Combine(AppPath, "Icon");
            }
        }
    }
}
