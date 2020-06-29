using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Reflection;
using Utility;

namespace RootApp.UI
{
    public class RegisterMenu
    {
        private static Dictionary<string, string> m_dllNameMapParentMenuName = new Dictionary<string, string>();

        public static Dictionary<string,string> DllNameMapParentMenuName
        {
            get { return m_dllNameMapParentMenuName; }
        }

        private static Dictionary<string, string> m_childMenuFullNameMapDllName = new Dictionary<string, string>();

        private static Dictionary<string, string> m_dllNameMapDllPath = new Dictionary<string, string>();
        public static Dictionary<string, string> DllNameMapDllPath
        {
            get { return m_dllNameMapDllPath; }
        }

        public static void AddMainMenu()
        {
            if (!Directory.Exists(ConstData.AppRootMenuPath))
                throw new NotImplementedException("找不到主菜单文件夹! 路径为：" + ConstData.AppRootMenuPath + ".");

            string configFilePath = Path.Combine(ConstData.AppRootMenuPath, "Config.xml");
            if (!File.Exists(configFilePath))
                throw new NotImplementedException("找不到主菜单配置文件！路径为：" + configFilePath + ".");

            XmlDocument configFile = new XmlDocument();
            configFile.Load(configFilePath);

            XmlNode rootNode = configFile.SelectSingleNode("Config");
            XmlNodeList subMenuNodeList = rootNode.ChildNodes;
            foreach (XmlNode subMenuNode in subMenuNodeList)
            {
                XmlAttributeCollection subMenuAttributes = subMenuNode.Attributes;
                foreach (XmlAttribute subMenuAttribute in subMenuAttributes)
                {
                    if (subMenuAttribute.Name == "name")
                    {
                        PNAMainForm.Instance.AddMenu(null, subMenuAttribute.Value);
                    }
                }
            }
        }

        public static void AddConfigDll()
        {
            if (!Directory.Exists(ConstData.AppRootMenuPath))
                throw new NotImplementedException("找不到主菜单文件夹! 路径为：" + ConstData.AppRootMenuPath + ".");

            string configFilePath = Path.Combine(ConstData.AppRootMenuPath, "Config.xml");
            if (!File.Exists(configFilePath))
                throw new NotImplementedException("找不到主菜单配置文件！路径为：" + configFilePath + ".");

            XmlDocument configFile = new XmlDocument();
            configFile.Load(configFilePath);

            XmlNode rootNode = configFile.SelectSingleNode("Config");
            XmlNodeList subMenuNodeList = rootNode.ChildNodes;
            foreach (XmlNode subMenuNode in subMenuNodeList)
            {
                XmlAttributeCollection subMenuAttributes = subMenuNode.Attributes;
                foreach (XmlAttribute subMenuAttribute in subMenuAttributes)
                {
                    if (subMenuAttribute.Name == "name")
                    {
                        string subMenuFolderFullName = Path.Combine(ConstData.AppRootMenuPath,subMenuAttribute.Value);
                        if (!Directory.Exists(subMenuFolderFullName))
                        {
                            Utility.DebugHelper.Log("Can not find the menu folder named " + subMenuAttribute.Value + " at " + ConstData.AppRootMenuPath + ".");
                            continue;
                        }

                        AddConfigDll(subMenuAttribute.Value, subMenuFolderFullName);
                    }
                }
            }     
        }

        public static void AddConfigDll(string menuName,string menuPath)
        {
            if (!Directory.Exists(menuPath))
                throw new NotImplementedException("找不到菜单文件夹! 路径为：" + menuPath + ".");

            string configFilePath = Path.Combine(menuPath, "Config.Xml");
            LoadConfigFile(menuName,configFilePath);

            DirectoryInfo menuFolderInfo = new DirectoryInfo(menuPath);
            
            foreach (DirectoryInfo subMenuFolder in menuFolderInfo.GetDirectories())
            {
                AddConfigDll(menuName, subMenuFolder.FullName);
            }
        }

        private static void LoadConfigFile(string menuName,string configFilePath)
        {
            if (!File.Exists(configFilePath))
                return;

            FileInfo configFileInfo = new FileInfo(configFilePath);

            XmlDocument configFile = new XmlDocument();
            configFile.Load(configFilePath);

            XmlNode configNode = configFile.SelectSingleNode("Config");
            XmlAttributeCollection configAttributes = configNode.Attributes;
            Dictionary<string, string> attributeValues = new Dictionary<string, string>();
            foreach (XmlAttribute configAttribute in configAttributes)
            {
                attributeValues.Add(configAttribute.Name, configAttribute.Value);
            }

            if (attributeValues.ContainsKey("Enable") &&
               attributeValues["Enable"].ToLower() == "true" &&
               attributeValues.ContainsKey("EntranceModule") &&
               File.Exists(Path.Combine(configFileInfo.DirectoryName, attributeValues["EntranceModule"])) &&
               attributeValues.ContainsKey("EntranceClass") &&
               !string.IsNullOrEmpty(attributeValues["EntranceClass"]))
            {
                string dllPath = Path.Combine(configFileInfo.DirectoryName, attributeValues["EntranceModule"]);
                Assembly assem = Assembly.LoadFile(dllPath);

                Type curClass = assem.GetType(attributeValues["EntranceClass"]);

                ConstructorInfo curClassConstructor = curClass.GetConstructor(Type.EmptyTypes);//获取不带参的构造函数
                object curClassObject = curClassConstructor.Invoke(new object[] { });
                MethodInfo initMethod = curClass.GetMethod("Init");

                FileInfo dllFileInfo = new FileInfo(dllPath);
                string dllName = dllFileInfo.Name.Replace(".dll", "");
                m_dllNameMapParentMenuName.Add(dllName, menuName);
                m_dllNameMapDllPath.Add(dllName, dllPath);

                initMethod.Invoke(curClassObject, null);
            }
        }

        public static void AddMenu(string dllName, string menuName)
        {
            if (!m_dllNameMapParentMenuName.ContainsKey(dllName))
                throw new NotImplementedException("m_dllNameMapParentMenuName中未能找到" + dllName + ".");

            if (string.IsNullOrEmpty(menuName))
                throw new NotImplementedException("RegisterCustomMenu中的menuName不能为空！");

            PNAMainForm.Instance.AddMenu(m_dllNameMapParentMenuName[dllName], menuName);

            string childMenuFullName = m_dllNameMapParentMenuName[dllName] + "_" + menuName;

            m_childMenuFullNameMapDllName.Add(childMenuFullName, dllName);
        }

        public static void AddMenu(string dllName, string parentMenuName, string childMenuName)
        {
            AddMenu(dllName, new List<string> { parentMenuName }, childMenuName);
        }

        public static void AddMenu(string dllName, List<string> parentMenuNameList, string childMenuName)
        {
            if (!m_dllNameMapParentMenuName.ContainsKey(dllName))
                throw new NotImplementedException("m_dllNameMapParentMenuName中未能找到" + dllName + ".");

            if (string.IsNullOrEmpty(childMenuName))
                throw new NotImplementedException("RegisterCustomMenu中的childMenuName不能为空！");

            if (parentMenuNameList == null || parentMenuNameList.Count == 0)
            {
                AddMenu(dllName, childMenuName);
                return;
            }

            string parentMenuFullName = m_dllNameMapParentMenuName[dllName];
            foreach (string parentMenuName in parentMenuNameList)
            {
                parentMenuFullName += "_" + parentMenuName;
                if (!m_childMenuFullNameMapDllName.ContainsKey(parentMenuFullName))
                {
                    AddMenu(dllName, parentMenuName);
                }
            }       

            PNAMainForm.Instance.AddMenu(parentMenuFullName, childMenuName);
            string childMenuFullName = parentMenuFullName + "_" + childMenuName;
            m_childMenuFullNameMapDllName.Add(childMenuFullName, dllName);
        }

        public static void SetMenuIcon(string dllName, string menuName, string iconFileFullPath)
        {
            SetMenuIcon(dllName, new List<string> (), menuName, iconFileFullPath);
        }

        public static void SetMenuIcon(string dllName, string parentMenuName, string childMenuName, string iconFileFullPath)
        {
            SetMenuIcon(dllName, new List<string> { parentMenuName }, childMenuName, iconFileFullPath);
        }

        public static void SetMenuIcon(string dllName, List<string> parentMenuNameList, string childMenuName, string iconFileFullPath)
        {
            string childMenuFullName = GetMenuFullName(dllName, parentMenuNameList, childMenuName);
            PNAMainForm.Instance.SetMenuIcon(childMenuFullName, iconFileFullPath);
        }

        public static void SetMenuShortcuts(string dllName, string menuName, System.Windows.Forms.Keys keyData)
        {
            SetMenuShortcuts(dllName, new List<string> (), menuName, keyData);
        }

        public static void SetMenuShortcuts(string dllName, string parentMenuName, string childMenuName, System.Windows.Forms.Keys keyData)
        {
            SetMenuShortcuts(dllName, new List<string> { parentMenuName }, childMenuName, keyData);
        }

        public static void SetMenuShortcuts(string dllName, List<string> parentMenuNameList, string childMenuName, System.Windows.Forms.Keys keyData)
        {
            string childMenuFullName = GetMenuFullName(dllName, parentMenuNameList, childMenuName);
            PNAMainForm.Instance.SetMenuShortcuts(childMenuFullName, keyData);
        }

        public static void AddMenuSeparator(string dllName)
        {
            AddMenuSeparator(dllName, new List<string> { });
        }
        public static void AddMenuSeparator(string dllName, string parentMenuName)
        {
            AddMenuSeparator(dllName, new List<string> { parentMenuName });
        }

        public static void AddMenuSeparator(string dllName, List<string> parentMenuNameList)
        {
            string parentMenuFullName = GetParentMenuFullName(dllName, parentMenuNameList);
            PNAMainForm.Instance.AddMenuSeparator(parentMenuFullName);
        }

        public static void OnMenuCommand(string activeMenuFullName)
        {       
            if (!m_childMenuFullNameMapDllName.ContainsKey(activeMenuFullName))
                return;

            string dllName = m_childMenuFullNameMapDllName[activeMenuFullName];
            if (!m_dllNameMapParentMenuName.ContainsKey(dllName))
                throw new NotImplementedException("在m_dllNameMapParentMenuName中未能找到dll" + dllName + ".");
            string dllPath = m_dllNameMapDllPath[dllName];
            FileInfo dllFileInfo = new FileInfo(dllPath);

            string configFilePath = Path.Combine(dllFileInfo.DirectoryName, "Config.Xml");
            if (!File.Exists(configFilePath))
                throw new NotImplementedException("未能找到" + configFilePath + "配置文件.");

            XmlDocument configFile = new XmlDocument();
            configFile.Load(configFilePath);

            XmlNode configNode = configFile.SelectSingleNode("Config");
            XmlAttributeCollection configAttributes = configNode.Attributes;
            Dictionary<string, string> attributeValues = new Dictionary<string, string>();
            foreach (XmlAttribute configAttribute in configAttributes)
            {
                attributeValues.Add(configAttribute.Name, configAttribute.Value);
            }

            if (attributeValues.ContainsKey("Enable") &&
                attributeValues["Enable"].ToLower() == "true" &&
                attributeValues.ContainsKey("EntranceModule") &&
                File.Exists(dllPath) &&
                attributeValues.ContainsKey("EntranceClass") &&
                !string.IsNullOrEmpty(attributeValues["EntranceClass"]))
            {
                Assembly assem = Assembly.LoadFile(dllPath);

                Type curClass = assem.GetType(attributeValues["EntranceClass"]);

                ConstructorInfo curClassConstructor = curClass.GetConstructor(Type.EmptyTypes);//获取不带参的构造函数
                object curClassObject = curClassConstructor.Invoke(new object[] { });
                MethodInfo initMethod = curClass.GetMethod("RunCommand");
                string childMenuName = activeMenuFullName.Split('_').Last();
                initMethod.Invoke(curClassObject, new string[] { childMenuName });
            }
        }

        public static string GetMenuFullName(string dllName,List<string> parentMenuNameList,string childMenuName)
        {
            if (string.IsNullOrEmpty(childMenuName))
                throw new NotImplementedException("RegisterCustomMenu中的childMenuName不能为空！");

            string childMenuFullName = GetParentMenuFullName(dllName, parentMenuNameList);
            childMenuFullName += "_" + childMenuName;
            if (!m_childMenuFullNameMapDllName.ContainsKey(childMenuFullName))
                throw new NotImplementedException("未能找到" + childMenuFullName + "菜单！");
            return childMenuFullName;
        }

        private static string GetParentMenuFullName(string dllName, List<string> parentMenuNameList)
        {
            if (!m_dllNameMapParentMenuName.ContainsKey(dllName))
                throw new NotImplementedException("m_dllNameMapParentMenuName中未能找到" + dllName + ".");

            string childMenuFullName = m_dllNameMapParentMenuName[dllName];
            foreach (string parentMenuName in parentMenuNameList)
            {
                childMenuFullName += "_" + parentMenuName;
            }
            if(parentMenuNameList.Count != 0 && !m_childMenuFullNameMapDllName.ContainsKey(childMenuFullName))
                throw new NotImplementedException("未能找到" + childMenuFullName + "菜单！");

            return childMenuFullName;
        }
    }
}
