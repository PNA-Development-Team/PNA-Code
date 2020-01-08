using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RootApp;

namespace DrawUtility
{
    public class DrawUtility : RootApp.UI.IPluginBase
    {
        public void Init()
        {
            RootApp.UI.RegisterMenu.AddMenu("DrawUtility","Place");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "Place",Path.Combine(RootApp.ConstData.AppIconPath,"Place.png"));
            RootApp.UI.RegisterMenu.SetMenuShortcuts("DrawUtility", "Place", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P);
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "Place", Path.Combine(RootApp.ConstData.AppIconPath, "Place.png"));

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "Transition");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "Transition", Path.Combine(RootApp.ConstData.AppIconPath, "Transition.png"));
            RootApp.UI.RegisterMenu.SetMenuShortcuts("DrawUtility", "Transition", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T);
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "Transition", Path.Combine(RootApp.ConstData.AppIconPath, "Transition.png"));

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "Arc");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "Arc", Path.Combine(RootApp.ConstData.AppIconPath, "Arc.png"));
            RootApp.UI.RegisterMenu.SetMenuShortcuts("DrawUtility", "Arc", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A);
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "Arc", Path.Combine(RootApp.ConstData.AppIconPath, "Arc.png"));

            RootApp.UI.RegisterTool.AddToolSeparator();
        }

        public void RunCommand(string strCommand)
        {
            if(strCommand == "Place")
            {
                RootApp.UI.UI.AddPlace();
            }
            if (strCommand == "Transition")
            {
                RootApp.UI.UI.AddTransition();
            }
            if(strCommand == "Arc")
            {
                RootApp.UI.UI.AddArc();
            }
        }
    }
}
