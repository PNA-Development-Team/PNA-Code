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
            RootApp.UI.RegisterMenu.AddMenu("ViewUtility", "Mathematical Tool");
            RootApp.UI.RegisterMenu.AddMenu("ViewUtility", "Mathematical Tool", "Integer Linear Programming");
            RootApp.UI.RegisterMenu.AddMenu("ViewUtility", "Mathematical Tool", "Matrix Operations");
        }

        public void RunCommand(string strCommand)
        {
            if(strCommand == "Set Grid")
            {
                SetGridForm.ShowWindow();
                RootApp.UI.UI.ShowGrid(SetGridForm.IsShowGrid,SetGridForm.Accuracy);
            }

            if (strCommand == "Integer Linear Programming")
            {
                ILPForm.ShowWindow();
            }

            if(strCommand == "Matrix Operations")
            {

            }
        }
    }
}

