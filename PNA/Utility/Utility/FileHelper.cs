using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Utility
{
    public static class FileHelper
    {
        public static List<string> GetAllConfigFiles(string folderPath)
        {
            List<string> allConfigFiles = new List<string>();

            List<string> allXmlFiles = GetAllFormatFiles(folderPath,".xml",true);
            foreach(string xmlFile in allXmlFiles)
            {
                FileInfo fileInfo = new FileInfo(xmlFile);
                if (fileInfo.Name == "Config.xml")
                    allConfigFiles.Add(fileInfo.FullName);
            }

            return allConfigFiles;
        }

        public static List<string> GetAllFormatFiles(string folderPath, string format)
        {
            return GetAllFormatFiles(folderPath,format,false);
        }

        public static List<string> GetAllFormatFiles(string folderPath,string format,bool isLoopSubFolder)
        {
            List<string> allFiles = new List<string>();
            if (string.IsNullOrEmpty(folderPath))
                return allFiles;
            if (!Directory.Exists(folderPath))
                return allFiles;

            DirectoryInfo folderInfo = new DirectoryInfo(folderPath);

            foreach(FileInfo file in folderInfo.GetFiles(format))
            {
                allFiles.Add(file.FullName);
            }

            if (isLoopSubFolder)
            {
                foreach(DirectoryInfo subFolder in folderInfo.GetDirectories())
                {
                    allFiles.AddRange(GetAllFormatFiles(subFolder.FullName,format,true));
                }
            }

            return allFiles;
        }
    }
}
