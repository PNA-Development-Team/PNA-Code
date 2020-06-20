using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MathematicalTool;
using MathematicalTool.PetriNetTTuple;
using MathematicalTool.PetriNetOperator;

namespace PntFile.Forms
{
    public struct ROWDATA
    {
        public int placeName;
        public int initialTokenNum;
        public KeyValuePair<int, int> preTransition;
        public KeyValuePair<int, int> postTransition;
    }
    public partial class CreatePntForm : Form
    {
        #region DECLARATION 

        private enum ColumnsName
        {
            Place, Token, Pre_Transition, Pre_Weight, Post_Transition, Post_Weight
        }

        #endregion

        #region STATIC VARIOUS & STATIC FUNCTIONS

        private static CreatePntForm m_Instance = null;
        public static CreatePntForm Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CreatePntForm();
                    return m_Instance;
                }
                return m_Instance;
            }
        }

        public static void ShowWindow()
        {
            Instance.Show();
        }

        #endregion

        #region DYNAMIC FUNCTIONS

        private SortedDictionary<int, Place> m_PlaceTable = new SortedDictionary<int, Place>();
        
        public CreatePntForm()
        {
            InitializeComponent();
            Utility.FormHelper.SetFormStartPositionAtCentral(this, this.ParentForm);
            dgvTable.CellEndEdit += dgvTable_CellEndEdit;
            dgvTable.RowsAdded += dgvTable_RowsAdded;
            dgvTable.UserDeletingRow += dgvTable_UserDeletingRow;
            dgvTable.RowPostPaint += dgvTable_RowPostPaint;
            dgvTable.CellValueChanged += dgvTable_CellValueChanged;
            dgvTable.CellBeginEdit += dgvTable_CellBeginEdit;

            dgvTable.Rows.Add(1);

            cbExportMatrix.SelectedIndex = cbExportMatrix.Items.Count - 1;
            cbExportMatrix.SelectedIndexChanged += CbExportMatrix_SelectedIndexChanged;
        }

        private void CbExportMatrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = cbExportMatrix.SelectedItem as string;
            if (item == "Export Matrix")
                return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "matrix files (*.matrix)|*.matrix| All files (*.*)|*.*";
            DialogResult result = saveFileDialog.ShowDialog();
            if (result != DialogResult.OK){
                cbExportMatrix.SelectedIndex = cbExportMatrix.Items.Count - 1;
                return;
            }
                
            FileInfo fileInfo = new FileInfo(saveFileDialog.FileName);
            if (!Directory.Exists(fileInfo.DirectoryName))
                return;
            if (File.Exists(fileInfo.FullName))
                File.Delete(fileInfo.FullName);
            FileStream fs = new FileStream(fileInfo.FullName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            MathematicalTool.Matrix data = new Matrix();
            switch (item)
            {
                case "Pre Incidence Matrix":
                    data = PetriNetOperator.GetPreIncidenceMatrixFromPlaceDictionary(this.m_PlaceTable);
                    break;
                case "Post Incidence Matrix":
                    data = PetriNetOperator.GetPostIncidenceMatrixFromPlaceDictionary(this.m_PlaceTable);
                    break;
                case "Inceidence Matrix":
                    data = PetriNetOperator.GetIncidenceMatrixFromPlaceDictionary(this.m_PlaceTable);
                    break;
            }

            
            string T = "T = {";
            SortedSet<int> transitionNames = PetriNetOperator.GetTransitionNamesFromPlaceDictionary(this.m_PlaceTable);
            foreach(int name in transitionNames)
            {
                T += "t" + name.ToString() + " , ";
            }

            string P = "P = {";
            string initialMarking = "M0 = [";
            foreach(KeyValuePair<int,Place> subitem in this.m_PlaceTable)
            {
                P += "p" + subitem.Key.ToString() + " , ";
                initialMarking += subitem.Value.InitialTokenNum.ToString() + " ";
            }
            T = T.Remove(T.Length - 2, 2);
            T += "}";
            P = P.Remove(P.Length - 2, 2);
            P += "}";
            initialMarking = initialMarking.Remove(initialMarking.Length - 1, 1);
            initialMarking += "]";
            sw.WriteLine(P);
            sw.WriteLine(T);
            sw.WriteLine(initialMarking);
            sw.WriteLine("N = "+ data.ToString());
            sw.Close();
            fs.Close();

            cbExportMatrix.SelectedIndex = cbExportMatrix.Items.Count - 1;
            setNotice("Export matrix success.");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Enter)
            {
                if (dgvTable.CurrentCell == null){
                    dgvTable.Rows.Add(1);
                    dgvTable.CurrentCell = dgvTable[0, 0];
                    return true;
                }
                if (dgvTable.CurrentCell.ColumnIndex < (int)ColumnsName.Post_Weight)
                    dgvTable.CurrentCell = dgvTable[dgvTable.CurrentCell.ColumnIndex + 1, dgvTable.CurrentCell.RowIndex];
                else if (dgvTable.CurrentCell.RowIndex < dgvTable.RowCount - 1)
                    dgvTable.CurrentCell = dgvTable[(int)ColumnsName.Place, dgvTable.CurrentCell.RowIndex + 1];
                else
                {
                    btAddPlace.PerformClick();
                }
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            removeDataFromTable(e.RowIndex);
        }

        private void dgvTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgvTable.Update();
        }

        private void dgvTable_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                                e.RowBounds.Location.Y,
                                                dgvTable.RowHeadersWidth - 4,
                                                e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                  dgvTable.RowHeadersDefaultCellStyle.Font,
                                  rectangle,
                                  dgvTable.RowHeadersDefaultCellStyle.ForeColor,
                                  TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvTable_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void dgvTable_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            setNotice("RowsDeleting");
        }

        private void dgvTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = (sender as DataGridView)[e.ColumnIndex,e.RowIndex];

            switch (e.ColumnIndex)
            {
                case (int)ColumnsName.Place: checkPlace(cell); break;
                case (int)ColumnsName.Token: checkToken(cell); break;
                case (int)ColumnsName.Pre_Transition: checkPreTransition(cell); break;
                case (int)ColumnsName.Pre_Weight: checkPreWeight(cell); break;
                case (int)ColumnsName.Post_Transition: checkPostTransition(cell); break;
                case (int)ColumnsName.Post_Weight: checkPostTransition(cell); break;
                default:break;
            }


            if (getDgvTableValue(e.RowIndex,(int)ColumnsName.Place) != -1 &&
                getDgvTableValue(e.RowIndex,(int)ColumnsName.Token) != -1)
            {
                ROWDATA rowData = getDgvTableRowData(e.RowIndex);

                if (m_PlaceTable.ContainsKey(rowData.placeName))
                {           
                    if (rowData.preTransition.Key != 0 && rowData.preTransition.Value != 0)
                    {
                        if (m_PlaceTable[rowData.placeName].PreTransitions.ContainsKey(rowData.preTransition.Key))
                            m_PlaceTable[rowData.placeName].PreTransitions[rowData.preTransition.Key] = Convert.ToInt32(rowData.preTransition.Value);
                        else
                            m_PlaceTable[rowData.placeName].PreTransitions.Add(rowData.preTransition.Key, Convert.ToInt32(rowData.preTransition.Value));
                    }
                        
                    if (rowData.postTransition.Key != 0 && rowData.postTransition.Value != 0)
                    {
                        if (m_PlaceTable[rowData.placeName].PostTransitions.ContainsKey(rowData.postTransition.Key))
                            m_PlaceTable[rowData.placeName].PostTransitions[rowData.postTransition.Key] = Convert.ToInt32(rowData.postTransition.Value);
                        else
                            m_PlaceTable[rowData.placeName].PostTransitions.Add(rowData.postTransition.Key, Convert.ToInt32(rowData.postTransition.Value));
                    }                 
                }
                else
                {
                    m_PlaceTable.Add(rowData.placeName, new Place(rowData.placeName,rowData.initialTokenNum,
                                                                  rowData.preTransition,rowData.postTransition));
                }

            }
        }

        private void btAddPlace_Click(object sender, EventArgs e)
        {
            dgvTable.Rows.Insert(dgvTable.RowCount, 1);
            int lastPlaceName = 0;
            for(int i = dgvTable.Rows.Count - 2;i >= 0;i--)
            {
                if (dgvTable[(int)ColumnsName.Place, i].Value == null)
                    break;
                if(dgvTable[(int)ColumnsName.Place,i].Value.ToString() != string.Empty)
                {
                    lastPlaceName = Convert.ToInt32(dgvTable[(int)ColumnsName.Place, i].Value.ToString().Trim());
                    break;
                }
            }
            if(lastPlaceName != 0)
            {
                int initialTokenNum = m_PlaceTable[lastPlaceName].InitialTokenNum;
                setDgvTableValue(dgvTable.RowCount - 1, (int)ColumnsName.Place, lastPlaceName);
                setDgvTableValue(dgvTable.RowCount - 1, (int)ColumnsName.Token, initialTokenNum);
                dgvTable.CurrentCell = dgvTable[(int)ColumnsName.Place, dgvTable.RowCount - 1];
            }

            setNotice("The short Cut \"Add\" is Alt+A.");
        }

        private void btDeletePlace_Click(object sender, EventArgs e)
        {
            if(dgvTable.SelectedRows.Count == 0)
            {
                setNotice("Please select some rows before deleting.");
                return;
            }

            foreach(DataGridViewRow item in dgvTable.SelectedRows)
            {
                removeDataFromTable(item.Index);
                dgvTable.Rows.Remove(item);
            }

            setNotice("The short Cut \"Delete\" is Alt+D.");
        }

        private void btImportPnt_Click(object sender, EventArgs e)
        {
            setNotice("The short Cut \"Import\" is Alt+I.");
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pnt files (*.pnt)|*.pnt| All files (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            this.m_PlaceTable.Clear();
            this.m_PlaceTable = PntFileOperation.GetDataFromPntFile(dialog.FileName);
            if (this.m_PlaceTable == null)
                return;
            for (int i = this.dgvTable.Rows.Count-1; i >= 0; i--)
                this.dgvTable.Rows.Remove(this.dgvTable.Rows[i]);
            foreach(Place p in this.m_PlaceTable.Values.ToList())
            {
                List<KeyValuePair<int, int>> preList = p.PreTransitions.ToList();
                List<KeyValuePair<int, int>> postList = p.PostTransitions.ToList();

                int minCount = preList.Count < postList.Count ? preList.Count : postList.Count;
                for(int i = 0; i < minCount; i++)
                {
                    dgvTable.Rows.Add();
                    List<int> valuesList = new List<int> {p.PlaceName,p.InitialTokenNum,
                            preList[i].Key, preList[i].Value,postList[i].Key, postList[i].Value};
                    setDgvTableRowValue(dgvTable.RowCount - 1, valuesList);
                }
                if(minCount == preList.Count)
                {
                    for (int i = minCount; i < postList.Count; i++)
                    {
                        dgvTable.Rows.Add();
                        List<int> valuesList = new List<int> {p.PlaceName,p.InitialTokenNum,
                            0, 0,postList[i].Key, postList[i].Value};
                        setDgvTableRowValue(dgvTable.RowCount - 1, valuesList);
                    }
                }
                else
                {
                    for (int i = minCount; i < preList.Count; i++)
                    {
                        dgvTable.Rows.Add();
                        List<int> valuesList = new List<int> {p.PlaceName,p.InitialTokenNum,
                            preList[i].Key, preList[i].Value,0, 0};
                        setDgvTableRowValue(dgvTable.RowCount - 1, valuesList);
                    }
                }
            }
        }

        private void btExportPnt_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFIleDialog = new SaveFileDialog();
            saveFIleDialog.Filter = "pnt files (*.pnt)|*.pnt| All files (*.*)|*.*";
            DialogResult result = saveFIleDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            
            FileInfo fileInfo = new FileInfo(saveFIleDialog.FileName);
            if (!Directory.Exists(fileInfo.DirectoryName))
                return;
            if (File.Exists(fileInfo.FullName))
                File.Delete(fileInfo.FullName);
            FileStream fs = new FileStream(fileInfo.FullName,FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine("P        M        PRE,        POST        NETZ 0:");

            foreach(KeyValuePair<int,Place> item in this.m_PlaceTable)
            {
                sw.WriteLine(item.Value.ToString());
            }

            sw.WriteLine("@");

            sw.Close();
            fs.Close();

            setNotice("The short Cut \"Export\" is Alt+E.");
            setNotice("Export succeed.");
        }

        private void btExportGra_Click(object sender, EventArgs e)
        {
            PetriNetOperator.ExportReabilityGraphFileFromPlaceDiationary(this.m_PlaceTable);
            setNotice("Export reability graph file success.");
        }

        private ROWDATA getDgvTableRowData(int rowIndex)
        {
            ROWDATA rowData;
            rowData.placeName = getDgvTableValue(dgvTable[(int)ColumnsName.Place, rowIndex]);
            rowData.initialTokenNum = getDgvTableValue(dgvTable[(int)ColumnsName.Token, rowIndex]);
            int key = getDgvTableValue(dgvTable[(int)ColumnsName.Pre_Transition, rowIndex]);
            int value = getDgvTableValue(dgvTable[(int)ColumnsName.Pre_Weight, rowIndex]);
            rowData.preTransition = new KeyValuePair<int, int>(key, value);
            key = getDgvTableValue(dgvTable[(int)ColumnsName.Post_Transition, rowIndex]);
            value = getDgvTableValue(dgvTable[(int)ColumnsName.Post_Weight, rowIndex]);
            rowData.postTransition = new KeyValuePair<int, int>(key, value);
            return rowData;
        }

        private int getDgvTableValue(DataGridViewCell cell)
        {
            return getDgvTableValue(cell.RowIndex, cell.ColumnIndex);
        }
        private int getDgvTableValue(int rowIndex,int columnIndex)
        {
            if (rowIndex > dgvTable.RowCount - 1 || columnIndex > dgvTable.ColumnCount - 1)
            {
                Utility.DebugHelper.Log("Can not find cell at " + rowIndex.ToString() + " row," + columnIndex.ToString() + "column in dgvTable.");
                return -1;
            }
            else if (dgvTable[columnIndex, rowIndex].Value != null)
                return Convert.ToInt32(dgvTable[columnIndex, rowIndex].Value.ToString().Trim());
            else
                return -1;
        }

        private void setDgvTableRowValue(int rowIndex,List<int> valueList)
        {
            setDgvTableValue(rowIndex, (int)ColumnsName.Place, valueList[0]);
            setDgvTableValue(rowIndex, (int)ColumnsName.Token, valueList[1]);
            setDgvTableValue(rowIndex, (int)ColumnsName.Pre_Transition, valueList[2]);
            setDgvTableValue(rowIndex, (int)ColumnsName.Pre_Weight, valueList[3]);
            setDgvTableValue(rowIndex, (int)ColumnsName.Post_Transition, valueList[4]);
            setDgvTableValue(rowIndex, (int)ColumnsName.Post_Weight, valueList[5]);
        }
        private void setDgvTableValue(DataGridViewCell cell, int value)
        {
            setDgvTableValue(cell.RowIndex, cell.ColumnIndex, value);
        }
        private void setDgvTableValue(int rowIndex,int columnIndex,int value)
        {
            if (rowIndex > dgvTable.RowCount - 1 || columnIndex > dgvTable.ColumnCount - 1)
                throw new NotImplementedException("Can not find cell at " + rowIndex.ToString() + " row," + columnIndex.ToString() + "column in dgvTable.");
            else if(columnIndex == (int)ColumnsName.Token || value != 0)
                dgvTable[columnIndex, rowIndex].Value = value;
            dgvTable.Update();
        }

        private void checkPlace(DataGridViewCell cell)
        {
            if (cell.Value == null)
            {
                dgvTable.Rows.RemoveAt(cell.RowIndex);
                return;
            }
            if (!checkCellValueIsNumber(cell,0))
                return;
            if (m_PlaceTable.ContainsKey(Convert.ToInt32(cell.Value.ToString().Trim())))
            {
                setDgvTableValue(cell.RowIndex, (int)ColumnsName.Token, m_PlaceTable[Convert.ToInt32(cell.Value.ToString().Trim())].InitialTokenNum);
            }
            else
                setDgvTableValue(cell.RowIndex, (int)ColumnsName.Token, 0);
        }

        private void checkToken(DataGridViewCell cell)
        {
            if(getDgvTableValue(cell.RowIndex,(int)ColumnsName.Place) == -1)
                return;
            if (!checkCellValueIsNumber(cell, 0))
                return;

            int placeName = getDgvTableValue(cell.RowIndex, (int)ColumnsName.Place);
            int tokenNumStr = getDgvTableValue(cell.RowIndex, (int)ColumnsName.Token);
            int tokenNumber = tokenNumStr == 0 ? 0 : Convert.ToInt32(tokenNumStr);

            if (m_PlaceTable.ContainsKey(placeName) && 
                m_PlaceTable[placeName].InitialTokenNum != tokenNumber)
            {
                m_PlaceTable[placeName].InitialTokenNum = tokenNumber;
                foreach(DataGridViewRow row in dgvTable.Rows)
                {
                    if(row.Cells[0].Value != null && Convert.ToInt32(row.Cells[0].Value.ToString()) == placeName)
                    {
                        setDgvTableValue(row.Index, (int)ColumnsName.Token, tokenNumber);
                    }
                }    
            }
        }

        private void checkPreTransition(DataGridViewCell cell)
        {
            if (cell.Value == null)
                return;
            if (!checkCellValueIsNumber(cell, 0))
            {
                setDgvTableValue(cell.RowIndex, (int)ColumnsName.Pre_Weight, 0);
                return;
            }         
            int placeName = getDgvTableValue(cell.RowIndex, (int)ColumnsName.Place);
            if (!m_PlaceTable.ContainsKey(placeName) ||
                !m_PlaceTable[placeName].PreTransitions.ContainsKey(Convert.ToInt32(cell.Value.ToString().Trim())))
            {
                if (getDgvTableValue(cell.RowIndex, (int)ColumnsName.Pre_Weight) == -1)
                    setDgvTableValue(cell.RowIndex, (int)ColumnsName.Pre_Weight, 1);
                return;
            }         
            else
            {
                setNotice("Pre transition " + cell.Value.ToString() + "has existed at Place " + placeName);
                setDgvTableValue(cell, 0);
            }
        }

        private void checkPreWeight(DataGridViewCell cell)
        {
            if (cell.Value == null)
                return;
            if (!checkCellValueIsNumber(cell, 0))
                return;
        }

        private void checkPostTransition(DataGridViewCell cell)
        {
            if (cell.Value == null)
                return;
            if (!checkCellValueIsNumber(cell, 0))
            {
                setDgvTableValue(cell.RowIndex, (int)ColumnsName.Post_Weight, 0);
                return;
            }
            int placeName = getDgvTableValue(cell.RowIndex, (int)ColumnsName.Place);
            if (!m_PlaceTable.ContainsKey(placeName) || 
                !m_PlaceTable[placeName].PostTransitions.ContainsKey(Convert.ToInt32(cell.Value.ToString())))
            {
                if (getDgvTableValue(cell.RowIndex, (int)ColumnsName.Post_Weight) == -1)
                    setDgvTableValue(cell.RowIndex, (int)ColumnsName.Post_Weight, 1);
                return;
            }
            else
            {
                setNotice("Post transition " + cell.Value.ToString() + "has existed at Place " + placeName);
                setDgvTableValue(cell, 0);
            }
        }

        private void checkPostWeight(DataGridViewCell cell)
        {
            if (cell.Value == null)
                return;
            if (!checkCellValueIsNumber(cell, 0))
                return;
        }

        private bool checkCellValueIsNumber(DataGridViewCell cell,int resetValue)
        {
            try
            {
                int placeNum = Convert.ToInt32(cell.Value);
            }
            catch
            {
                var enumValue = (ColumnsName)cell.ColumnIndex;
                setNotice("Pleace input a number at " + enumValue);
                setDgvTableValue(cell, resetValue);
                return false ;
            }

            return true;
        }

        private void setNotice(string text)
        {
            lbNotice.Text = "Notice: " + text;
            lbNotice.Update();
        }

        private void removeDataFromTable(int rowIndex)
        {
            ROWDATA rowData = getDgvTableRowData(rowIndex);
            if (m_PlaceTable.ContainsKey(rowData.placeName))
            {
                if (m_PlaceTable[rowData.placeName].PreTransitions.ContainsKey(rowData.preTransition.Key))
                    m_PlaceTable[rowData.placeName].PreTransitions.Remove(rowData.preTransition.Key);
                if (m_PlaceTable[rowData.placeName].PostTransitions.ContainsKey(rowData.postTransition.Key))
                    m_PlaceTable[rowData.placeName].PostTransitions.Remove(rowData.postTransition.Key);

                if (m_PlaceTable[rowData.placeName].PreTransitions.Count == 0 &&
                   m_PlaceTable[rowData.placeName].PostTransitions.Count == 0)
                    m_PlaceTable.Remove(rowData.placeName);
            }
        }

        #endregion

        private void CreatePntForm_Load(object sender, EventArgs e)
        {

        }
    }
}
