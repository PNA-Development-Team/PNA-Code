using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;
using RootApp;

namespace ViewUtility
{
    public partial class SetGridForm : Form
    {
        private static SetGridForm m_Instance = null;
        public static SetGridForm Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = new SetGridForm();
                }
                return m_Instance;
            }
        }

        public static void ShowWindow()
        {
            Instance.ShowDialog();       
        }

        private static bool m_isShowGrid = true;
        public static bool IsShowGrid
        {
            get { return m_isShowGrid; }
            set { m_isShowGrid = value; }
        }

        private static int m_accuracy = 1;
        public static int Accuracy
        {
            get { return m_accuracy; }
            set
            {
                m_accuracy = value;
                if (m_accuracy < 1)
                    m_accuracy = 1;
                if (m_accuracy > 20)
                    m_accuracy = 20;
            }
        }
        public SetGridForm()
        {
            InitializeComponent();
            this.TopMost = true;
            Utility.FormHelper.SetFormStartPositionAtCentral(this, RootApp.PNAMainForm.Instance);
            this.cbIsShow.Checked = IsShowGrid;
            this.nUPAccuracy.Enabled = IsShowGrid;
            this.nUPAccuracy.Value = Accuracy;
        }

        private void cbIsShow_CheckedChanged(object sender, EventArgs e)
        {
            if(this.cbIsShow.Checked == true)
            {
                IsShowGrid = true;
                this.nUPAccuracy.Enabled = true;
            }
            else
            {
                IsShowGrid = false;
                this.nUPAccuracy.Enabled = false;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Accuracy = (int)this.nUPAccuracy.Value;
            this.Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
