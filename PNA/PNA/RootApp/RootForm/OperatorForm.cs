using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace RootApp
{
    public partial class OperatorForm : DockContent
    {
        private static OperatorForm m_Instance = null;
        public static OperatorForm Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new OperatorForm();
                return m_Instance;
            }
        }

        private static DockState m_dockState;

        public static void ShowWindow(DockPanel dockPanel, DockState dockState)
        {
            Instance.Show(dockPanel);
            Instance.DockState = dockState;
        }

        public OperatorForm()
        {
            InitializeComponent();
        }

        private void FrmFunction_DockStateChanged(object sender, EventArgs e)
        {
            //关闭时（dockstate为unknown） 不把dockstate保存  
            if (m_Instance != null)
            {
                if (this.DockState == DockState.Unknown || this.DockState == DockState.Hidden)
                {
                    return;
                }

                m_dockState = this.DockState;
            }
        }

    }
}
