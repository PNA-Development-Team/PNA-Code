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
            if(PNAMainForm.Instance.CurrentOpenPNsPage != null)
                PNAMainForm.Instance.CurrentOpenPNsPage.AddPlace();
        }

        public static void AddTransition()
        {
            if (PNAMainForm.Instance.CurrentOpenPNsPage != null)
                PNAMainForm.Instance.CurrentOpenPNsPage.AddTransition();
        }

        public static void AddArc()
        {
            if (PNAMainForm.Instance.CurrentOpenPNsPage != null)
                PNAMainForm.Instance.CurrentOpenPNsPage.AddArc();
        }

        public static void ShowGrid(bool isShow,int accuracy)
        {
            if(PetriNetsPageForm.IsShowGrid != isShow || PetriNetsPageForm.Accuracy != accuracy)
            {
                PetriNetsPageForm.IsShowGrid = isShow;
                PetriNetsPageForm.Accuracy = accuracy;
                if(PNAMainForm.Instance.CurrentOpenPNsPage != null)
                    PNAMainForm.Instance.CurrentOpenPNsPage.Update();
            }  
        }

        public static void Select()
        {

        }

        public static void Move()
        {

        }

        public static void Adapt()
        {

        }

        public static void ViewLarger()
        {
            PNAMainForm.Instance.CurrentOpenPNsPage.ViewLarger();
        }

        public static void ViewSmaller()
        {
            PNAMainForm.Instance.CurrentOpenPNsPage.ViewSmaller();
        }
    }
}
