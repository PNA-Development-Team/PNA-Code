using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace RootApp.UI
{
    public partial class UI
    {
        public static PetriNetsPageForm CurrentOpenPNsPage
        {
            get { return PNAMainForm.Instance.CurrentOpenPNsPage; }
        }

        public static void ShowDebugText(string debugText)
        {
            PNAMainForm.Instance.SetDebugText(debugText);
        }

        public static void CreateNewPage()
        {
            PNAMainForm.Instance.CreateNewPage();
        }

        public static void OpenPage()
        {
            PNAMainForm.Instance.OpenPage();
        }

        public static void SaveCurrentPage()
        {
            PNAMainForm.Instance.SaveCurrentPage();    
        }

        public static void ExitApplication()
        {
            PNAMainForm.Instance.Close();
        }

        public static void AddPlace()
        {
            if(PNAMainForm.Instance.CurrentOpenPNsPage == null)
            {
                MessageBox.Show("插入库所失败，请先创建或导入一张图纸！");
                return;
            }
            PNAMainForm.Instance.CurrentOpenPNsPage.AddPlace();
        }

        public static void AddTransition()
        {
            if (PNAMainForm.Instance.CurrentOpenPNsPage == null)
            {
                MessageBox.Show("插入变迁失败，请先创建或导入一张图纸！");
                return;
            }
            PNAMainForm.Instance.CurrentOpenPNsPage.AddTransition();
        }

        public static void AddArc()
        {
            if (PNAMainForm.Instance.CurrentOpenPNsPage == null)
            {
                MessageBox.Show("插入弧失败，请先创建或导入一张图纸！");
                return;
            }
            PNAMainForm.Instance.CurrentOpenPNsPage.AddArc();
        }
    }
}
