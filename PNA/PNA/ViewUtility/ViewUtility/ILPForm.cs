using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewUtility
{
    public partial class ILPForm : Form
    {
        private static ILPForm m_Instance = null;
        public static ILPForm Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new ILPForm();
                return m_Instance;
            }
        }

        public static void ShowWindow()
        {
            Instance.Show();
        }

        private string m_filePath = string.Empty;

        public ILPForm()
        {
            InitializeComponent();
            this.openFileDialog.Filter = "LP File(*.lp;*.lg4)|*.lp;*.lg4|All File(*.*)|*.*";
            this.saveFileDialog.Filter = "LP File(*.lp;*.lg4)|*.lp;*.lg4";
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.m_filePath) || !string.IsNullOrEmpty(this.rtbEdit.Text))
            {
                if (MessageBox.Show("Quit current text?", "Notice", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    this.m_filePath = string.Empty;
                    this.rtbEdit.Text = string.Empty;
                }                 
            }
            if(string.IsNullOrEmpty(this.m_filePath) && string.IsNullOrEmpty(this.rtbEdit.Text) && this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.m_filePath = this.openFileDialog.FileName;
                this.Import();
            }
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(this.rtbEdit.Text))
            {
                MessageBox.Show("Can not save file beacuse current text is empty!", "Notice");
                return;
            }

            if(string.IsNullOrEmpty(this.m_filePath))
                this.saveFileDialog.FileName = this.m_filePath;
            if (this.saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            this.Export(this.saveFileDialog.FileName);
        }

        private void btCaculate_Click(object sender, EventArgs e)
        {

        }

        private void Import()
        {
            if (string.IsNullOrEmpty(m_filePath))
                return;


        }

        private void Export(string filePath)
        {

        }
    }
}
