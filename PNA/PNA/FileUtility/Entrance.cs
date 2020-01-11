using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using RootApp;

namespace FileUtility
{
    public class Entrance : RootApp.UI.IPluginBase
    {
        public void Init()
        {
            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "New Page");
            RootApp.UI.RegisterMenu.SetMenuShortcuts("FileUtility", "New Page", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N);
            RootApp.UI.RegisterMenu.SetMenuIcon("FileUtility", "New Page",Path.Combine(ConstData.AppIconPath, "newfile.png"));
            RootApp.UI.RegisterTool.AddToolButton("FileUtility", "New Page", Path.Combine(ConstData.AppIconPath, "newfile.png"));

            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "Open Page");
            RootApp.UI.RegisterMenu.SetMenuShortcuts("FileUtility", "Open Page", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O);
            RootApp.UI.RegisterMenu.SetMenuIcon("FileUtility", "Open Page", Path.Combine(ConstData.AppIconPath, "openfile.png"));
            RootApp.UI.RegisterTool.AddToolButton("FileUtility", "Open Page", Path.Combine(ConstData.AppIconPath, "openfile.png"));

            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "Export", "Page To PNs");
            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "Export","Incidence Matrix To Execl");

            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "Save");
            RootApp.UI.RegisterMenu.SetMenuIcon("FileUtility", "Save", Path.Combine(ConstData.AppIconPath, "save.png"));
            RootApp.UI.RegisterMenu.SetMenuShortcuts("FileUtility", "Save", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S);
            RootApp.UI.RegisterTool.AddToolButton("FileUtility", "Save", Path.Combine(ConstData.AppIconPath, "save.png"));

            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "SaveAs");
            RootApp.UI.RegisterMenu.SetMenuIcon("FileUtility", "SaveAs", Path.Combine(ConstData.AppIconPath, "save-as.png"));

            RootApp.UI.RegisterMenu.AddMenu("FileUtility", "Save All");
            RootApp.UI.RegisterMenu.SetMenuIcon("FileUtility", "Save All", Path.Combine(ConstData.AppIconPath, "saveall.png"));
            RootApp.UI.RegisterMenu.SetMenuShortcuts("FileUtility", "Save All", System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S);
            RootApp.UI.RegisterTool.AddToolButton("FileUtility", "Save All", Path.Combine(ConstData.AppIconPath, "saveall.png"));

            RootApp.UI.RegisterTool.AddToolSeparator();

            RootApp.UI.RegisterMenu.AddMenuSeparator("FileUtility");

            RootApp.UI.RegisterMenu.AddMenu("FileUtility","Exit");
        }

        public void RunCommand(string strCommand)
        {
            if (strCommand == "New Page")
            {
                RootApp.UI.UI.CreateNewPage();
            }
            if (strCommand == "Open Page")
            {
                RootApp.UI.UI.OpenPage();
            }
            if (strCommand == "Save")
            {
                RootApp.UI.UI.SaveCurrentPage();
            }
            if (strCommand == "Exit")
            {
                RootApp.UI.UI.ExitApplication();
            }
            if(strCommand == "Page To PNs")
            {
                MessageBox.Show("Page To PNs!");
            }
        }

    }
}
