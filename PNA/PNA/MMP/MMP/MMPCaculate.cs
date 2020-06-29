using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathematicalTool.PetriNetOperator;
using MathematicalTool.PetriNetTTuple;

namespace MMP
{
    public class MMPCaculate
    {
        #region parameters

        protected Reachability m_Reachability = new Reachability();

        protected Dictionary<int, HashSet<int>> m_TCriticalMTSIs = new Dictionary<int, HashSet<int>>();

        protected Dictionary<int, HashSet<int>> m_TGoodMarkings = new Dictionary<int, HashSet<int>>();

        protected Dictionary<int, HashSet<int>> m_TDangerousMarkings = new Dictionary<int, HashSet<int>>();

        protected Dictionary<int, HashSet<int>> m_TEnabledGoodMarkings = new Dictionary<int, HashSet<int>>();

        protected Dictionary<int, HashSet<int>> m_TDisabledGoodMarkings = new Dictionary<int, HashSet<int>>();

        protected Dictionary<int, HashSet<int>> m_MinimalTCriticalMTSIs = new Dictionary<int, HashSet<int>>();

        protected HashSet<int> m_MinimalLegalMarkings = new HashSet<int>();

        protected HashSet<int> m_MinimalFBMs = new HashSet<int>();

        protected Dictionary<int, HashSet<int>> m_MinimalTEnabledGoodMarkings = new Dictionary<int, HashSet<int>>();

        protected Dictionary<int, List<int>> m_CriticalTransitionOperationPlaces = new Dictionary<int, List<int>>();

        protected Dictionary<int, HashSet<int>> m_MMinimalTEnabledGoodMarkings = new Dictionary<int, HashSet<int>>();

        #endregion

        #region Constructor

        public MMPCaculate(Reachability Reachability)
        {
            this.m_Reachability = Reachability;
            AnalyzeMarkingState();
            List<int> coverPlaceNames = this.m_Reachability.GetOperationPlaceNames();
            this.m_MinimalLegalMarkings = GetMinimalCoveringSet(this.m_Reachability.LegalMarkings, ref coverPlaceNames, true);
            this.m_MinimalFBMs = GetMinimalCoveringSet(this.m_Reachability.FBMs, ref coverPlaceNames, false);
            GetMinimalCoveringDictionary(ref this.m_MinimalTCriticalMTSIs, ref this.m_TCriticalMTSIs, ref coverPlaceNames, false);
            GetMinimalCoveringDictionary(ref this.m_MinimalTEnabledGoodMarkings, ref this.m_TEnabledGoodMarkings, ref coverPlaceNames, true);
            CaculateCriticalTransitionOperationPlaces();
            GetMMinimalTEnabledGoodMarkings();
        }

        private void AnalyzeMarkingState()
        {
            foreach(int legalMarking in this.m_Reachability.LegalMarkings)
            {
                Marking m = this.m_Reachability.GetMarkingFromMarkingName(legalMarking);
                foreach(int t in this.m_Reachability.TransitionNames)
                {
                    if (!this.m_TGoodMarkings.ContainsKey(t))
                        this.m_TGoodMarkings.Add(t, new HashSet<int>());
                    if (m.CanFireTransition(t))
                    {
                        int nextM = this.m_Reachability.Graph[legalMarking][t];
                        if (this.m_Reachability.LegalMarkings.Contains(nextM))
                        {
                            this.m_TGoodMarkings[t].Add(legalMarking);
                            if (!this.m_TEnabledGoodMarkings.ContainsKey(t))
                                this.m_TEnabledGoodMarkings.Add(t, new HashSet<int>());
                            this.m_TEnabledGoodMarkings[t].Add(legalMarking);
                        }
                        else
                        {
                            if (!this.m_TDangerousMarkings.ContainsKey(t))
                                this.m_TDangerousMarkings.Add(t, new HashSet<int>());
                            this.m_TDangerousMarkings[t].Add(legalMarking);

                            if (!this.m_TCriticalMTSIs.ContainsKey(t))
                                this.m_TCriticalMTSIs.Add(t, new HashSet<int>());
                            this.m_TCriticalMTSIs[t].Add(legalMarking);
                        }
                    }
                    else
                    {
                        this.m_TGoodMarkings[t].Add(legalMarking);
                        if (!this.m_TDisabledGoodMarkings.ContainsKey(t))
                            this.m_TDisabledGoodMarkings.Add(t, new HashSet<int>());
                        this.m_TDisabledGoodMarkings[t].Add(legalMarking);
                    }
                }
            }
        }

        private void GetMinimalCoveringDictionary(ref Dictionary<int,HashSet<int>> newDic,
                                                  ref Dictionary<int,HashSet<int>> oldDic,
                                                  ref List<int> coverPlaceNames,bool isGreat)
        {
            foreach (KeyValuePair<int, HashSet<int>> item in oldDic)
            {
                newDic.Add(item.Key, new HashSet<int>());
                newDic[item.Key] = GetMinimalCoveringSet(item.Value, ref coverPlaceNames, isGreat);
            }
        }

        private HashSet<int> GetMinimalCoveringSet(HashSet<int> oldMarkingSet,
                                                   ref List<int> coverPlaceNames,bool isGreat)
        {
            HashSet<int> newMarkingSet = new HashSet<int>();
            if (oldMarkingSet.Count == 0)
                return newMarkingSet;
            List<int> oldMarkingList = oldMarkingSet.ToList();
            List<int> newMarkingList = GetMinimalCoveringList(0, oldMarkingList.Count - 1, ref oldMarkingList, ref coverPlaceNames, isGreat);
            foreach (int m in newMarkingList)
                newMarkingSet.Add(m);
            return newMarkingSet;
        }

        private List<int> GetMinimalCoveringList(int left,int right,
                                                 ref List<int> markingList,
                                                 ref List<int> coverPlaceNames,bool isGreat)
        {
            List<int> minCoveringList = new List<int>();
            if(left + 1 == right)
            {
                int result = CompareTwoMarking(markingList[left], markingList[right],ref coverPlaceNames);
                if(result == 1)
                {
                    if (isGreat)
                        minCoveringList.Add(markingList[left]);
                    else
                        minCoveringList.Add(markingList[right]);
                }
                else if(result == -1)
                {
                    if (isGreat)
                        minCoveringList.Add(markingList[right]);
                    else
                        minCoveringList.Add(markingList[left]);
                }
                else
                {
                    minCoveringList.Add(markingList[left]);
                    minCoveringList.Add(markingList[right]);
                }
                return minCoveringList;
            }
            else if (left == right)
            {
                minCoveringList.Add(markingList[left]);
                return minCoveringList;
            }
 
            int mid = (left + right) / 2;
            List<int> leftList = GetMinimalCoveringList(left, mid, ref markingList, ref coverPlaceNames, isGreat);
            List<int> rightList = GetMinimalCoveringList(mid+1, right, ref markingList, ref coverPlaceNames, isGreat);
            bool[] leftCheck = new bool[leftList.Count];
            bool[] rightCheck = new bool[rightList.Count];
            for (int leftIndex = 0;leftIndex < leftList.Count;leftIndex++)
            {
                if (leftCheck[leftIndex])
                    continue;
                for(int rightIndex = 0; rightIndex < rightList.Count; rightIndex++)
                {
                    if (rightCheck[rightIndex])
                        continue;
                    int result = CompareTwoMarking(leftList[leftIndex], rightList[rightIndex], ref coverPlaceNames);
                    if (isGreat)
                    {
                        if (result == 1)
                            rightCheck[rightIndex] = true;
                        else if (result == -1)
                            leftCheck[leftIndex] = true;
                    }
                    else
                    {
                        if (result == 1)
                            leftCheck[leftIndex] = true;
                        else if (result == -1)
                            rightCheck[rightIndex] = true;
                    }
                }
            }
            for(int leftIndex = 0;leftIndex < leftList.Count; leftIndex++)
            {
                if (!leftCheck[leftIndex])
                    minCoveringList.Add(leftList[leftIndex]);
            }
            for(int rightIndex = 0;rightIndex < rightList.Count; rightIndex++)
            {
                if (!rightCheck[rightIndex])
                    minCoveringList.Add(rightList[rightIndex]);
            }
            return minCoveringList;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m1Name"></param>
        /// <param name="m2Name"></param>
        /// <param name="coverPlaceNames"></param>
        /// <returns>
        /// return 1: Marking 1 > Marking 2
        /// return 0: Can not compare Marking 1 with Marking 2
        /// return -1: Marking 1 < Marking 2
        /// </returns>
        private int CompareTwoMarking(int m1Name,int m2Name,ref List<int> coverPlaceNames)
        {
            bool isGreater = true;
            bool isLess = true;
            Marking m1 = this.m_Reachability.GetMarkingFromMarkingName(m1Name);
            Marking m2 = this.m_Reachability.GetMarkingFromMarkingName(m2Name);
            foreach (int p in coverPlaceNames)
            {
                int value1 = m1.GetMarkingValueFromPlaceName(p);
                int value2 = m2.GetMarkingValueFromPlaceName(p);
                if (value1 > value2)
                    isLess = false;
                else
                    isGreater = false;
            }
            if (isGreater)
                return 1;
            else if (isLess)
                return -1;
            else
                return 0;
        }

        public void ExportStateFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "state files (*.state)|*.state| All files (*.*)|*.*";
            DialogResult dialogResult = dialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            FileInfo fileInfo = new FileInfo(dialog.FileName);
            if (!Directory.Exists(fileInfo.DirectoryName))
                return;
            if (File.Exists(fileInfo.FullName))
                File.Delete(fileInfo.FullName);
            FileStream fs = new FileStream(fileInfo.FullName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            WriteDataToStateFile(ref sw, ref this.m_TGoodMarkings, "good marking");
            WriteDataToStateFile(ref sw, ref this.m_TEnabledGoodMarkings, "enabled good marking");
            WriteDataToStateFile(ref sw, ref this.m_TDisabledGoodMarkings, "disabled good marking");
            WriteDataToStateFile(ref sw, ref this.m_TDangerousMarkings, "dangerous marking");
            WriteMISIToStateFile(ref sw, ref this.m_TCriticalMTSIs, "critical marking");

            WriteDataToStateFile(ref sw, ref this.m_MinimalLegalMarkings, "legal marking*");
            WriteDataToStateFile(ref sw, ref this.m_MinimalFBMs, "FBM*");
            WriteDataToStateFile(ref sw, ref this.m_MinimalTEnabledGoodMarkings, "enabled good marking*");
            WriteDataToStateFile(ref sw, ref this.m_MMinimalTEnabledGoodMarkings, "enabled good marking**");
            WriteMISIToStateFile(ref sw, ref this.m_MinimalTCriticalMTSIs, "criticak marking*");

            sw.Close();
            fs.Close();

        }

        private void WriteDataToStateFile(ref StreamWriter sw, ref HashSet<int> set, string name)
        {
            int count = 1;
            foreach (int m in set)
            {
                sw.WriteLine(name + " " + count.ToString() + ": " + m);
                count++;
            }
            sw.WriteLine("The total num of " + name + " : " + set.Count);
            sw.WriteLine();
        }

        private void WriteDataToStateFile(ref StreamWriter sw, ref Dictionary<int,HashSet<int>> dic, string name)
        { 
            foreach (int t in dic.Keys)
            {
                int count = 1;
                foreach (int m in dic[t])
                {
                    sw.WriteLine("t" + t.ToString() + "-" + name + " " + count.ToString() + ": " + m);
                    count++;
                }
                sw.WriteLine("The total num of " + "t" + t.ToString() + "-" + name + " : " + dic[t].Count);
                sw.WriteLine();
            }      
        }

        private void WriteMISIToStateFile(ref StreamWriter sw,ref Dictionary<int,HashSet<int>> dic,string name)
        {
            foreach (int t in dic.Keys)
            {
                int count = 1;
                foreach (int m in dic[t])
                {
                    sw.WriteLine("t" + t.ToString() + "-" + name + " " + count.ToString() + ": (" + m.ToString() + "," + t.ToString() + ")");
                    count++;
                }
                sw.WriteLine("The total num of " + "t" + t.ToString() + "-" + name + " : " + dic[t].Count);
                sw.WriteLine();
            }
        }

        public MathematicalTool.ILPDataFormat GetMMPILPFormatData(int criticalTransitionName)
        {
            return GetMMPILPFormatData(criticalTransitionName,false);
        }

        public MathematicalTool.ILPDataFormat GetMMPILPFormatData(int criticalTransitionName,bool isMM)
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
            foreach (int m in operatorMarkings)
            {
                variables.Add("l" + m.ToString());
                objectFunction.Add(0);

                gin.Add(true);
                bin.Add(false);
            }
                
            foreach(int m in this.m_MinimalTCriticalMTSIs[criticalTransitionName])
            {
                variables.Add("f" + m.ToString());
                objectFunction.Add(1);

                gin.Add(false);
                bin.Add(true);
            }
                
            //Belta
            variables.Add("Belta");
            objectFunction.Add(0);
            gin.Add(true);
            bin.Add(false);

            //Omega
            variables.Add("Omega");
            objectFunction.Add(0);
            gin.Add(true);
            bin.Add(false);

            foreach(int markingName in this.m_MinimalLegalMarkings)
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

                //f_j
                foreach (int criticalMarking in this.m_MinimalTCriticalMTSIs[criticalTransitionName])
                {
                    leftRow.Add(0);
                    rightRow.Add(0);
                }

                //Belta
                leftRow.Add(0);
                rightRow.Add(1);

                //Omega
                leftRow.Add(0);
                rightRow.Add(0);

                leftMatrix.Add(leftRow);
                signs.Add(MathematicalTool.LimitedConditionSign.LessOrEqual);
                leftConstValues.Add(0);
                rightMatrix.Add(rightRow);
                rightConstValues.Add(0);
                annotation.Add("Legal marking* : " + markingName);
            }

            if (isMM)
            {
                foreach (int markingName in this.m_MMinimalTEnabledGoodMarkings[criticalTransitionName])
                {
                    Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                    List<int> leftRow = new List<int>();
                    List<int> rightRow = new List<int>();

                    //p_i
                    foreach (int operatorMarkingName in operatorMarkings)
                    {
                        int value = m.GetMarkingValueFromPlaceName(operatorMarkingName) +
                                    this.m_Reachability.IncidenceMatrix.GetValueFromMarkingNameAndTransitionName(operatorMarkingName, criticalTransitionName);
                        leftRow.Add(value);
                        rightRow.Add(0);
                    }

                    //f_j
                    foreach (int criticalMarking in this.m_MinimalTCriticalMTSIs[criticalTransitionName])
                    {
                        leftRow.Add(0);
                        rightRow.Add(0);
                    }

                    //Belta
                    leftRow.Add(0);
                    rightRow.Add(1);

                    //Omega
                    leftRow.Add(0);
                    rightRow.Add(-1);

                    leftMatrix.Add(leftRow);
                    signs.Add(MathematicalTool.LimitedConditionSign.LessOrEqual);
                    leftConstValues.Add(0);
                    rightMatrix.Add(rightRow);
                    rightConstValues.Add(0);
                    annotation.Add("t" + criticalTransitionName + "-enabled good marking** : " + markingName);
                }
            }
            else
            {
                foreach (int markingName in this.m_MinimalTEnabledGoodMarkings[criticalTransitionName])
                {
                    Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                    List<int> leftRow = new List<int>();
                    List<int> rightRow = new List<int>();

                    //p_i
                    foreach (int operatorMarkingName in operatorMarkings)
                    {
                        int value = m.GetMarkingValueFromPlaceName(operatorMarkingName) +
                                    this.m_Reachability.IncidenceMatrix.GetValueFromMarkingNameAndTransitionName(operatorMarkingName, criticalTransitionName);
                        leftRow.Add(value);
                        rightRow.Add(0);
                    }

                    //f_j
                    foreach (int criticalMarking in this.m_MinimalTCriticalMTSIs[criticalTransitionName])
                    {
                        leftRow.Add(0);
                        rightRow.Add(0);
                    }

                    //Belta
                    leftRow.Add(0);
                    rightRow.Add(1);

                    //Omega
                    leftRow.Add(0);
                    rightRow.Add(-1);

                    leftMatrix.Add(leftRow);
                    signs.Add(MathematicalTool.LimitedConditionSign.LessOrEqual);
                    leftConstValues.Add(0);
                    rightMatrix.Add(rightRow);
                    rightConstValues.Add(0);
                    annotation.Add("t" + criticalTransitionName + "-enabled good marking* : " + markingName);
                }
            }
            
            foreach(int markingName in this.m_MinimalTCriticalMTSIs[criticalTransitionName])
            {
                Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                List<int> leftRow = new List<int>();
                List<int> rightRow = new List<int>();

                //p_i
                foreach (int operatorMarkingName in operatorMarkings)
                {
                    int value = m.GetMarkingValueFromPlaceName(operatorMarkingName) +
                                this.m_Reachability.IncidenceMatrix.GetValueFromMarkingNameAndTransitionName(operatorMarkingName, criticalTransitionName);
                    leftRow.Add(value);
                    rightRow.Add(0);
                }

                //f_j
                foreach (int criticalMarking in this.m_MinimalTCriticalMTSIs[criticalTransitionName])
                {
                    leftRow.Add(0);
                    if (criticalMarking == markingName)
                        rightRow.Add(MathematicalTool.ILP.ILPMaxNum);
                    else
                        rightRow.Add(0);
                }

                //Belta
                leftRow.Add(0);
                rightRow.Add(1);

                //Omega
                leftRow.Add(0);
                rightRow.Add(-1);

                leftMatrix.Add(leftRow);
                rightMatrix.Add(rightRow);
                signs.Add(MathematicalTool.LimitedConditionSign.GreaterOrEqual);
                leftConstValues.Add(0);
                rightConstValues.Add(1 - MathematicalTool.ILP.ILPMaxNum);
                annotation.Add("MTSIs* : (" + markingName + " , t" + criticalTransitionName + ")");
            }

            return new MathematicalTool.ILPDataFormat(variables,objectFunction,true,leftMatrix,leftConstValues,
                                                        signs,rightMatrix,rightConstValues,gin,bin,annotation);
        }

        public void ExportMMPILPFile()
        {
            ExportMMPILPFile(false);
        }

        public void ExportMMPILPFile(bool isMM)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "ILP file (*.ilp)|*.ilp| All file (*.*)|*.*";
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            foreach(int criticalTransitionName in this.m_Reachability.CriticalTransitions)
            {
                string filePath = dialog.FileName;
                filePath = filePath.Remove(filePath.Length - 4, 4);
                filePath += "-t" + criticalTransitionName + ".ilp";
                FileStream fs = new FileStream(filePath,FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                MathematicalTool.ILPDataFormat data = GetMMPILPFormatData(criticalTransitionName,isMM);
                string str = MathematicalTool.ILP.CreateILPFileString(ref data);
                sw.Write(str);
                sw.Close();
                fs.Close();
            }
        }

        private void CaculateCriticalTransitionOperationPlaces()
        {
            List<int> operationPlaces = this.m_Reachability.GetOperationPlaceNames();
            foreach(KeyValuePair<int,HashSet<int>> item in this.m_MinimalTCriticalMTSIs)
            {
                this.m_CriticalTransitionOperationPlaces.Add(item.Key, new List<int>());
                foreach(int operationPlace in operationPlaces)
                {
                    if (this.m_Reachability.IncidenceMatrix.GetValueFromMarkingNameAndTransitionName(item.Key, operationPlace) > 0)
                    {
                        this.m_CriticalTransitionOperationPlaces[item.Key].Add(operationPlace);
                        continue;
                    }
                    bool check = true;
                    foreach(int markingName in item.Value)
                    {
                        Marking m = this.m_Reachability.GetMarkingFromMarkingName(markingName);
                        if (m.GetMarkingValueFromPlaceName(operationPlace) != 0)
                        {
                            check = false;
                            break;
                        }  
                    }
                    if (!check)
                        this.m_CriticalTransitionOperationPlaces[item.Key].Add(operationPlace);
                }
            }
        }

        private void GetMMinimalTEnabledGoodMarkings()
        {
            foreach(KeyValuePair<int,List<int>> item in this.m_CriticalTransitionOperationPlaces)
            {
                List<int> coverPlaces = item.Value;
                this.m_MMinimalTEnabledGoodMarkings.Add(item.Key, new HashSet<int>());
                this.m_MMinimalTEnabledGoodMarkings[item.Key] = GetMinimalCoveringSet(this.m_MinimalTEnabledGoodMarkings[item.Key],
                                      ref coverPlaces, true);
            }
        }

        #endregion
    }
}
