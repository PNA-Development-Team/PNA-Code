using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RootApp;

namespace ViewUtility
{
    public class Entrance : RootApp.UI.IPluginBase
    {
        public void Init()
        {
            RootApp.UI.RegisterMenu.AddMenu("ViewUtility", "Set Grid");
        }

        public void RunCommand(string strCommand)
        {
            if(strCommand == "Set Grid")
            {
                SetGridForm.ShowWindow();
                RootApp.UI.UI.ShowGrid(SetGridForm.IsShowGrid,SetGridForm.Accuracy);
            }
        }
    }
}

