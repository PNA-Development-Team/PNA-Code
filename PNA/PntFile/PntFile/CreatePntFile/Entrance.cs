using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RootApp;
using PntFile.Forms;

namespace PntFile
{
    public class Entrance : RootApp.UI.IPluginBase
    {
        public void Init()
        {
            RootApp.UI.RegisterMenu.AddMenu("PntFile", "Create Pnt File");
        }

        public void RunCommand(string strCommand)
        {
            if(strCommand == "Create Pnt File")
            {
                CreatePntForm.ShowWindow();
            }
        }
    }
}
