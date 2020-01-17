using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using DrawTool;

namespace RootApp
{
    public class PetriNetsPageInfo
    {
        private string m_pageName = string.Empty;
        
        public string PageName
        {
            get { return m_pageName; }
            set { m_pageName = value; }
        }

        private double m_viewSize = 0.0;
        public double ViewSize
        {
            get { return m_viewSize; }
            set
            {
                m_viewSize = value;
            }
        }

        private Point2D m_viewPosition = new Point2D();
        public Point2D ViewPosition
        {
            get { return m_viewPosition; }
            set
            {
                m_viewPosition = value;
            }
        }

        public PetriNetsPageInfo()
        {
            
        }

        public PetriNetsPageInfo(string xmlData)
        {
            XmlDocument doc = new XmlDocument();
            doc.InnerXml = xmlData;

            XmlElement rootElement = doc.SelectSingleNode(@"\PetriNetsPageInfo") as XmlElement;

            XmlElement pageElement = rootElement.SelectSingleNode(@"\Page") as XmlElement;
            this.m_pageName = pageElement.GetAttribute("name");
            XmlElement viewSizeElement = rootElement.SelectSingleNode(@"\ViewSize") as XmlElement;
            this.m_viewSize = Convert.ToDouble(viewSizeElement.GetAttribute("value"));
            XmlElement viewPositionElement = rootElement.SelectSingleNode(@"\ViewPosition") as XmlElement;
            this.m_viewPosition = new Point2D(viewPositionElement.GetAttribute("value"));
        }
        public string ToXmlData()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement rootElement = doc.CreateElement("PetriNetsPageInfo");

            XmlElement pageElement = doc.CreateElement("Page");
            pageElement.SetAttribute("name", this.m_pageName);
            XmlElement viewSizeElement = doc.CreateElement("ViewSize");
            viewSizeElement.SetAttribute("value", this.m_viewSize.ToString());
            XmlElement viewPositionElement = doc.CreateElement("ViewPosition");
            viewPositionElement.SetAttribute("value", this.m_viewPosition.ToString());

            rootElement.AppendChild(pageElement);
            rootElement.AppendChild(viewSizeElement);
            rootElement.AppendChild(viewPositionElement);

            doc.AppendChild(rootElement);

            return doc.InnerXml;
        }
    }

    public partial class PetriNetsPageForm : DockContent
    {
        private static DrawTool.Color m_backgroundColor = DrawTool.Color.WHITE;
        public static DrawTool.Color BackgroundColor
        {
            get { return m_backgroundColor; }
            set
            {
                m_backgroundColor = value;
                PNAMainForm.Instance.CurrentOpenPNsPage.Update();
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
    }

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

        private PetriNetsPageInfo m_pageInfo = new PetriNetsPageInfo();
        public PetriNetsPageInfo PageInfo
        {
            get { return m_pageInfo; }
        }

        public PetriNetsPageForm()
        {
            InitializeComponent();
            this.TopLevel = false;
            this.Shown += new EventHandler(PetriNetsPageForm_Shown);
            this.Paint += new PaintEventHandler(PetriNetsPageForm_Paint);
            this.Click += new EventHandler(PetriNetsPageForm_Click);
            this.SizeChanged += new EventHandler(PetriNetsPageForm_SizeChanged);
            this.MouseWheel += new MouseEventHandler(PetriNetsPageForm_MouseWheelMove);
            ModifiedPageName("New Page");
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

        public void ModifiedPageName(string pageName)
        {
            this.m_pageInfo.PageName = pageName;
            this.Text = pageName;
        }
        public new void Update()
        {
            if (!DrawTool.Window.IsLoadedWindow())
                SetActiveMode(true);
            DrawTool.Window.SetViewDistance(this.m_pageInfo.ViewSize);
            DrawTool.Window.SetBackgroundColor(m_backgroundColor);
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

        private void PetriNetsPageForm_MouseWheelMove(object sender,MouseEventArgs e)
        {
            if(e.Delta > 0)
            {
                this.m_pageInfo.ViewSize++;             
                this.Update();
            }
            else if(e.Delta < 0)
            {
                this.m_pageInfo.ViewSize--;
                this.Update();
            }
            else if(e.Button == System.Windows.Forms.MouseButtons.Middle)
            {

            }
        }

        public void Adapt()
        {
            this.m_pageInfo.ViewSize = 0;
            this.Update();
        }
    }
}
