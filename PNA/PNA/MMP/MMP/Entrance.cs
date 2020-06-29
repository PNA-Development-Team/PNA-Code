using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RootApp.UI;
using System.Windows.Forms;
using MathematicalTool.PetriNetOperator;
using MathematicalTool.PetriNetTTuple;
using System.IO;

namespace MMP
{
    public class Entrance :RootApp.UI.IPluginBase
    {
        public void Init()
        {
            RootApp.UI.RegisterMenu.AddMenu("MMP", "MMP");
            RootApp.UI.RegisterMenu.AddMenu("MMP", "NMMP");
        }

        public void RunCommand(string strCommand)
        {
            if(strCommand == "MMP")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "pnt files (*.pnt)|*.pnt| All files (*.*)|*.*";
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Marking M0 = PetriNetOperator.GetInitialMarkingFromPntFile(dialog.FileName);
                    Reachability gra = new Reachability(M0);
                    MMPCaculate mmp = new MMPCaculate(gra);
                    mmp.ExportStateFile();
                    RootApp.UI.UI.ShowDebugText("Success export state file.");
                    mmp.ExportMMPILPFile(true);
                    RootApp.UI.UI.ShowDebugText("Success export MMP ILP files.");
                }   
            }
            else if(strCommand == "NMMP")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "pnt files (*.pnt)|*.pnt| All files (*.*)|*.*";
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Marking M0 = PetriNetOperator.GetInitialMarkingFromPntFile(dialog.FileName);
                    Reachability gra = new Reachability(M0);
                    NMMPCaculate nmmp = new NMMPCaculate(gra);
                    nmmp.ExportStateFile();
                    RootApp.UI.UI.ShowDebugText("Success export state file.");
                    nmmp.ExportNMMPILPFile();
                    RootApp.UI.UI.ShowDebugText("Success export NMMP ILP files.");
                    nmmp.ExportXls();
                    RootApp.UI.UI.ShowDebugText("Success export excel file.");
                }
            }  
        }
    }
}
