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
    public partial class PetriNetsPageForm : DockContent
    {
        public PetriNetsPageForm()
        {
            InitializeComponent();
            this.TopLevel = false;
        }

        private void PetriNetsPageForm_Load(object sender, EventArgs e)
        {

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
