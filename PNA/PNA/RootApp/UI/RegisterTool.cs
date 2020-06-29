using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;

namespace RootApp.UI
{
    public class RegisterTool
    {
        private static Dictionary<string, string> m_toolButtonFullNameMapDllName = new Dictionary<string, string>();

        public static void AddToolButton(string dllName,string toolButtonName,string toolButtonIconFileFullPath)
        {
            if (!RootApp.UI.RegisterMenu.DllNameMapDllPath.ContainsKey(dllName))
                throw new NotImplementedException("m_dllNameMapParentMenuName中未能找到" + dllName + ".");
            if(string.IsNullOrEmpty(toolButtonName))
                throw new NotImplementedException("添加的工具栏按钮名字不能为空！");
            if(!File.Exists(toolButtonIconFileFullPath))
                throw new NotImplementedException("添加的工具栏按钮时未能找到图标文件！路径为："+ toolButtonIconFileFullPath);

            string toolButtonFullName = RootApp.UI.RegisterMenu.DllNameMapParentMenuName[dllName] + "_" + toolButtonName;
            PNAMainForm.Instance.AddToolButton(toolButtonFullName, toolButtonIconFileFullPath);
            m_toolButtonFullNameMapDllName.Add(toolButtonFullName,dllName);
        }

        public static void AddToolButton(string dllName, string parentMenuName, string childMenuName, string toolButtonIconFileFullPath)
        {
            AddToolButton(dllName,new List<string> { parentMenuName},childMenuName, toolButtonIconFileFullPath);
        }

        public static void AddToolButton(string dllName, List<string> parentMenuNameList, string childMenuName, string toolButtonIconFileFullPath)
        {
            if (!RootApp.UI.RegisterMenu.DllNameMapDllPath.ContainsKey(dllName))
                throw new NotImplementedException("m_dllNameMapParentMenuName中未能找到" + dllName + ".");
            if (string.IsNullOrEmpty(childMenuName))
                throw new NotImplementedException("添加的工具栏按钮名字不能为空！");
            if (!File.Exists(toolButtonIconFileFullPath))
                throw new NotImplementedException("添加的工具栏按钮时未能找到图标文件！路径为：" + toolButtonIconFileFullPath);

            string childMenuFullName = RootApp.UI.RegisterMenu.GetMenuFullName(dllName,parentMenuNameList,childMenuName);
            PNAMainForm.Instance.AddToolButton(childMenuFullName, toolButtonIconFileFullPath);
            m_toolButtonFullNameMapDllName.Add(childMenuFullName, dllName);
        }

        public static void AddToolSeparator()
        {
            PNAMainForm.Instance.AddToolSeparator();
        }

        public static void OnToolCommand(string toolButtonFullName)
        {
            if (!m_toolButtonFullNameMapDllName.ContainsKey(toolButtonFullName))
                return;

            string dllName = m_toolButtonFullNameMapDllName[toolButtonFullName];
            if (!RootApp.UI.RegisterMenu.DllNameMapDllPath.ContainsKey(dllName))
                throw new NotImplementedException("在m_dllNameMapParentMenuName中未能找到dll" + dllName + ".");
            string dllPath = RootApp.UI.RegisterMenu.DllNameMapDllPath[dllName];
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
                string childMenuName = toolButtonFullName.Split('_').Last();
                initMethod.Invoke(curClassObject, new string[] { childMenuName });
            }
        }
    }
}
