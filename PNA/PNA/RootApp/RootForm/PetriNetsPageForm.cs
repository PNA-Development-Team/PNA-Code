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

        private string m_pageName = "New Page";
        public string PageName
        {
            get { return m_pageName; }
            set
            {
                m_pageName = value;
                this.Text = value;
            }
        }

        private DrawTool.Color m_backgroundColor = DrawTool.Color.WHITE;
        public DrawTool.Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set
            {
                m_backgroundColor = value;
                this.Update();
            }
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
            set { m_accuracy = value; }
        }

        public PetriNetsPageForm()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.Shown += new EventHandler(PetriNetsPageForm_Shown);
            this.Paint += new PaintEventHandler(PetriNetsPageForm_Paint);
            this.Click += new EventHandler(PetriNetsPageForm_Click);
            this.SizeChanged += new EventHandler(PetriNetsPageForm_SizeChanged);
        }

        public void SetActiveMode(bool isActive)
        {
            m_isActive = isActive;
            if(m_isActive)
                DrawTool.Window.LoadWindow(this);
            else if(DrawTool.Window.IsLoadedWindow() && DrawTool.Window.WinHandle == this.m_WinHandle)
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
            this.Update();
        }

        private void PetriNetsPageForm_Paint(object sender, EventArgs e)
        {
            this.Update();
        }

        private void PetriNetsPageForm_Click(object sender, EventArgs e)
        {
            this.Update();
        }

        private void PetriNetsPageForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                DrawTool.Window.ReLoadWindow();
                this.Update();
            }
        }

        public void ShowWindow(DockPanel dockPanel,DockState dockState)
        {
            this.Show(dockPanel);
            this.DockState = dockState;
        }

        public new void Update()
        {
            if (!DrawTool.Window.IsLoadedWindow())
                SetActiveMode(true);
            DrawTool.Window.SetBackgroundColor(this.m_backgroundColor);
            if (m_isShowGrid)
                DrawTool.Window.ShowGrid(Accuracy);
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

        public void ViewLarger()
        {
            DrawTool.Window.ChangeViewPoint(2);
            this.Update();
        }

        public int smaller = 1;

        public void ViewSmaller()
        {
            if(smaller > 0)
                smaller += 1;
            DrawTool.Window.ChangeViewPoint(smaller);
            this.Update();
        }
    }
}
