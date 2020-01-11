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
    public class Entrance : RootApp.UI.IPluginBase
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

            RootApp.UI.RegisterMenu.AddMenuSeparator("DrawUtility");
            RootApp.UI.RegisterTool.AddToolSeparator();

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "Select");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "Select", Path.Combine(RootApp.ConstData.AppIconPath, "select.png"));
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "Select", Path.Combine(RootApp.ConstData.AppIconPath, "select.png"));

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "Move");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "Move", Path.Combine(RootApp.ConstData.AppIconPath, "move.png"));
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "Move", Path.Combine(RootApp.ConstData.AppIconPath, "move.png"));

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "Adapt");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "Adapt", Path.Combine(RootApp.ConstData.AppIconPath, "Adapt.png"));
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "Adapt", Path.Combine(RootApp.ConstData.AppIconPath, "Adapt.png"));

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "View Larger");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "View Larger", Path.Combine(RootApp.ConstData.AppIconPath, "view larger.png"));
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "View Larger", Path.Combine(RootApp.ConstData.AppIconPath, "view larger.png"));

            RootApp.UI.RegisterMenu.AddMenu("DrawUtility", "View Smaller");
            RootApp.UI.RegisterMenu.SetMenuIcon("DrawUtility", "View Smaller", Path.Combine(RootApp.ConstData.AppIconPath, "view smaller.png"));
            RootApp.UI.RegisterTool.AddToolButton("DrawUtility", "View Smaller", Path.Combine(RootApp.ConstData.AppIconPath, "view smaller.png"));

            RootApp.UI.RegisterTool.AddToolSeparator();
        }

        public void RunCommand(string strCommand)
        {
            switch (strCommand)
            {
                case "Place": RootApp.UI.UI.AddPlace();break;
                case "Transition": RootApp.UI.UI.AddTransition(); break;
                case "Arc": RootApp.UI.UI.AddArc(); break;
                case "Select": RootApp.UI.UI.Select(); break;
                case "Move": RootApp.UI.UI.Move(); break;
                case "Adapt": RootApp.UI.UI.Adapt(); break;
                case "View Larger": RootApp.UI.UI.ViewLarger(); break;
                case "View Smaller": RootApp.UI.UI.ViewSmaller(); break;
                default: break;
            }
        }
    }
}
