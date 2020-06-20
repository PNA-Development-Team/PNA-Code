using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalTool
{
    public class RowVector
    {
        private int m_Count = 0;
        public int Count
        {
            get { return m_Count; }
        }

        private List<double> m_data = new List<double>();

        public double this[int index]
        {
            get
            {
                if (index >= m_data.Count || index < 0)
                    throw new NotSupportedException("Index of row vector is over boundary.");
                return m_data[index];
            }
            set
            {
                if (index >= m_data.Count || index < 0)
                    throw new NotSupportedException("Index of row vector is over boundary.");
                m_data[index] = value;
            }
        }

        public RowVector()
        {
        }
        public RowVector(List<float> data)
        {
            foreach (int value in data)
                m_data.Add((double)value);
            m_Count = data.Count;
        }

        public RowVector(List<int> data)
        {
            foreach (int value in data)
                m_data.Add((double)value);
            m_Count = data.Count;
        }

        public RowVector(List<double> data)
        {
            m_data = data.Where(t => true).ToList();
            m_Count = data.Count;
        }

        public RowVector(RowVector vector)
        {
            m_data = vector.m_data.Where(t=>true).ToList();
            m_Count = vector.m_Count;
        }

        public RowVector(string str)
        {
            if (str.Length == 0)
                return;
            if (str.Contains("'"))
                throw new NotSupportedException("Inputted string is a column vector not row column.");
            str = str.Replace("[","");
            str = str.Replace("]", "");
            List<string> strList = str.Split(' ').ToList();
            foreach (string s in strList)
            {
                string temp = s.Trim();
                if (temp.Length == 0)
                    continue;
                m_data.Add(Convert.ToDouble(temp));
                m_Count++;
            }
        }

        public override string ToString()
        {
            string retStr = "[";
            foreach (double cell in m_data)
            {
                retStr += cell.ToString() + ' ';
            }
            if (retStr.Length > 0)
                retStr.Remove(retStr.Length - 1, 1);
            retStr += "]";
            return retStr;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RowVector))
                throw new NotSupportedException("Can not compare because their type is not same.");
            if (obj == null)
                return (this as object) == null;
            RowVector other = obj as RowVector;
            if (this.Count != other.Count)
                return false;
            else
            {
                for (int i = 0; i < this.Count; i++)
                    if (this[i] != other[i])
                        return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public Matrix Transform2Matrix()
        {
            Matrix matrix = new Matrix(new List<RowVector> { this });
            return matrix;
        }

        public ColumnVector GetTransposeColumnVector()
        {
            return new ColumnVector(this.m_data);
        }

        //Insert data to the end of vector.
        public void Append(double data)
        {
            this.m_data.Add(data);
            this.m_Count++;
        }

        public void Insert(double data,int index)
        {
            if (index >= this.Count)
                throw new NotSupportedException("Index is over boundary when insert data to row vector.");
            this.m_data.Insert(index, data);
            this.m_Count++;
        }

        public void Remove(int index)
        {
            if (index >= this.Count)
                throw new NotSupportedException("Index is over boundary when remove data to row vector.");
            this.m_data.RemoveAt(index);
            this.m_Count--;
        }

        public double MultiplyRightWithColumnVector(ColumnVector column)
        {
            if(this.Count != column.Count)
                throw new NotSupportedException("The count of row vector is not equal to column vector's when row vector is multiplied with matrix.");
            double sum = 0;
            for (int i = 0; i < this.Count; i++)
            {
                sum += this[i] * column[i];
            }
            return sum;
        }

        public Matrix MultiplyLeftWithColumnVector(ColumnVector column)
        {
            List<RowVector> rows = new List<RowVector>();
            for(int i = 0; i < column.Count; i++)
            {
                rows.Add(column[i] * this);
            }
            return new Matrix(rows);
        }

        public RowVector MultiplyRightWithMatrix(Matrix matrix)
        {
            if(this.Count == 0)
                throw new NotSupportedException("The count of row vector is zero when row vector is multiplied with matrix.");
            if(matrix.RowsCount == 0)
                throw new NotSupportedException("The row count of matrix is zero when row vector is multiplied with matrix.");
            if (this.Count != matrix.RowsCount)
                throw new NotSupportedException("The count of row vector is not equal to matrix's row count when row vector is multiplied with matrix.");

            RowVector result = new RowVector();
            foreach(ColumnVector column in matrix.GetColumnVectors())
            {
                result.Append(this * column);
            }
            return result;
        }

        public RowVector MultiplyWithNumber(decimal x)
        {
            RowVector newRow = new RowVector(this);
            for (int i = 0; i < newRow.Count; i++)
                newRow[i] = (double)Math.Round((decimal)newRow[i] * x,5,MidpointRounding.AwayFromZero);
                
            return newRow;
        }

        public RowVector MultiplyWithNumber(double x)
        {
            return MultiplyWithNumber((decimal)x);
        }

        public RowVector Plus(RowVector row)
        {
            if (row.Count != this.m_Count)
                throw new NotSupportedException("Can not plus two row because their count is not equal.");
            RowVector newRow = new RowVector(this);
            for (int i = 0; i < this.Count; i++)
                newRow[i] += row[i];
            return newRow;
        }

        public RowVector Minus(RowVector row)
        {
            return this.Plus(-row);
        }

        public static double operator*(RowVector row,ColumnVector column)
        {
            return row.MultiplyRightWithColumnVector(column);
        }

        public static Matrix operator*(ColumnVector column,RowVector row)
        {
            return row.MultiplyLeftWithColumnVector(column);
        }

        public static RowVector operator *(RowVector row, int x)
        {
            return row.MultiplyWithNumber((decimal)x);
        }

        public static RowVector operator *(RowVector row, float x)
        {
            return row.MultiplyWithNumber(x);
        }

        public static RowVector operator *(RowVector row, decimal x)
        {
            return row.MultiplyWithNumber(x);
        }

        public static RowVector operator*(RowVector row ,double x)
        {
            return row.MultiplyWithNumber(x);
        }

        public static RowVector operator*(double x,RowVector row)
        {
            return row * x;
        }

        public static RowVector operator+(RowVector row1,RowVector row2)
        {
            return row1.Plus(row2);
        }

        public static RowVector operator-(RowVector row1)
        {
            RowVector newRow = new RowVector(row1);
            for(int i=0;i<newRow.Count;i++)
            {
                newRow[i] = 0 - newRow[i];
            }
            return newRow;
        }

        public static RowVector operator-(RowVector row1,RowVector row2)
        {
            return row1.Minus(row2);
        }

        public static bool operator == (RowVector row1,RowVector row2)
        {
            if ((row1 as object) == null || (row2 as object) == null)
                return (row1 as object) == (row2 as object);
            return row1.Equals(row2);
        }

        public static bool operator != (RowVector row1,RowVector row2)
        {
            return !(row1 == row2);
        }

        public static bool operator >(RowVector row1,RowVector row2)
        {
            if (row1.Count != row2.Count)
                return false;
            for (int i = 0; i < row2.Count; i++)
                if (row1[i] <= row2[i])
                    return false;
            return true;
        }

        public static bool operator <(RowVector row1, RowVector row2)
        {
            if (row1.Count != row2.Count)
                return false;
            for (int i = 0; i < row2.Count; i++)
                if (row1[i] >= row2[i])
                    return false;
            return true;
        }

        public static bool operator >= (RowVector row1, RowVector row2)
        {
            if (row1.Count != row2.Count)
                return false;
            for (int i = 0; i < row2.Count; i++)
                if (row1[i] < row2[i])
                    return false;
            return true;
        }

        public static bool operator <=(RowVector row1, RowVector row2)
        {
            if (row1.Count != row2.Count)
                return false;
            for (int i = 0; i < row2.Count; i++)
                if (row1[i] > row2[i])
                    return false;
            return true;
        }
    }
    public class ColumnVector
    {
        private int m_Count = 0;
        public int Count
        {
            get { return m_Count; }
        }

        private List<double> m_data = new List<double>();

        public double this[int index]
        {
            get
            {
                if (index >= m_data.Count || index < 0)
                    throw new NotSupportedException("Index of column vector is over boundary.");
                return m_data[index];
            }
            set
            {
                if (index >= m_data.Count || index < 0)
                    throw new NotSupportedException("Index of column vector is over boundary.");
                m_data[index] = value;
            }
        }

        public ColumnVector()
        {
        }

        public ColumnVector(List<int> data)
        {
            foreach (int value in data)
                m_data.Add((double)value);
            this.m_Count = data.Count;
        }

        public ColumnVector(List<float> data)
        {
            foreach (int value in data)
                m_data.Add((double)value);
            this.m_Count = data.Count;
        }

        public ColumnVector(List<double> data)
        {
            this.m_data = data.Where(t => true).ToList();
            this.m_Count = data.Count;
        }

        public ColumnVector(ColumnVector vector)
        {
            this.m_data = vector.m_data.Where(t => true).ToList();
            this.m_Count = vector.m_Count;
        }

        public ColumnVector(string str)
        {
            if (str.Length == 0)
                return;
            if (!str.Contains("'"))
                throw new NotSupportedException("Inputted string is a row vector not column column.");
            str = str.Replace("[", "");
            str = str.Replace("]'", "");
            List<string> strList = str.Split(' ').ToList();
            foreach (string s in strList)
            {
                string temp = s.Trim();
                if (temp.Length == 0)
                    continue;
                m_data.Add(Convert.ToDouble(temp));
                m_Count++;
            }
        }

        public override string ToString()
        {
            string retStr = "[";
            foreach (double cell in m_data)
            {
                retStr += cell.ToString() + ' ';
            }
            if (retStr.Length > 0)
                retStr.Remove(retStr.Length - 1, 1);
            retStr += "]'";
            return retStr;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ColumnVector))
                throw new NotSupportedException("Can not compare because their type is not same.");
            if (obj == null)
                return (this as object) == null;
            ColumnVector other = obj as ColumnVector;
            if (this.Count != other.Count)
                return false;
            else
            {
                for (int i = 0; i < this.Count; i++)
                    if (this[i] != other[i])
                        return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public Matrix Transform2Matrix()
        {
            return new Matrix(new List<ColumnVector> { this });
        }

        public RowVector GetTransposeColumnVector()
        {
            return new RowVector(this.m_data);
        }

        //Insert data to the end of vector.
        public void Append(double data)
        {
            this.m_data.Add(data);
            this.m_Count++;
        }

        public void Insert(double data, int index)
        {
            if (index >= this.Count)
                throw new NotSupportedException("Index is over boundary when insert data to column vector.");
            this.m_data.Insert(index, data);
            this.m_Count++;
        }

        public void Remove(int index)
        {
            if (index >= this.Count)
                throw new NotSupportedException("Index is over boundary when remove data to column vector.");
            this.m_data.RemoveAt(index);
            this.m_Count--;
        }

        public double MultiplyLeftWithRowVector(RowVector row)
        {
            return row * this;
        }

        public ColumnVector MultiplyLeftWithMatrix(Matrix matrix)
        {
            if (this.Count == 0)
                throw new NotSupportedException("The count of column vector is zero when column vector is multiplied with matrix.");
            if (matrix.ColumnsCount == 0)
                throw new NotSupportedException("The column count of matrix is zero when column vector is multiplied with matrix.");
            if (this.Count != matrix.ColumnsCount)
                throw new NotSupportedException("The count of column vector is not equal to matrix's column count when row vector is multiplied with matrix.");

            ColumnVector result = new ColumnVector();
            foreach (RowVector row in matrix.GetRowVectors())
            {
                result.Append(row * this);
            }
            return result;
        }

        public ColumnVector MultiplyWithNumber(decimal x)
        {
            ColumnVector newColumn = new ColumnVector(this);
            for (int i = 0; i < this.Count; i++)
                newColumn[i] = (double)Math.Round((decimal)newColumn[i],5,MidpointRounding.AwayFromZero);
            return newColumn;
        }

        public ColumnVector MultiplyWithNumber(double x)
        {
            return MultiplyWithNumber((decimal)x);
        }

        public ColumnVector Plus(ColumnVector row)
        {
            if (row.Count != this.m_Count)
                throw new NotSupportedException("Can not plus two column because their count is not equal.");
            ColumnVector newRow = new ColumnVector(this);
            for (int i = 0; i < this.Count; i++)
                newRow[i] += row[i];
            return newRow;
        }

        public ColumnVector Minus(ColumnVector row)
        {
            return this.Plus(-row);
        }

        public static ColumnVector operator *(ColumnVector column, decimal x)
        {
            return column.MultiplyWithNumber(x);
        }

        public static ColumnVector operator *(ColumnVector column, int x)
        {
            return column.MultiplyWithNumber((decimal)x);
        }

        public static ColumnVector operator *(ColumnVector column, float x)
        {
            return column.MultiplyWithNumber(x);
        }

        public static ColumnVector operator*(ColumnVector column,double x)
        {
            return column.MultiplyWithNumber(x);
        }

        public static ColumnVector operator *(double x,ColumnVector column)
        {
            return column * x;
        }

        public static ColumnVector operator +(ColumnVector column1, ColumnVector column2)
        {
            return column1.Plus(column2);
        }

        public static ColumnVector operator -(ColumnVector column1)
        {
            ColumnVector newRow = new ColumnVector(column1);
            for (int i = 0; i < newRow.Count; i++)
            {
                newRow[i] = 0 - newRow[i];
            }
            return newRow;
        }

        public static ColumnVector operator -(ColumnVector column1, ColumnVector column2)
        {
            return column1.Minus(column2);
        }

        public static bool operator ==(ColumnVector column1, ColumnVector column2)
        {
            if ((column1 as object) == null || (column2 as object) == null)
                return (column1 as object) == (column2 as object);
            return column1.Equals(column2);
        }

        public static bool operator !=(ColumnVector column1, ColumnVector column2)
        {
            return !(column1 == column2);
        }

        public static bool operator >(ColumnVector column1, ColumnVector column2)
        {
            if (column1.Count != column2.Count)
                return false;
            for (int i = 0; i < column2.Count; i++)
                if (column1[i] <= column2[i])
                    return false;
            return true;
        }

        public static bool operator <(ColumnVector column1, ColumnVector column2)
        {
            if (column1.Count != column2.Count)
                return false;
            for (int i = 0; i < column2.Count; i++)
                if (column1[i] >= column2[i])
                    return false;
            return true;
        }

        public static bool operator >=(ColumnVector column1, ColumnVector column2)
        {
            if (column1.Count != column2.Count)
                return false;
            for (int i = 0; i < column2.Count; i++)
                if (column1[i] < column2[i])
                    return false;
            return true;
        }

        public static bool operator <=(ColumnVector column1, ColumnVector column2)
        {
            if (column1.Count != column2.Count)
                return false;
            for (int i = 0; i < column2.Count; i++)
                if (column2[i] > column2[i])
                    return false;
            return true;
        }
    }
    public class Matrix
    {
        #region Properties

        private List<RowVector> m_data = new List<RowVector>();

        private int m_RowsCount = 0;
        public int RowsCount
        {
            get { return m_RowsCount; }
        }

        private int m_ColumnsCount = 0;
        public int ColumnsCount
        {
            get { return m_ColumnsCount; }
        }

        public RowVector this[int rowIndex]
        {
            get
            {
                if (rowIndex >= 0 && rowIndex < this.RowsCount)
                    return this.m_data[rowIndex];
                else
                    throw new NotSupportedException("rowIndex is over boundary at matrix.");
            }
            set
            {
                if (rowIndex >= 0 && rowIndex < this.RowsCount)
                    this.m_data[rowIndex] = value;
                else
                    throw new NotSupportedException("rowIndex is over boundary at matrix.");
            }
        }

        public double this[int rowIndex, int columnIndex]
        {
            get
            {
                if (rowIndex >= 0 && rowIndex < this.RowsCount &&
                   columnIndex >= 0 && columnIndex < this.ColumnsCount)
                    return this.m_data[rowIndex][columnIndex];
                else
                    throw new NotSupportedException("[rowIndex,columnIndex] is over boundary at matrix.");
            }
            set
            {
                if (rowIndex >= 0 && rowIndex < this.RowsCount &&
                   columnIndex >= 0 && columnIndex < this.ColumnsCount)
                    this.m_data[rowIndex][columnIndex] = value;
                else
                    throw new NotSupportedException("[rowIndex,columnIndex] is over boundary at matrix.");
            }
        }

        private List<ColumnVector> m_Columns = new List<ColumnVector>();

        public List<ColumnVector> Columns
        {
            get
            {
                if(m_Columns.Count == 0)
                {
                    m_Columns = this.GetColumnVectors();
                }
                return m_Columns;
            }
        }

        public List<RowVector> Rows
        {
            get
            {
                return this.m_data;
            }
        }
        #endregion

        #region Constructor

        public Matrix()
        {

        }

        public Matrix(List<List<int>> data)
        {
            foreach (List<int> row in data)
                this.m_data.Add(new RowVector(row));
            this.m_RowsCount = data.Count;
            if (m_RowsCount != 0)
                m_ColumnsCount = m_data.First().Count;
        }

        public Matrix(List<List<float>> data)
        {
            foreach (List<float> row in data)
                this.m_data.Add(new RowVector(row));
            this.m_RowsCount = data.Count;
            if (m_RowsCount != 0)
                m_ColumnsCount = m_data.First().Count;
        }

        public Matrix(List<List<double>> data)
        {
            foreach (List<double> row in data)
                this.m_data.Add(new RowVector(row));
            this.m_RowsCount = data.Count;
            if (m_RowsCount != 0)
                m_ColumnsCount = m_data.First().Count;
        }

        public Matrix(Matrix matrix)
        {
            foreach (RowVector row in matrix.m_data)
                this.m_data.Add(new RowVector(row));
            m_RowsCount = m_data.Count;
            if (m_RowsCount != 0)
                m_ColumnsCount = m_data.First().Count;
        }

        public Matrix(List<RowVector> rows)
        {
            m_data.AddRange(rows.Where(t => true));
            this.m_RowsCount = rows.Count;
            if (m_RowsCount != 0)
                m_ColumnsCount = m_data.First().Count;
        }

        public Matrix(List<ColumnVector> columns)
        {
            for (int i = 0; i < columns[0].Count; i++)
            {
                this.m_data.Add(new RowVector(new List<double> { columns[0][i] }));
            }
            this.m_RowsCount = columns[0].Count;
            this.m_ColumnsCount = 1;
            for (int i = 1; i < columns.Count; i++)
                this.AppendColumn(columns[i]);
        }

        //Matrix string format: [1 0 0 ; 0 1 0 ; 0 0 1]
        public Matrix(string matrixStr)
        {
            if (!matrixStr.Contains("[") || !matrixStr.Contains("]"))
                throw new NotSupportedException("Input string is not match matrix format.");
            matrixStr = matrixStr.Replace(";", "];[");
            List<string> rowStrings = matrixStr.Split(';').ToList();
            List<RowVector> rows = new List<RowVector>();
            foreach(string rowStr in rowStrings)
            {
                rows.Add(new RowVector(rowStr));
            }
            this.m_data.Add(rows.First());
            this.m_RowsCount = 1;
            this.m_ColumnsCount = rows.First().Count;
            for (int i = 1; i < rows.Count; i++)
                this.AppendRow(rows[i]);
        }

        #endregion

        #region Caculate Functions

        public Matrix MultiplyLeftWithMatrix(Matrix leftMatrix)
        {
            List<RowVector> newRows = new List<RowVector>();
            foreach(RowVector row in leftMatrix.GetRowVectors())
            {
                newRows.Add(row * this);
            }
            return new Matrix(newRows);
        }

        public Matrix MultiplyRightWithMatrix(Matrix rightMatrix)
        {
            return rightMatrix.MultiplyLeftWithMatrix(this);
        }

        public RowVector MultiplyLeftWithRowVector(RowVector row)
        {
            if (row.Count != this.RowsCount)
                throw new NotSupportedException("Can not multiply RowVector * Matrix because Rowvector's count is not equal with Matrix's ColumnCount.");
            RowVector newRow = new RowVector();
            foreach(ColumnVector column in this.GetColumnVectors())
            {
                newRow.Append(row * column);
            }
            return newRow;
        }

        public ColumnVector MutliplyRightWithColumnVector(ColumnVector column)
        {
            ColumnVector newColumn = new ColumnVector();
            foreach(RowVector row in this.GetRowVectors())
            {
                newColumn.Append(row * column);
            }
            return newColumn;
        }

        public Matrix MultiWithNumber(decimal x)
        {
            List<RowVector> rows = new List<RowVector>();
            foreach (RowVector row in this.GetRowVectors())
                rows.Add(row * x);
            return new Matrix(rows);
        }

        public Matrix MultiWithNumber(double x)
        {
            return MultiWithNumber((decimal)x);
        }

        public Matrix Plus(Matrix matrix)
        {
            if (this.RowsCount != matrix.RowsCount ||
                this.ColumnsCount != matrix.ColumnsCount)
                throw new NotSupportedException("Can not plus two matrix bacause their degree is not equal.");
            List<RowVector> newRows = new List<RowVector>();
            for (int i = 0; i < this.RowsCount; i++)
                newRows.Add(this[i] + matrix[i]);
            return new Matrix(newRows);
        }

        public Matrix Minus(Matrix matrix)
        {
            return this + (-matrix);
        }

        public Matrix Power(int times)
        {
            if (!isSquareMatrix())
                throw new NotSupportedException("Can not caculate matrix's power since it is not a square matrix.");
            if (times == 0)
            {
                return GetIndentityMatrix(this.RowsCount);
            }
            else if (times > 0)
            {
                Matrix multiMatrix = new Matrix(this);
                for(int i = 1; i < times; i++)
                {
                    multiMatrix = multiMatrix * this;
                }
                return multiMatrix;
            }
            else if (times == -1)
            {
                return this.GetInverseMatrix();
            }
            else
            {
                return this.GetInverseMatrix().Power(0 - times);
            }
        }

        public Matrix GetInverseMatrix()
        {
            List<RowVector> newRows = new List<RowVector>();
            for(int i = 0; i < this.RowsCount; i++)
            {
                RowVector row = new RowVector();
                for (int j = 0; j < this.ColumnsCount; j++)
                    row.Append(this.GetAlgeraicComplement(i, j));
                newRows.Add(row);
            }
            Matrix newMatrix = new Matrix(newRows);
            double module = this.GetModule();
            decimal temp = (decimal)1.0 / (decimal)module;
            newMatrix = newMatrix * temp;
            return newMatrix.GetTransposeMatrix();
        }

        public Matrix Multiply(Matrix matrix)
        {
            return this * matrix;
        }

        public ColumnVector Multiply(ColumnVector column)
        {
            return this * column;
        }

        public Matrix Divide(Matrix matrix)
        {
            throw new NotImplementedException();
        }

        public double GetModule()
        {
            if (!isSquareMatrix())
                throw new NotSupportedException("Can not get module because it is not square matrix.");
            if (this.RowsCount == 1)
                return this[0][0];
            double sum = 0;
            for(int i = 0; i < this.ColumnsCount; i++)
            {
                if (this[0][i] == 0)
                    continue;
                sum += this[0][i] * this.GetAlgeraicComplement(0, i);
            }
            return sum;
        }

        public int CaculateRank()
        {
            double[][] matrix = new double[this.RowsCount][];
            for(int i = 0; i < this.RowsCount; i++)
            {
                matrix[i] = new double[this.ColumnsCount];
                for (int j = 0; i < this.ColumnsCount; j++)
                    matrix[i][j] = this[i, j];
            }
            return GetRank(matrix);
        }

        public bool isSquareMatrix()
        {
            return this.m_RowsCount == this.m_ColumnsCount;
        }

        #endregion

        #region Overrided Functions
        public override string ToString()
        {
            string retStr = "[ ";
            foreach(RowVector row in this.m_data)
            {
                string curRowStr = row.ToString();
                curRowStr = curRowStr.Replace("[","");
                curRowStr = curRowStr.Replace("]","");
                retStr += curRowStr + ";";
            }
            retStr = retStr.Remove(retStr.Length - 1, 1);
            retStr += "]";
            return retStr;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix))
                throw new NotSupportedException("Can not compare because their type is not same.");
            if (obj == null)
                return (this as object) == null;
            Matrix other = obj as Matrix;
            if (this.RowsCount != other.RowsCount || this.ColumnsCount != other.ColumnsCount)
                return false;
            else
            {
                for (int i = 0; i < this.RowsCount; i++)
                    if (this[i] != other[i])
                        return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Overloaded operator functions

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.Plus(matrix2);
        }

        public static Matrix operator-(Matrix matrix1)
        {
            List<RowVector> newRows = new List<RowVector>();
            foreach (RowVector row in matrix1.GetRowVectors())
                newRows.Add(-row);
            return new Matrix(newRows);
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            return matrix1 + (-matrix2);
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.MultiplyRightWithMatrix(matrix2);
        }

        public static RowVector operator *(RowVector row, Matrix matrix2)
        {
            return matrix2.MultiplyLeftWithRowVector(row);
        }

        public static ColumnVector operator *(Matrix matrix1, ColumnVector column)
        {
            return matrix1.MutliplyRightWithColumnVector(column);
        }

        public static Matrix operator *(Matrix matrix,double x)
        {
            return matrix.MultiWithNumber(x);
        }

        public static Matrix operator *(Matrix matrix, decimal x)
        {
            return matrix.MultiWithNumber(x);
        }

        public static Matrix operator *(decimal x, Matrix matrix)
        {
            return matrix.MultiWithNumber(x);
        }

        public static Matrix operator *(double x,Matrix matrix)
        {
            return matrix * x;
        }

        public static Matrix operator /(Matrix matrix1, Matrix matrix2)
        {
            return matrix1.Divide(matrix2);
        }

        public static bool operator ==(Matrix matrix1, Matrix matrix2)
        {
            if ((matrix1 as object) == null || (matrix2 as object) == null)
                return (matrix1 as object) == (matrix2 as object);
            return matrix1.Equals(matrix2);
        }

        public static bool operator !=(Matrix matrix1, Matrix matrix2)
        {
            return !(matrix1 == matrix2);
        }

        public static Matrix operator ^(Matrix matrix1, int times)
        {
            return matrix1.Power(times);
        }

        #endregion

        #region Matrix Operate Functions

        public void AppendRow(RowVector row)
        {
            if (row.Count != this.ColumnsCount)
                throw new NotSupportedException("The count of row which is appeded to matrix is not equal to the count of column of matrix.");
            this.m_data.Add(row);
            this.m_RowsCount++;
            m_Columns.Clear();
        }

        public void InsertRow(RowVector row, int index)
        {
            if (row.Count != this.ColumnsCount)
                throw new NotSupportedException("The count of row which is appeded to matrix is not equal to the count of column of matrix.");
            if (index < 0 || index >= this.RowsCount)
                throw new NotSupportedException("The row insertion position is beyond the boundary of the matrix.");
            this.m_data.Insert(index, row);
            this.m_RowsCount++;
            m_Columns.Clear();
        }

        public void AppendRows(List<RowVector> rows)
        {
            foreach (RowVector row in rows)
                this.AppendRow(row);
        }

        public void InsertRows(List<RowVector> rows,int index)
        {
            for (int i = 0; i < rows.Count; i++)
                this.InsertRow(rows[i], index + i);
        }

        public void AppendColumn(ColumnVector column)
        {
            if(column.Count != this.RowsCount)
                throw new NotSupportedException("The count of column which is appeded to matrix is not equal to the count of row of matrix.");
            for (int i = 0; i < this.RowsCount; i++)
                this[i].Append(column[i]);
            this.m_ColumnsCount++;
            m_Columns.Clear();
        }

        public void InsertColumn(ColumnVector column, int index)
        {
            if (column.Count != this.RowsCount)
                throw new NotSupportedException("The count of column which is appeded to matrix is not equal to the count of row of matrix.");
            if(index >= this.m_ColumnsCount || index < 0)
                throw new NotSupportedException("The column insertion position is beyond the matrix boundary.");
            for (int i = 0; i < this.RowsCount; i++)
                this[i].Insert(column[i], index);
            this.m_ColumnsCount++;
            m_Columns.Clear();
        }

        public void AppendColumns(List<ColumnVector> columns)
        {
            foreach (ColumnVector column in columns)
                this.AppendColumn(column);
        }

        public void InsertColumns(List<ColumnVector> columns, int index)
        {
            for (int i = 0; i < columns.Count; i++)
                this.InsertColumn(columns[i], index + i);
        }

        public void RemoveRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= this.RowsCount)
                throw new NotSupportedException("The row deletion position exceeds the matrix boundary.");
            this.m_data.RemoveAt(rowIndex);
            this.m_RowsCount--;
            m_Columns.Clear();
        }

        public void RemoveRows(int rowIndex,int count)
        {
            if (count > this.RowsCount - rowIndex + 1)
                throw new NotSupportedException("The total number of rows deleted exceeds the total number of remaining rows.");
            for (int i = 0; i < count; i++)
                this.RemoveRow(rowIndex);
        }

        public void RemoveColumn(int columnIndex)
        {
            if (columnIndex < 0 || columnIndex >= this.ColumnsCount)
                throw new NotSupportedException("The column deletion position exceeds the matrix boundary.");
            foreach(RowVector row in this.m_data)
                row.Remove(columnIndex);
            this.m_ColumnsCount--;
            m_Columns.Clear();
        }

        public void RemoveColumns(int columnIndex, int count)
        {
            if (count > this.ColumnsCount - columnIndex + 1)
                throw new NotSupportedException("The total number of columns deleted exceeds the total number of remaining columns.");
            for (int i = 0; i < count; i++)
                this.RemoveRow(columnIndex);
        }

        public Matrix GetTransposeMatrix()
        {
            List<ColumnVector> columns = new List<ColumnVector>();
            foreach(RowVector row in m_data)
            {
                columns.Add(row.GetTransposeColumnVector());
            }
            return new Matrix(columns);
        }

        public List<ColumnVector> GetColumnVectors()
        {
            List<ColumnVector> columns = new List<ColumnVector>();
            Matrix transposeMatrix = this.GetTransposeMatrix();
            foreach (RowVector row in transposeMatrix.GetRowVectors())
                columns.Add(row.GetTransposeColumnVector());
            return columns;
        }

        public List<RowVector> GetRowVectors()
        {
            return m_data;
        }

        public double GetAlgeraicComplement(int rowIndex,int columnIndex)
        {
            return Math.Pow(-1, rowIndex + columnIndex) *
                this.GetComplementMatrix(rowIndex, columnIndex).GetModule();
        }

        public Matrix GetComplementMatrix(int rowIndex,int columnIndex)
        {
            if (rowIndex < 0 || rowIndex > this.RowsCount - 1 ||
                columnIndex < 0 || columnIndex > this.ColumnsCount - 1)
                throw new NotSupportedException("Can not get complement matrix because index is over boundary.");
            Matrix matrix = new Matrix(this);
            matrix.RemoveRow(rowIndex);
            matrix.RemoveColumn(columnIndex);
            return matrix;
        }

        #endregion

        #region Static Functions


        public static Matrix GetIndentityMatrix(int degree)
        {
            if (degree <= 0)
                throw new NotSupportedException("Degree can not be less than zero when get an indentity matrix.");
            List<RowVector> rows = new List<RowVector>();
            List<double> temp = new List<double>();
            for (int i = 0; i < degree;i++)
                temp.Add(0.0);
            for(int i = 0; i < degree; i++)
            {
                List<double> row = new List<double>(temp);
                row[i] = 1;
                rows.Add(new RowVector(row));
            }
            return new Matrix(rows);
        }

        #endregion

        #region Caculate Rank Functions

        /// <summary>
        /// 计算矩阵的秩
        /// </summary>
        /// <param name="matrix">矩阵</param>
        /// <returns></returns>
        private static int GetRank(double[][] matrix)
        {
            //matrix为空则直接默认已经是最简形式
            if (matrix == null || matrix.Length == 0) return 0;
            //复制一个matrix到copy，之后因计算需要改动矩阵时并不改动matrix本身
            double[][] copy = new double[matrix.Length][];
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = new double[matrix[i].Length];
            }
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    copy[i][j] = matrix[i][j];
                }
            }
            //先以最左侧非零项的位置进行行排序
            Operation1(copy);
            //循环化简矩阵
            while (!isFinished(copy))
            {
                Operation2(copy);
                Operation1(copy);
            }
            //过于趋近0的项，视作0，减小误差
            Operation3(copy);
            //行最简矩阵的秩即为所求
            return Operation4(matrix);
        }
        /// <summary>
        /// 判断矩阵是否变换到最简形式（非零行数达到最少）
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>true:</returns>
        private static bool isFinished(double[][] matrix)
        {
            //统计每行第一个非零元素的出现位置
            int[] counter = new int[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        counter[i]++;
                    }
                    else break;
                }
            }
            //后面行的非零元素出现位置必须在前面行的后面，全零行除外
            for (int i = 1; i < counter.Length; i++)
            {
                if (counter[i] <= counter[i - 1] && counter[i] != matrix[0].Length)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 排序（按左侧最前非零位位置自上而下升序排列）
        /// </summary>
        /// <param name="matrix">矩阵</param>
        private static void Operation1(double[][] matrix)
        {
            //统计每行第一个非零元素的出现位置
            int[] counter = new int[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        counter[i]++;
                    }
                    else break;
                }
            }
            //按每行非零元素的出现位置升序排列
            for (int i = 0; i < counter.Length; i++)
            {
                for (int j = i; j < counter.Length; j++)
                {
                    if (counter[i] > counter[j])
                    {
                        double[] dTemp = matrix[i];
                        matrix[i] = matrix[j];
                        matrix[j] = dTemp;
                    }
                }
            }
        }
        /// <summary>
        /// 行初等变换（左侧最前非零位位置最靠前的行，只保留一个）
        /// </summary>
        /// <param name="matrix">矩阵</param>
        private static void Operation2(double[][] matrix)
        {
            //统计每行第一个非零元素的出现位置
            int[] counter = new int[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        counter[i]++;
                    }
                    else break;
                }
            }
            for (int i = 1; i < counter.Length; i++)
            {
                if (counter[i] == counter[i - 1] && counter[i] != matrix[0].Length)
                {
                    double a = matrix[i - 1][counter[i - 1]];
                    double b = matrix[i][counter[i]]; //counter[i]==counter[i-1]
                    matrix[i][counter[i]] = 0;
                    for (int j = counter[i] + 1; j < matrix[i].Length; j++)
                    {
                        double c = matrix[i - 1][j];
                        matrix[i][j] -= (c * b / a);
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 将和0非常接近的数字视为0
        /// </summary>
        /// <param name="matrix"></param>
        private static void Operation3(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    if (Math.Abs(matrix[i][j]) <= 0.00001)
                    {
                        matrix[i][j] = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 计算行最简矩阵的秩
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        private static int Operation4(double[][] matrix)
        {
            int rank = -1;
            bool isAllZero = true;
            for (int i = 0; i < matrix.Length; i++)
            {
                isAllZero = true;
                //查看当前行有没有0
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    if (matrix[i][j] != 0)
                    {
                        isAllZero = false;
                        break;
                    }
                }
                //若第i行全为0，则矩阵的秩为i
                if (isAllZero)
                {
                    rank = i;
                    break;
                }
            }
            //满秩矩阵的情况
            if (rank == -1)
            {
                rank = matrix.Length;
            }
            return rank;
        }

        #endregion
    }
}
