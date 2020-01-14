using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Utility;
using DrawTool;

namespace RootApp
{
    public partial class PNAMainForm : Form
    {
        private static PNAMainForm m_Instance = null;

        public static PNAMainForm Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new PNAMainForm();
                }
                return m_Instance;
            }
        }

        public static void ShowWindow()
        {
            Instance.Show();
        }

        private Dictionary<string, ToolStripMenuItem> m_menuDictionary = new Dictionary<string, ToolStripMenuItem>();
        
        public Dictionary<string, ToolStripMenuItem> MenuDictionary
        {
            get { return m_menuDictionary; }
        }

        private Dictionary<string, PetriNetsPageForm> m_petriNetsPageDictionary = new Dictionary<string, PetriNetsPageForm>();
        public Dictionary<string, PetriNetsPageForm> PetriNetsPageDictionary
        {
            get { return m_petriNetsPageDictionary; }
        }

        private Dictionary<string,System.Windows.Forms.Keys> m_menuFullNameMapKeys = new Dictionary<string, System.Windows.Forms.Keys>();
        public Dictionary<string, System.Windows.Forms.Keys> MenuFullNameMapKeys
        {
            get { return m_menuFullNameMapKeys; }
        }

        private Dictionary<string, ToolStripButton> m_toolButtonDictionary = new Dictionary<string, ToolStripButton>();
        public Dictionary<string,ToolStripButton> ToolButtonDictionary
        {
            get { return m_toolButtonDictionary; }
        }

        private PetriNetsPageForm m_currentOpenPNsPage = null;
        public PetriNetsPageForm CurrentOpenPNsPage
        {
            get
            {
                if(m_currentOpenPNsPage == null)
                {
                    MessageBox.Show("Please create or open a page first!");
                    return null;
                }
                return m_currentOpenPNsPage;
            }
        }

        private int m_newPageCount = 0;

        public PNAMainForm()
        {
            Utility.DebugHelper.Log("Appliction Started.");
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.SizeChanged += new EventHandler(PNAMainForm_SizeChanged);
            string iconImagePath = Path.Combine(ConstData.AppIconPath, "PNA.ico");
            if (File.Exists(iconImagePath))
                this.Icon = new Icon(iconImagePath);
            
            OperatorForm.ShowWindow(this.operatorDockPanel, DockState.DockLeft);

            this.operatorDockPanel.ActiveDocumentChanged += new EventHandler(OperatorDockPanelActiveDocumentChanged);
            
        }

        private void PNAMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                RootApp.UI.RegisterMenu.AddMainMenu();
                RootApp.UI.RegisterMenu.AddConfigDll();
                SetDebugText("Application Address：" + ConstData.AppPath);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("An execption occurred during program initialization :", ex.Message, MessageBoxButtons.OK) == DialogResult.OK)
                {
                    this.Close();
                }
            }
        }

        private void PNAMainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.m_currentOpenPNsPage != null)
            {

            }    
        }

        public void SetDebugText(string debugText)
        {
            this.debugTextBox.Text = debugText;
        }

        public void AddMenu(string parentMenuFullName,string childMenuName)
        {
            if (!string.IsNullOrEmpty(parentMenuFullName) && !this.m_menuDictionary.ContainsKey(parentMenuFullName))
                throw new NotImplementedException("Can not find menu named : " + parentMenuFullName + "!");

            if(string.IsNullOrEmpty(childMenuName))
                throw new NotImplementedException("The menu named should not to be empty !");

            ToolStripMenuItem childMenu = new ToolStripMenuItem();
            childMenu.Text = childMenuName;
            childMenu.Click += new EventHandler(this.MenuItem_Click);
            
            if (string.IsNullOrEmpty(parentMenuFullName))
            {
                childMenu.Name = childMenuName + "_MenuButton";
                this.menuRoot.Items.AddRange(new ToolStripItem[] { childMenu});
                this.m_menuDictionary.Add(childMenuName, childMenu);
            }
            else
            {
                string childMenuFullName = parentMenuFullName + "_" + childMenuName;
                childMenu.Name = childMenuFullName + "_MenuButton";
                ToolStripMenuItem parentMenu = this.m_menuDictionary[parentMenuFullName];
                parentMenu.DropDownItems.Add(childMenu);
                this.m_menuDictionary.Add(childMenuFullName, childMenu);
            }         
        }

        public void AddMenuSeparator(string parentMenuFullName)
        {
            if (string.IsNullOrEmpty(parentMenuFullName))
                throw new NotImplementedException("The parent menu name of menu separator can not be empty !");

            if (!this.m_menuDictionary.ContainsKey(parentMenuFullName))
                throw new NotImplementedException("Can not find the parent menu name of menu separator !");

            ToolStripSeparator separator = new ToolStripSeparator();

            ToolStripMenuItem parentMenu = this.m_menuDictionary[parentMenuFullName];
            parentMenu.DropDownItems.Add(separator);
        }

        public void SetMenuIcon(string menuFullName,string iconFilePath)
        {
            if (!m_menuDictionary.ContainsKey(menuFullName))
                throw new NotImplementedException(@"Can not find the menu name :" + menuFullName + @" when set menu icon !");

            if(!File.Exists(iconFilePath))
                throw new NotImplementedException("");

            ToolStripMenuItem menu = m_menuDictionary[menuFullName];
            Image icon = Image.FromFile(iconFilePath);
            menu.Image = icon;
        }

        public void SetMenuShortcuts(string menuFullName, System.Windows.Forms.Keys keyData)
        {
            if (!m_menuDictionary.ContainsKey(menuFullName))
                throw new NotImplementedException(@"Can not find the menu name :" + menuFullName + @" when add a shortcuts !");

            if (m_menuFullNameMapKeys.ContainsValue(keyData))
                throw new NotImplementedException(@"The shortcuts named :" + keyData.ToString() + " has been used !");

            ToolStripMenuItem menu = m_menuDictionary[menuFullName];
            menu.ShortcutKeys = keyData;
            menu.ShowShortcutKeys = true;
            m_menuFullNameMapKeys.Add(menuFullName, keyData);
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType() != typeof(ToolStripMenuItem))
                    return;
                string activeMenuFullName = GetMenuFullName(sender as ToolStripMenuItem);
                RootApp.UI.RegisterMenu.OnMenuCommand(activeMenuFullName);
            }
            catch(Exception ex)
            {
                return;
            }
        }

        private string GetMenuFullName(ToolStripMenuItem menu)
        {
            if (menu == null)
                return String.Empty;

            string menuFullName = menu.ToString();
            ToolStripItem parentMenu = menu.OwnerItem;
            while (parentMenu != null)
            {
                menuFullName = parentMenu.ToString() + "_" + menuFullName;
                parentMenu = parentMenu.OwnerItem;
            }
            return menuFullName;
        }

        public void AddToolButton(string toolButtonFullName,string iconFilePath)
        {
            if (string.IsNullOrEmpty(toolButtonFullName))
                throw new NotImplementedException("Tool button name can not to be empty!");
            if (string.IsNullOrEmpty(iconFilePath))
                throw new NotImplementedException("Tool button icon file address can not to be empty!");
            if(!File.Exists(iconFilePath))
                throw new NotImplementedException("Tool button icon file address is error！Error address：" + iconFilePath + ".");

            ToolStripButton toolButton = new ToolStripButton();
            toolButton.Name = toolButtonFullName + "_ToolButton";
            Image iconImage = Image.FromFile(iconFilePath);
            toolButton.Image = iconImage;
            toolButton.Click += new EventHandler(ToolButton_Click);
            string toolButtonTipText = toolButtonFullName.Split('_').Last();             
            toolButton.ToolTipText = toolButtonTipText;

            this.toolStripRoot.Items.Add(toolButton);
            this.m_toolButtonDictionary.Add(toolButtonFullName,toolButton);
        }

        public void AddToolSeparator()
        {
            ToolStripSeparator separator = new ToolStripSeparator();
            this.toolStripRoot.Items.Add(separator);
        }

        private void ToolButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType() != typeof(ToolStripButton))
                    return;
                ToolStripButton toolButton = sender as ToolStripButton;
                string toolButtonFullName = toolButton.Name.ToString().Replace("_ToolButton","");
                RootApp.UI.RegisterTool.OnToolCommand(toolButtonFullName);
            }
            catch(Exception ex)
            {
                return;
            }
        }

        public void CreateNewPage()
        {
            PetriNetsPageForm newPage = new PetriNetsPageForm();
            newPage.Parent = this;
            if(this.m_newPageCount != 0)
            {
                newPage.ModifiedPageName("New Page" + this.m_newPageCount.ToString());
            }

            newPage.Disposed += new EventHandler(CurrentOpenPnsPage_Dispose);

            if(this.m_currentOpenPNsPage != null)
                this.m_currentOpenPNsPage.SetActiveMode(false);
            this.m_currentOpenPNsPage = newPage;
            this.m_currentOpenPNsPage.ShowWindow(this.operatorDockPanel,DockState.Document);

            this.m_petriNetsPageDictionary.Add(this.m_currentOpenPNsPage.Text, this.m_currentOpenPNsPage);
            this.m_newPageCount++;

            RootApp.UI.UI.ShowDebugText("Create new page named " + this.m_currentOpenPNsPage.Text + ".");
        }

        public void OpenPage()
        {
            throw new NotImplementedException();
        }

        public void SaveCurrentPage()
        {
            throw new NotImplementedException();
        }

        private void OperatorDockPanelActiveDocumentChanged(object sender, EventArgs e)
        {
            if(this.operatorDockPanel.ActiveDocument != null)
            {
                this.m_currentOpenPNsPage.SetActiveMode(false);
                this.m_currentOpenPNsPage = this.operatorDockPanel.ActiveDocument as PetriNetsPageForm;
                this.m_currentOpenPNsPage.SetActiveMode(true);
                RootApp.UI.UI.ShowDebugText("Open " + this.m_currentOpenPNsPage.Text + ".");
            }
            else
            {
                this.m_currentOpenPNsPage.SetActiveMode(false);
                this.m_currentOpenPNsPage = null;
                RootApp.UI.UI.ShowDebugText("All page has been closed !");
            }
        }

        private void CurrentOpenPnsPage_Dispose(object sender,EventArgs e)
        {
            if(this.m_currentOpenPNsPage != null && 
               this.m_petriNetsPageDictionary.ContainsKey(this.m_currentOpenPNsPage.PageInfo.PageName))
                this.m_petriNetsPageDictionary.Remove(this.m_currentOpenPNsPage.PageInfo.PageName);
        }

    }
}
