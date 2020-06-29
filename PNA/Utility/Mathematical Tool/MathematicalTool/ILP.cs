using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using LpSolveDotNet;

namespace MathematicalTool
{
    public enum LimitedConditionSign { GreaterOrEqual = 0, Greater, Equal, LessOrEqual, Less };

    public struct ILPDataFormat
    {
        public List<string> Variables;
        public List<int> ObjectFunction;
        public bool IsMax;
        public List<List<int>> LeftMatrix;
        public List<int> LeftConstValues;
        public List<LimitedConditionSign> LimitedConditionSigns;
        public List<List<int>> RightMatrix;
        public List<int> RightConstValue;
        public List<bool> Gin;
        public List<bool> Bin;
        public List<string> Annotations;

        public int VariableNum;
        public int InequalityNum;

        public ILPDataFormat(List<string> variables,
                            List<int> objectFunction,
                            bool isMax,
                            List<List<int>> leftMatrix,
                            List<int> leftConstValues,
                            List<LimitedConditionSign> limitedConditionSigns,
                            List<List<int>> rightMatrix,
                            List<int> rightConstValue,
                            List<bool> gin,
                            List<bool> bin,
                            List<string> annotations)
        {
            if (leftMatrix.Count == 0 || rightMatrix.Count == 0)
                throw new NotSupportedException("ILPDataFormat error : num of inequality can not be zero.");
            if (leftMatrix.Count != rightMatrix.Count)
                throw new NotSupportedException("ILPDataFormat error : left matrix count is not euqla to right matrix.");
            int inequalityNum = leftMatrix.Count;
            if (inequalityNum != limitedConditionSigns.Count)
                throw new NotSupportedException("ILPDataFormat error : num of inequality is not equal to num of limited condition sign.");
            if (inequalityNum != leftConstValues.Count || inequalityNum != rightConstValue.Count)
                throw new NotSupportedException("ILPDataFormat error : num of inequality is not equal to num of limited condition const value.");

            string retStr = string.Empty;
            int variableNum = variables.Count;
            if (variableNum == 0)
                throw new NotSupportedException("ILPDataFormat error : num of variable ca not be zero.");
            if (variableNum != objectFunction.Count ||
                variableNum != leftMatrix.First().Count ||
                variableNum != rightMatrix.First().Count ||
                variableNum != gin.Count ||
                variableNum != bin.Count)
                throw new NotSupportedException("ILPDataFormat error : num of variables is not equal to others' parameters.");

            this.InequalityNum = inequalityNum;
            this.VariableNum = variableNum;
            this.Variables = variables;
            this.ObjectFunction = objectFunction;
            this.IsMax = isMax;
            this.LeftMatrix = leftMatrix;
            this.LeftConstValues = leftConstValues;
            this.LimitedConditionSigns = limitedConditionSigns;
            this.RightMatrix = rightMatrix;
            this.RightConstValue = rightConstValue;
            this.Gin = gin;
            this.Bin = bin;
            this.Annotations = annotations;
        }

        public string GetAnnotation(int index)
        {
            if (index >= this.Annotations.Count || this.Annotations[index] == string.Empty)
                return string.Empty;
            return "\t!  " + this.Annotations[index] + ";";
        }
    }

    public class ILP
    {
        private string m_filePath = string.Empty;
        private List<string> m_variate = new List<string>();
        private Dictionary<string, double> m_result = new Dictionary<string, double>();
        public bool Load(string filePath)
        {
            throw new NotImplementedException();
        }

        public bool Caculate()
        {
            throw new NotImplementedException();
        } 

        public Dictionary<string,double> GetResult()
        {
            throw new NotImplementedException();
        }

        public static string CreateILPFileString(ref ILPDataFormat data)
        {
            string retStr = string.Empty;
            int annotationIndex = 0;
            retStr += "model:"+ data.GetAnnotation(annotationIndex++) +"\n";

            string objStr = string.Empty;
            if (data.IsMax)
                objStr += "max = ";
            else
                objStr += "min = ";
            bool isFrontExistNum = false;
            for (int i = 0; i < data.VariableNum; i++)
                StringHelper(ref objStr, data.ObjectFunction[i], data.Variables[i],ref isFrontExistNum);
            retStr += objStr + data.GetAnnotation(annotationIndex++) + ";\n";

            for (int i = 0; i < data.InequalityNum; i++)
            {
                string inequalityStr = string.Empty;
                isFrontExistNum = false;
                for (int j = 0; j < data.VariableNum; j++)
                    StringHelper(ref inequalityStr, data.LeftMatrix[i][j], data.Variables[j],ref isFrontExistNum);
                StringHelper(ref inequalityStr, data.LeftConstValues[i], string.Empty, ref isFrontExistNum);
                
                switch (data.LimitedConditionSigns[i])
                {
                    case LimitedConditionSign.GreaterOrEqual: inequalityStr += " >= "; break;
                    case LimitedConditionSign.Greater: inequalityStr += " > "; break;
                    case LimitedConditionSign.Equal: inequalityStr += " = "; break;
                    case LimitedConditionSign.LessOrEqual: inequalityStr += " <= "; break;
                    case LimitedConditionSign.Less: inequalityStr += " < "; break;
                }

                isFrontExistNum = false;
                for (int j = 0; j < data.VariableNum; j++)
                    StringHelper(ref inequalityStr, data.RightMatrix[i][j], data.Variables[j], ref isFrontExistNum);
                StringHelper(ref inequalityStr, data.RightConstValue[i], string.Empty, ref isFrontExistNum);
                inequalityStr += ";" + data.GetAnnotation(annotationIndex++) + "\n";
                retStr += inequalityStr ;
            }

            for (int i = 0; i < data.VariableNum; i++)
            {
                if (data.Gin[i] == false)
                    continue;
                retStr += "@gin( " + data.Variables[i] + " );" + data.GetAnnotation(annotationIndex++) + "\n";
            }
            for (int i = 0; i < data.VariableNum; i++)
            {
                if (data.Bin[i] == false)
                    continue;
                retStr += "@bin( " + data.Variables[i] + " );" + data.GetAnnotation(annotationIndex++) + "\n";
            }
            retStr += "end" + data.GetAnnotation(annotationIndex++) + "\n";

            return retStr;
        }

        public const int ILPMaxNum = 100000;

        private static void StringHelper(ref string str,int num,string symbol,ref bool isFrontExistNum)
        {
            if (num == 0)
                return;
            if(!isFrontExistNum)
            {
                isFrontExistNum = true;
                if (symbol == string.Empty)
                    str += num.ToString();
                else
                {
                    if (num == 1)
                        str += symbol;
                    else if (num == -1)
                        str += " - " + symbol;
                    else
                        str += num.ToString() + " * " + symbol;
                }
            }
            else
            {
                if (symbol == string.Empty)
                {
                    if (num > 0)
                        str += " + " + num.ToString();
                    else
                        str += num.ToString();
                }
                else
                {
                    if (num == 1)
                        str += " + " + symbol;
                    else if (num == -1)
                        str += " - " + symbol;
                    else if (num > 0)
                        str += " + " + num.ToString() + " * " + symbol;
                    else
                        str += num.ToString() + " * " + symbol;
                }
            }
        }
    }
}
