using System;
using System.Linq;
using System.Collections.Generic;
using MathematicalTool;
using System.IO;
using Utility;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Windows.Forms;
using System.Configuration;


namespace Test
{
    public class ILP
    {
        private string m_ResultFilePath = string.Empty;

        private List<string> m_EcxelFiles = new List<string>();

        private Dictionary<string, Dictionary<string, string>> m_Result = new Dictionary<string, Dictionary<string, string>>();

        public ILP(string resultFilePath, string excelFileDirectoryPath)
        {
            this.m_ResultFilePath = resultFilePath;
            m_EcxelFiles = Utility.FileHelper.GetAllFormatFiles(excelFileDirectoryPath, "lgr");
            if (m_EcxelFiles.Count == 0)
                throw new NotSupportedException("Can not find any .lgr file.");
            foreach (string filePath in this.m_EcxelFiles)
                AnalyzeLgrFile(filePath);
            this.ExportExcelFile();
            this.CreateNMMPResultILPFile();
        }

        public void AnalyzeLgrFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string name = fileInfo.Name.Replace(".lgr", "");
            this.m_Result.Add(name, new Dictionary<string, string>());

            FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            for (int i = 0; i < 23; i++)
                sr.ReadLine();

            while (sr.Peek() >= 0)
            {
                string str = sr.ReadLine();
                if (str == string.Empty)
                    break;
                int index = 0;
                while (str[index] == ' ' && index < str.Length)
                    index++;
                str = str.Remove(0, index);
                List<string> strList = str.Split(' ').ToList();
                strList.RemoveAll(s => s == string.Empty);
                if (strList[0].StartsWith("F"))
                {
                    List<string> temp = strList[0].Split('_').ToList();
                    this.m_Result[name].Add(temp[1].Trim(), strList[1].Trim());
                }
                else
                {
                    this.m_Result[name].Add(strList[0].Trim(), strList[1].Trim());
                }
            }

            sr.Close();
            fs.Close();
        }

        public void ExportExcelFile()
        {
            if (this.m_Result.Count == 0)
                throw new NotSupportedException("Result is empty.");

            FileInfo fileInfo = new FileInfo(this.m_ResultFilePath);
            FileStream fs = new FileStream(fileInfo.FullName, FileMode.Create);
            HSSFWorkbook workBook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workBook.CreateSheet(fileInfo.Name);

            #region fontStyle

            ICellStyle style = workBook.CreateCellStyle();
            IFont font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            style.SetFont(font);
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            #endregion

            Dictionary<string, int> columnNameMapIndex = new Dictionary<string, int>();

            #region Add Sheet Titles

            sheet.CreateRow(0);
            HSSFRow row = (HSSFRow)sheet.GetRow(0);
            row.Height = 17 * 20;

            HSSFCell cell = (HSSFCell)row.CreateCell(0);
            cell.CellStyle = style;
            int columnCount = 1;
            foreach (string markingName in this.m_Result.First().Value.Keys.ToList())
            {
                cell = (HSSFCell)row.CreateCell(columnCount);
                cell.CellStyle = style;
                cell.SetCellValue(markingName);
                columnNameMapIndex.Add(markingName, columnCount);
                columnCount++;
            }

            #endregion

            int rowCount = 1;
            foreach (KeyValuePair<string, Dictionary<string, string>> item in this.m_Result)
            {
                row = (HSSFRow)sheet.CreateRow(rowCount);
                cell = (HSSFCell)row.CreateCell(0);
                cell.CellStyle = style;
                cell.SetCellValue(item.Key);
                foreach (KeyValuePair<string, string> subItem in item.Value)
                {
                    cell = (HSSFCell)row.CreateCell(columnNameMapIndex[subItem.Key]);
                    cell.CellStyle = style;
                    cell.SetCellValue(Convert.ToInt32(Convert.ToDouble(subItem.Value)).ToString());
                }

                rowCount++;
            }

            workBook.Write(fs);
            workBook.Close();
            fs.Close();
            
        }

        public void CreateNMMPResultILPFile()
        {
            #region ILPFormateData

            List<string> variables = new List<string>();
            List<int> objectiveFunction = new List<int>();
            List<List<int>> leftMatrix = new List<List<int>>();
            List<int> leftConstValue = new List<int>();
            List<MathematicalTool.LimitedConditionSign> signs = new List<MathematicalTool.LimitedConditionSign>();
            List<List<int>> rightMatrix = new List<List<int>>();
            List<int> rightConstValue = new List<int>();
            List<bool> gin = new List<bool>();
            List<bool> bin = new List<bool>();
            List<string> annotation = new List<string>();

            foreach(string s in this.m_Result.Keys)
            {
                variables.Add("v"+s);
                objectiveFunction.Add(1);
                gin.Add(false);
                bin.Add(true);
            }

            foreach(string s in this.m_Result.First().Value.Keys)
            {
                if (s.StartsWith("L") || s.StartsWith("O") || s.StartsWith("B"))
                    continue;
                List<int> leftRow = new List<int>();
                List<int> rightRow = new List<int>();
                foreach (Dictionary<string,string> dic in this.m_Result.Values)
                {
                    leftRow.Add((int)Convert.ToDouble(dic[s]));
                    rightRow.Add(0);
                }
                leftMatrix.Add(leftRow);
                rightMatrix.Add(rightRow);
                leftConstValue.Add(0);
                rightConstValue.Add(1);
                signs.Add(LimitedConditionSign.GreaterOrEqual);
            }

            MathematicalTool.ILPDataFormat data = new ILPDataFormat(variables, objectiveFunction, false,
                                                                    leftMatrix,leftConstValue,signs,rightMatrix,
                                                                    rightConstValue,gin,bin,annotation);
            string dataStr = MathematicalTool.ILP.CreateILPFileString(ref data);

            #endregion

            string filePath = this.m_ResultFilePath.Replace(".xls", ".ilp");
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(dataStr);

            sw.Close();
            fs.Close();
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            DialogResult result = folderDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel File (*.xls)|*.xls | all file (*.*)|*.*";
            result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            ILP test = new ILP(saveFileDialog.FileName,folderDialog.SelectedPath);
        }
    }
}

