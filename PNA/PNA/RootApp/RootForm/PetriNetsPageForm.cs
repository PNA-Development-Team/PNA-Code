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
using DrawTool;

namespace RootApp
{
    public partial class PetriNetsPageForm : DockContent
    {
        private IntPtr m_WinHandle = IntPtr.Zero;
        public IntPtr WinHandle
        {
            get { return m_WinHandle; }
        }
        private bool m_isActive = false;
        public bool IsActive
        {
            get { return m_isActive; }
        }

        public PetriNetsPageForm()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.Shown += new EventHandler(PetriNetsPageForm_Shown);
            this.Paint += new PaintEventHandler(PetriNetsPageForm_Paint);
            this.Click += new EventHandler(PetriNetsPageForm_Click);
            
        }

        public void SetActiveMode(bool isActive)
        {
            m_isActive = isActive;
            if(m_isActive)
                DrawTool.Window.LoadWindow(this, DrawTool.Color.BLACK);
            else if(DrawTool.Window.WinHandle == this.m_WinHandle)
            {
                DrawTool.Window.UnLoadWindow();
            }
        }

        private void PetriNetsPageForm_Load(object sender, EventArgs e)
        {
            this.m_WinHandle = this.Handle;
        }

        private void PetriNetsPageForm_Shown(object sender, EventArgs e)
        {
            DrawTool.Window.SetBackgroundColor(DrawTool.Color.BLACK);
        }

        private void PetriNetsPageForm_Paint(object sender, EventArgs e)
        {
            DrawTool.Window.SetBackgroundColor(DrawTool.Color.BLACK);
        }

        private void PetriNetsPageForm_Click(object sender, EventArgs e)
        {
            DrawTool.Window.SetBackgroundColor(DrawTool.Color.BLACK);
        }

        public void ShowWindow(DockPanel dockPanel,DockState dockState)
        {
            this.Show(dockPanel);
            this.DockState = dockState;
        }

        public void AddPlace()
        {
            MessageBox.Show("Added Place at " + this.Text + "!");
        }

        public void AddTransition()
        {
            MessageBox.Show("Added Transition at " + this.Text + "!");
        }

        public void AddArc()
        {
            MessageBox.Show("Added Arc at " + this.Text + "!");
        }
    }
}
