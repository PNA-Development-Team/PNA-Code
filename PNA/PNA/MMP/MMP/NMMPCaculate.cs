using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MathematicalTool.PetriNetOperator;
using MathematicalTool.PetriNetTTuple;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;


namespace MMP
{
    public class NMMPCaculate : MMPCaculate
    {
        private HashSet<int> m_MinimalCriticalMarkingNames = new HashSet<int>();
        public NMMPCaculate(Reachability Reachability) : base(Reachability)
        {
            foreach(HashSet<int> item in this.m_MinimalTCriticalMTSIs.Values)
            {
                foreach (int m in item)
                    this.m_MinimalCriticalMarkingNames.Add(m);
            }
        }

        public MathematicalTool.ILPDataFormat GetNMMPILPFormatData(int criticalMarkingName)
        {
            List<string> variables = new List<string>();
            List<int> objectFunction = new List<int>();
            List<List<int>> leftMatrix = new List<List<int>>();
            List<int> leftConstValues = new List<int>();
            List<MathematicalTool.LimitedConditionSign> signs = new List<MathematicalTool.LimitedConditionSign>();
            List<List<int>> rightMatrix = new List<List<int>>();
            List<int> rightConstValues = new List<int>();
            List<bool> gin = new List<bool>();
            List<bool> bin = new List<bool>();
            List<string> annotation = new List<string>();
            annotation.Add(string.Empty);
            annotation.Add(string.Empty);

            List<int> operatorMarkings = this.m_Reachability.GetOperationPlaceNames();

            #region Add Variables

            //Add variables: operationMarkings
            foreach (int m in operatorMarkings)
            {
                variables.Add("l" + m.ToString());
                objectFunction.Add(0);

                gin.Add(true);
                bin.Add(false);
            }

            //Add variables: f_kj
            foreach (int m in this.m_MinimalCriticalMarkingNames)
            {
                variables.Add("f" + criticalMarkingName.ToString() + "_" + m.ToString());
                objectFunction.Add(1);

                gin.Add(false);
                bin.Add(true);
            }

            //Add variables: Belta
            variables.Add("Belta");
            objectFunction.Add(0);
            gin.Add(true);
            bin.Add(false);

            //Add variables: Omega
            foreach(int t in this.m_Reachability.CriticalTransitions)
            {
                variables.Add("Omega" + t.ToString());
                objectFunction.Add(0);
                gin.Add(true);
                bin.Add(false);
            }

            #endregion

            #region Add Constraints

            foreach (int markingName in this.m_MinimalLegalMarkings)
            {
                Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                List<int> leftRow = new List<int>();
                List<int> rightRow = new List<int>();

                //p_i
                foreach (int operatorMarkingName in operatorMarkings)
                {
                    int value = m.GetMarkingValueFromPlaceName(operatorMarkingName);
                    leftRow.Add(value);
                    rightRow.Add(0);
                }

                //f_kj
                foreach (int mm in this.m_MinimalCriticalMarkingNames)
                {
                    leftRow.Add(0);
                    rightRow.Add(0);
                }

                //Belta
                leftRow.Add(0);
                rightRow.Add(1);

                //Omega
                foreach(int t in this.m_Reachability.CriticalTransitions)
                {
                    leftRow.Add(0);
                    rightRow.Add(0);
                }


                leftMatrix.Add(leftRow);
                signs.Add(MathematicalTool.LimitedConditionSign.LessOrEqual);
                leftConstValues.Add(0);
                rightMatrix.Add(rightRow);
                rightConstValues.Add(0);
                annotation.Add("legal marking*: " + markingName.ToString());
            }

            foreach(KeyValuePair<int,HashSet<int>> item in this.m_MMinimalTEnabledGoodMarkings)
            {
                foreach(int markingName in item.Value)
                {
                    List<int> leftRow = new List<int>();
                    List<int> rightRow = new List<int>();
                    Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                    foreach(int operatorPlace in operatorMarkings)
                    {
                        int value = m.GetMarkingValueFromPlaceName(operatorPlace) +
                                    this.m_Reachability.IncidenceMatrix.GetValueFromMarkingNameAndTransitionName(operatorPlace, item.Key);
                        leftRow.Add(value);
                        rightRow.Add(0);
                    }

                    foreach (int mm in this.m_MinimalCriticalMarkingNames)
                    {
                        leftRow.Add(0);
                        rightRow.Add(0);
                    }

                    //Belta
                    leftRow.Add(0);
                    rightRow.Add(1);

                    //Omega
                    foreach(int t in this.m_Reachability.CriticalTransitions)
                    {
                        leftRow.Add(0);
                        if (t == item.Key)
                            rightRow.Add(-1);
                        else
                            rightRow.Add(0);
                    }

                    leftMatrix.Add(leftRow);
                    leftConstValues.Add(0);
                    signs.Add(MathematicalTool.LimitedConditionSign.LessOrEqual);
                    rightMatrix.Add(rightRow);
                    rightConstValues.Add(0);
                    annotation.Add("t" + item.Key + "-enabled good marking** : " + markingName.ToString());
                }
            }

            foreach (KeyValuePair<int, HashSet<int>> item in this.m_MinimalTCriticalMTSIs)
            {
                foreach (int markingName in item.Value)
                {
                    List<int> leftRow = new List<int>();
                    List<int> rightRow = new List<int>();
                    Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                    foreach (int operatorPlace in operatorMarkings)
                    {
                        int value = m.GetMarkingValueFromPlaceName(operatorPlace) +
                                    this.m_Reachability.IncidenceMatrix.GetValueFromMarkingNameAndTransitionName(operatorPlace, item.Key);
                        leftRow.Add(value);
                        rightRow.Add(0);
                    }

                    foreach (int mm in this.m_MinimalCriticalMarkingNames)
                    {
                        leftRow.Add(0);
                        if (mm == markingName)
                            rightRow.Add(MathematicalTool.ILP.ILPMaxNum);
                        else
                            rightRow.Add(0);
                    }

                    //Belta
                    leftRow.Add(0);
                    rightRow.Add(1);

                    //Omega
                    foreach (int t in this.m_Reachability.CriticalTransitions)
                    {
                        leftRow.Add(0);
                        if (t == item.Key)
                            rightRow.Add(-1);
                        else
                            rightRow.Add(0);
                    }

                    leftMatrix.Add(leftRow);
                    leftConstValues.Add(0);
                    signs.Add(MathematicalTool.LimitedConditionSign.GreaterOrEqual);
                    rightMatrix.Add(rightRow);
                    rightConstValues.Add(1 - MathematicalTool.ILP.ILPMaxNum);
                    annotation.Add("MTSIs*: (" + markingName + " , t" + item.Key + ")");
                }
            }

            List<int> lastLeftRow = new List<int>();
            List<int> lastRightRow = new List<int>();

            //p_i
            foreach (int operatorMarkingName in operatorMarkings)
            {
                lastLeftRow.Add(0);
                lastRightRow.Add(0);
            }

            //f_kj
            foreach (int mm in this.m_MinimalCriticalMarkingNames)
            {
                if (mm == criticalMarkingName)
                    lastLeftRow.Add(1);
                else
                    lastLeftRow.Add(0);
                lastRightRow.Add(0);
            }

            //Belta
            lastLeftRow.Add(0);
            lastRightRow.Add(0);

            //Omega
            foreach (int t in this.m_Reachability.CriticalTransitions)
            {
                lastLeftRow.Add(0);
                lastRightRow.Add(0);
            }

            leftMatrix.Add(lastLeftRow);
            signs.Add(MathematicalTool.LimitedConditionSign.Equal);
            leftConstValues.Add(0);
            rightMatrix.Add(lastRightRow);
            rightConstValues.Add(1);

            #endregion

            return new MathematicalTool.ILPDataFormat(variables, objectFunction, true, leftMatrix, leftConstValues,
                                                        signs, rightMatrix, rightConstValues, gin, bin,annotation);
        }

        public void ExportNMMPILPFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "ILP file (*.ilp)|*.ilp| All file (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            foreach (int criticalMarkingName in this.m_MinimalCriticalMarkingNames)
            {
                string filePath = dialog.FileName;
                filePath = filePath.Remove(filePath.Length - 4, 4);
                filePath += "-M" + criticalMarkingName + ".ilp";
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                MathematicalTool.ILPDataFormat data = GetNMMPILPFormatData(criticalMarkingName);
                string str = MathematicalTool.ILP.CreateILPFileString(ref data);
                sw.Write(str);
                sw.Close();
                fs.Close();
            }
        }

        public void ExportXls()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter += "Excel file (*.xlsx)|.xlsx | all file (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            FileStream fs = new FileStream(dialog.FileName, FileMode.Create);
            XSSFWorkbook workBook = new XSSFWorkbook();
            workBook.CreateSheet("Sheet1");
            XSSFSheet sheet = (XSSFSheet)workBook.GetSheet("Sheet1");

            #region fontStyle

            ICellStyle style = workBook.CreateCellStyle();
            IFont font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            style.SetFont(font);
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            #endregion

            sheet.CreateRow(0);
            XSSFRow row = (XSSFRow)sheet.GetRow(0);
            row.Height = 17 * 20;
            
            XSSFCell cell = (XSSFCell)row.CreateCell(0);
            cell.CellStyle = style;
            int count = 1;
            foreach(int markingName in this.m_MinimalCriticalMarkingNames)
            {
                cell = (XSSFCell)row.CreateCell(count++);
                cell.CellStyle = style;
                cell.SetCellValue(markingName);
            }

            foreach(int operatorMarking in this.m_Reachability.GetOperationPlaceNames())
            {
                cell = (XSSFCell)row.CreateCell(count++);
                cell.CellStyle = style;
                cell.SetCellValue("l"+operatorMarking.ToString());
            }

            cell = (XSSFCell)row.CreateCell(count++);
            cell.CellStyle = style;
            cell.SetCellValue("Belta");

            foreach(int t in this.m_Reachability.CriticalTransitions)
            {
                cell = (XSSFCell)row.CreateCell(count++);
                cell.CellStyle = style;
                cell.SetCellValue("Omega"+t.ToString());
            }

            int rowCount = 1;
            foreach (int markingName in this.m_MinimalCriticalMarkingNames)
            {
                row = (XSSFRow)sheet.CreateRow(rowCount++);
                row.Height = 17 * 20;
                cell = (XSSFCell)row.CreateCell(0);
                cell.CellStyle = style;
                cell.SetCellValue(markingName);
                for (int i = 1;i<count;i++)
                {
                    cell = (XSSFCell)row.CreateCell(i);
                    cell.CellStyle = style;
                }
            }

            workBook.Write(fs);
            fs.Close();
            workBook.Close();
        }
    }
}
