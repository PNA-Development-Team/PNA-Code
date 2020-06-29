using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Utility;

namespace MathematicalTool.PetriNetTTuple
{
    using MarkingName = System.Int32;
    using ArcWeight = System.Int32;
    using PlaceTokenCount = System.Int32;
    using PlaceName = System.Int32;
    using TransitionName = System.Int32;

    public class Place
    {
        private PlaceName m_PlaceName = 0;
        private PlaceTokenCount m_InitialTokenNum = 0;
        private SortedDictionary<TransitionName, ArcWeight> m_PreTransitions = new SortedDictionary<TransitionName, ArcWeight>();
        private SortedDictionary<TransitionName, ArcWeight> m_PostTransitions = new SortedDictionary<TransitionName, ArcWeight>();

        public SortedDictionary<TransitionName, ArcWeight> PreTransitions { get => m_PreTransitions; set => m_PreTransitions = value; }
        public SortedDictionary<TransitionName, ArcWeight> PostTransitions { get => m_PostTransitions; set => m_PostTransitions = value; }
        public PlaceTokenCount InitialTokenNum { get => m_InitialTokenNum; set => m_InitialTokenNum = value; }
        public PlaceName PlaceName { get => m_PlaceName; set => m_PlaceName = value; }

        public Place()
        {

        }

        public Place(PlaceName placeName,PlaceTokenCount initialTokenNum)
        {
            this.PlaceName = placeName;
            this.InitialTokenNum = initialTokenNum;
        }

        public Place(PlaceName placeName, PlaceTokenCount initialTokenNum, 
                     KeyValuePair<TransitionName, ArcWeight> preTransition, 
                     KeyValuePair<TransitionName, ArcWeight> postTransition)
        {
            this.PlaceName = placeName;
            this.InitialTokenNum = initialTokenNum;
            if (preTransition.Key != 0 && preTransition.Value != 0)
                this.m_PreTransitions.Add(preTransition.Key, preTransition.Value);
            if (postTransition.Key != 0 && postTransition.Value != 0)
                this.m_PostTransitions.Add(postTransition.Key, postTransition.Value);
        }

        public Place(string s)
        {
            List<string> strList = Utility.StringHelper.GetFrontNumAndLastString(s);
            if (strList.Count != 2)
                return;

            this.PlaceName = Convert.ToInt32(strList[0].Trim());
            strList = Utility.StringHelper.GetFrontNumAndLastString(strList[1]);
            this.InitialTokenNum = Convert.ToInt32(strList[0].Trim());
            strList = strList[1].Split(',').ToList();
            if (strList.Count != 2)
                return;

            List<string> transitions = strList[0].Split(' ').ToList();
            foreach (string t in transitions)
            {
                if (t == "")
                    continue;
                List<string> t_w = t.Split(':').ToList();
                if (t_w.Count == 2)
                {
                    this.m_PreTransitions.Add(Convert.ToInt32(t_w[0].Trim()), Convert.ToInt32(t_w[1].Trim()));
                }
                else
                    this.m_PreTransitions.Add(Convert.ToInt32(t_w[0].Trim()), 1);
            }
            transitions = strList[1].Split(' ').ToList();
            foreach (string t in transitions)
            {
                if (t == "")
                    continue;
                List<string> t_w = t.Split(':').ToList();
                if (t_w.Count == 2)
                {
                    this.m_PostTransitions.Add(Convert.ToInt32(t_w[0].Trim()), Convert.ToInt32(t_w[1].Trim()));
                }
                else
                    this.m_PostTransitions.Add(Convert.ToInt32(t_w[0].Trim()), 1);
            }

        }

        public void AddPreTransition(TransitionName preTransitionName,ArcWeight weight)
        {
            AddPreTransition(new KeyValuePair<TransitionName, ArcWeight>(preTransitionName, weight));
        }

        public void AddPostTransition(TransitionName postTransitionName, ArcWeight weight)
        {
            AddPostTransition(new KeyValuePair<TransitionName, ArcWeight>(postTransitionName, weight));
        }

        public void AddPreTransition(KeyValuePair<TransitionName, ArcWeight> preTransition)
        {
            this.m_PostTransitions.Add(preTransition.Key,preTransition.Value);
        }

        public void AddPostTransition(KeyValuePair<TransitionName, ArcWeight> postTransition)
        {
            this.m_PostTransitions.Add(postTransition.Key, postTransition.Value);
        }

        public void RemovePreTransition(TransitionName preTransitionName)
        {
            if(this.m_PreTransitions.ContainsKey(preTransitionName))
                this.m_PreTransitions.Remove(preTransitionName);
        }

        public void RemovePostTransition(TransitionName postTransitionName)
        {
            if (this.m_PostTransitions.ContainsKey(postTransitionName))
                this.m_PostTransitions.Remove(postTransitionName);
        }

        public override string ToString()
        {
            string returnStr = string.Empty;
            if (this.PlaceName == 0 ||
                this.InitialTokenNum < 0 ||
                (this.m_PreTransitions.Count == 0 && this.m_PostTransitions.Count == 0))
                return returnStr;

            returnStr += this.PlaceName;
            returnStr += "        " + this.InitialTokenNum.ToString() + "        ";
            foreach (KeyValuePair<TransitionName, ArcWeight> item in this.m_PreTransitions)
            {
                returnStr += item.Key;
                if (item.Value > 1)
                    returnStr += ":" + item.Value.ToString();
                returnStr += " ";
            }
            returnStr += ",        ";
            foreach (KeyValuePair<TransitionName, ArcWeight> item in this.m_PostTransitions)
            {
                returnStr += item.Key;
                if (item.Value > 1)
                    returnStr += ":" + item.Value.ToString();
                returnStr += " ";
            }

            return returnStr;
        }
    }

    public class Transition
    {
        private TransitionName m_TransitionName = 0;
        private SortedDictionary<PlaceName, ArcWeight> m_PrePlaces = new SortedDictionary<PlaceName, ArcWeight>();
        private SortedDictionary<PlaceName, ArcWeight> m_PostPlaces = new SortedDictionary<PlaceName, ArcWeight>();

        public TransitionName TransitionName { get => m_TransitionName; set => m_TransitionName = value; }
        public SortedDictionary<PlaceName, ArcWeight> PrePlaces { get => m_PrePlaces; set => m_PrePlaces = value; }
        public SortedDictionary<PlaceName, ArcWeight> PostPlaces { get => m_PostPlaces; set => m_PostPlaces = value; }

        public Transition()
        {

        }

        public Transition(TransitionName transitionName)
        {
            m_TransitionName = transitionName;
        }

        public Transition(TransitionName transitionName, 
            KeyValuePair<PlaceName, ArcWeight> prePlace, 
            KeyValuePair<PlaceName, ArcWeight> postPlace)
        {
            this.TransitionName = transitionName;
            if (prePlace.Key != 0 && prePlace.Value != 0)
                this.PrePlaces.Add(prePlace.Key, prePlace.Value);
            if (postPlace.Key != 0 && postPlace.Value != 0)
                this.PostPlaces.Add(postPlace.Key, postPlace.Value);
        }

        public void AddPrePlace(KeyValuePair<PlaceName, ArcWeight> prePlace)
        {
            this.m_PrePlaces.Add(prePlace.Key,prePlace.Value);
        }

        public void AddPostPlace(KeyValuePair<PlaceName, ArcWeight> postPlace)
        {
            this.m_PostPlaces.Add(postPlace.Key, postPlace.Value);
        }

        public void RemovePrePlace(PlaceName prePlaceName)
        {
            if (this.m_PrePlaces.ContainsKey(prePlaceName))
                this.m_PrePlaces.Remove(prePlaceName);
        }

        public void RemovePostPlace(PlaceName postPlaceName)
        {
            if (this.m_PostPlaces.ContainsKey(postPlaceName))
                this.m_PostPlaces.Remove(postPlaceName);
        }
    }

    public class IncidenceMatrix : Matrix
    {
        private List<PlaceName> m_PlaceNames = new List<PlaceName>();
        private List<TransitionName> m_TransitionNames = new List<TransitionName>();
        private Dictionary<PlaceName, RowVector> m_PlaceDictionary = new Dictionary<TransitionName, RowVector>();
        private Dictionary<TransitionName, ColumnVector> m_TransitionDictionary = new Dictionary<TransitionName, ColumnVector>();

        public List<PlaceName> PlaceNames
        {
            get => m_PlaceNames;
        }
        public List<TransitionName> TransitionNames
        {
            get => m_TransitionNames;
        }
        public Dictionary<PlaceName, RowVector> PlaceRows
        {
            get { return m_PlaceDictionary; }
        }

        public Dictionary<TransitionName, ColumnVector> TransitionColumns
        {
            get { return m_TransitionDictionary; }
        }

        public IncidenceMatrix()
        {

        }

        public IncidenceMatrix(List<PlaceName> placeNames, List<TransitionName> transitionNames, List<List<int>> data)
            : base(data)
        {
            SetDictionary(placeNames, transitionNames);
        }

        public IncidenceMatrix(List<PlaceName> placeNames, List<TransitionName> transitionNames, Matrix matrix)
            : base(matrix)
        {
            SetDictionary(placeNames, transitionNames);
        }

        public IncidenceMatrix(List<PlaceName> placeNames, List<TransitionName> transitionNames, List<RowVector> rows)
            : base(rows)
        {
            SetDictionary(placeNames, transitionNames);
        }

        public IncidenceMatrix(List<PlaceName> placeNames, List<TransitionName> transitionNames, List<ColumnVector> columns)
            : base(columns)
        {
            SetDictionary(placeNames, transitionNames);
        }

        //Matrix string format: [1 0 0 ; 0 1 0 ; 0 0 1]
        public IncidenceMatrix(List<PlaceName> placeNames, List<TransitionName> transitionNames, string matrixStr)
            : base(matrixStr)
        {
            SetDictionary(placeNames, transitionNames);
        }

        private void SetDictionary(List<PlaceName> placeNames, List<TransitionName> transitionNames)
        {
            if (placeNames.Count != this.RowsCount)
                throw new Exception("Number of name of places is not equal to matrix's row count.");
            if (transitionNames.Count != this.ColumnsCount)
                throw new Exception("Number of name of transitions is not equal to matrix's column count.");

            this.m_PlaceNames = placeNames;
            this.m_TransitionNames = transitionNames;
            for (int i = 0; i < this.RowsCount; i++)
            {
                this.m_PlaceDictionary.Add(placeNames[i], this[i]);
            }

            for (int i = 0; i < this.ColumnsCount; i++)
            {
                this.m_TransitionDictionary.Add(transitionNames[i], this.Columns[i]);
            }
        }

        public Matrix GetMatrix()
        {
            return this;
        }

        public int GetValueFromMarkingNameAndTransitionName(MarkingName markingName,TransitionName transitionName)
        {
            int rowIndex = this.m_PlaceNames.FindIndex(m => m == markingName);
            int columnIndex = this.m_TransitionNames.FindIndex(t => t == transitionName);
            return (int)this[rowIndex, columnIndex];

        }

        #region Overrided Operator Functions

        public static IncidenceMatrix operator +(IncidenceMatrix matrix1, IncidenceMatrix matrix2)
        {
            return new IncidenceMatrix(matrix1.PlaceRows.Keys.ToList(),
                                       matrix1.TransitionColumns.Keys.ToList(),
                                       matrix1.GetMatrix() + matrix2.GetMatrix());
        }

        public static IncidenceMatrix operator -(IncidenceMatrix matrix1)
        {
            return new IncidenceMatrix(matrix1.PlaceRows.Keys.ToList(),
                                       matrix1.TransitionColumns.Keys.ToList(),
                                       -matrix1.GetMatrix());
        }

        public static IncidenceMatrix operator -(IncidenceMatrix matrix1, IncidenceMatrix matrix2)
        {
            return matrix1 + (-matrix2);
        }

        #endregion
    }

    public class Marking : ColumnVector
    {
        private MarkingName m_MarkingName = -1;
        private List<TransitionName> m_EnabledTransitions = new List<TransitionName>();
        private IncidenceMatrix m_PreIncidenceMatrix = new IncidenceMatrix();
        private IncidenceMatrix m_PostIncidenceMatrix = new IncidenceMatrix();
        private IncidenceMatrix m_IncidenceMatrix = new IncidenceMatrix();

        public MarkingName MarkingName{
            get => m_MarkingName;
            set => m_MarkingName = value;
        }
        public List<TransitionName> EnabledTransition
        {
            get=> m_EnabledTransitions;
        }

        public IncidenceMatrix PreIncidenceMatrix
        {
            get => this.m_PreIncidenceMatrix;
        }

        public IncidenceMatrix PostIncidenceMatrix
        {
            get => this.m_PostIncidenceMatrix;
        }

        public IncidenceMatrix IncidenceMatrix
        {
            get => this.m_IncidenceMatrix;
        }

        public Marking()
        {

        }

        public Marking(MarkingName markingName,ColumnVector data,IncidenceMatrix incidenceMatrix)
            : base(data)
        {
            this.m_MarkingName = markingName;
            this.m_IncidenceMatrix = incidenceMatrix;
            foreach(TransitionName transitionName in this.m_IncidenceMatrix.TransitionNames)
            {
                if (this >= -this.m_IncidenceMatrix.TransitionColumns[transitionName])
                    this.m_EnabledTransitions.Add(transitionName);
            }
        }

        public Marking(MarkingName markingName, ColumnVector data, IncidenceMatrix preIncidenceMatrix, IncidenceMatrix postIncidence)
            : base(data)
        {
            this.m_MarkingName = markingName;
            this.m_PreIncidenceMatrix = preIncidenceMatrix;
            this.m_PostIncidenceMatrix = postIncidence;
            this.m_IncidenceMatrix = this.m_PostIncidenceMatrix - this.m_PreIncidenceMatrix;

            foreach (TransitionName transitionName in this.m_IncidenceMatrix.TransitionNames)
            {
                if (this >= this.m_PreIncidenceMatrix.TransitionColumns[transitionName])
                    this.m_EnabledTransitions.Add(transitionName);
            }
        }

        public Marking(Marking m) : base(m)
        {
            this.m_MarkingName = m.MarkingName;
            this.m_PreIncidenceMatrix = m.m_PreIncidenceMatrix;
            this.m_PostIncidenceMatrix = m.m_PostIncidenceMatrix;
            this.m_IncidenceMatrix = m.IncidenceMatrix;
            this.m_EnabledTransitions = m.EnabledTransition;
        }

        public Marking FireTransitionSequence(List<Transition> transitionSequence)
        {
            Marking m = new Marking(this);
            foreach (Transition t in transitionSequence)
            {
                m = m.FireTransition(t.TransitionName);
            }
            return m;
        }

        public Marking FireTransitionSequence(List<TransitionName> transitionSequence)
        {
            Marking m = new Marking(this);
            foreach(TransitionName t in transitionSequence)
            {
                m = m.FireTransition(t);
            }
            return m;
        }

        public Marking FireTransition(Transition t)
        {
            return FireTransition(t.TransitionName);
        }

        public Marking FireTransition(TransitionName t)
        {
            if (!CanFireTransition(t))
                throw new NotSupportedException("M"+this.m_MarkingName+" can not fire t"+t);
            if (this.m_PostIncidenceMatrix.RowsCount == 0 || this.m_PreIncidenceMatrix.RowsCount == 0)
                return new Marking(this.m_MarkingName + 1, this + this.IncidenceMatrix.TransitionColumns[t], this.IncidenceMatrix);
            else
                return new Marking(this.m_MarkingName + 1, this + this.IncidenceMatrix.TransitionColumns[t], this.m_PreIncidenceMatrix, this.m_PostIncidenceMatrix);
        }

        public bool CanFireTransition(TransitionName t)
        {
            if (this.m_EnabledTransitions.Contains(t))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int GetMarkingValueFromPlaceName(PlaceName placeName)
        {
            if (!this.m_IncidenceMatrix.PlaceNames.Contains(placeName))
            {
                throw new NotSupportedException("Can not find p" + placeName + " at M" + this.MarkingName + ".");
            }
            int index = this.m_IncidenceMatrix.PlaceNames.IndexOf(placeName);
            return (int)this[index];
        }
    }

    public class Reachability
    {
        #region parameters

        private Marking m_M0 = new Marking();

        public Marking M0 { get => m_M0; }

        public IncidenceMatrix IncidenceMatrix
        {
            get

            {
                if (this.m_M0.MarkingName == -1)
                    return null;
                return this.m_M0.IncidenceMatrix;
            }
        }

        public List<PlaceName> PlaceNames
        {
            get => this.IncidenceMatrix.PlaceNames;
        }

        public List<TransitionName> TransitionNames
        {
            get => this.IncidenceMatrix.TransitionNames;
        }

        private Dictionary<MarkingName,Marking> m_MarkingNameMapMarking = new Dictionary<MarkingName, Marking>();

        public Dictionary<MarkingName, Marking> Markings
        {
            get => m_MarkingNameMapMarking;
        }

        private Dictionary<Marking, MarkingName> m_MarkingMapMarkingName = new Dictionary<Marking, MarkingName>();

        private Dictionary<MarkingName, Dictionary<TransitionName, MarkingName>> m_Graph =
                        new Dictionary<MarkingName, Dictionary<TransitionName, MarkingName>>();

        public Dictionary<MarkingName, Dictionary<TransitionName,MarkingName>> Graph
        {
            get => m_Graph;
        }

        private Dictionary<TransitionName, HashSet<MarkingName>> m_TransitionMapMarkings = new Dictionary<MarkingName, HashSet<MarkingName>>();

        public Dictionary<TransitionName,HashSet<MarkingName>> TransitionMapMarkings
        {
            get => m_TransitionMapMarkings;
        }

        private HashSet<MarkingName> m_LegalMarkings = new HashSet<MarkingName>();

        public HashSet<MarkingName> LegalMarkings
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_BadMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_LegalMarkings;
            }
        }

        private HashSet<MarkingName> m_IllegalMarkings = new HashSet<MarkingName>();

        public HashSet<MarkingName> IllegalMarkings
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_IllegalMarkings;
            }
        }

        private HashSet<MarkingName> m_GoodMarkings = new HashSet<MarkingName>();

        public HashSet<MarkingName> GoodMarkings
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_GoodMarkings;
            }
        }

        private HashSet<MarkingName> m_DangerousMarkings = new HashSet<MarkingName>();

        public HashSet<MarkingName> DangerousMarkings
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_DangerousMarkings;
            }
        }

        private HashSet<MarkingName> m_BadMarkings = new HashSet<MarkingName>();

        public HashSet<MarkingName> BadMarkings
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_BadMarkings;
            }
        }

        private HashSet<MarkingName> m_DeadMarkings = new HashSet<MarkingName>();

        public HashSet<MarkingName> DeadMarkings
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_DeadMarkings;
            }
        }

        private Dictionary<MarkingName, HashSet<TransitionName>> m_MTSIs = new Dictionary<MarkingName, HashSet<TransitionName>>();

        public Dictionary<MarkingName, HashSet<TransitionName>> MTSIs
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_MTSIs;
            }
        }

        private HashSet<MarkingName> m_FBMs = new HashSet<MarkingName>();

        public HashSet<MarkingName> FBMs
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_FBMs;
            }
        }

        private HashSet<TransitionName> m_GoodTransitions = new HashSet<TransitionName>();

        public HashSet<TransitionName> GoodTransitions
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_GoodTransitions;
            }
        }

        private HashSet<TransitionName> m_CriticalTransitions = new HashSet<TransitionName>();

        public HashSet<TransitionName> CriticalTransitions
        {
            get
            {
                if (m_LegalMarkings.Count == 0 || m_IllegalMarkings.Count == 0)
                    AnalyzeMarkingsState();
                return this.m_CriticalTransitions;
            }
        }

        private int m_Count = 0;

        public int Count { get => m_Count; }

        private HashSet<MarkingName> m_CaculatedMarkings = new HashSet<MarkingName>();

        #endregion

        public Reachability()
        {

        }

        public Reachability(Marking M0)
        {
            this.m_M0 = M0;
            this.m_MarkingNameMapMarking.Add(M0.MarkingName,M0);
            this.m_MarkingMapMarkingName.Add(M0, M0.MarkingName);
            CaculateReachability();
        }

        private void CaculateReachability()
        {
            while(this.m_CaculatedMarkings.Count < this.m_MarkingNameMapMarking.Count)
            {
                foreach(KeyValuePair<MarkingName,Marking> item in this.m_MarkingNameMapMarking)
                {
                    if (m_CaculatedMarkings.Contains(item.Key))
                        continue;
                    else
                    {
                        FireAllTransitionsFromMarking(item.Value);
                        break;
                    }        
                }
            }
            this.m_Count = this.m_MarkingNameMapMarking.Count;
        }

        private void FireAllTransitionsFromMarking(Marking m)
        {
            if (!this.m_Graph.ContainsKey(m.MarkingName))
                this.m_Graph.Add(m.MarkingName, new Dictionary<TransitionName, MarkingName>());

            foreach(TransitionName t in m.EnabledTransition)
            {
                if (!this.m_TransitionMapMarkings.ContainsKey(t))
                {
                    this.m_TransitionMapMarkings.Add(t, new HashSet<MarkingName>());
                }
                Marking nextM = m.FireTransition(t);
                if (!this.m_MarkingMapMarkingName.ContainsKey(nextM))
                {
                    nextM.MarkingName = ++this.m_Count;
                    this.m_MarkingNameMapMarking.Add(nextM.MarkingName,nextM);
                    this.m_MarkingMapMarkingName.Add(nextM, nextM.MarkingName);
                    this.m_Graph[m.MarkingName].Add(t, nextM.MarkingName);
                    this.m_TransitionMapMarkings[t].Add(nextM.MarkingName);
                }
                else
                {
                    MarkingName name = this.m_MarkingMapMarkingName[nextM];
                    this.m_Graph[m.MarkingName].Add(t,name);
                    this.m_TransitionMapMarkings[t].Add(name);
                }
            }
            this.m_CaculatedMarkings.Add(m.MarkingName);
        }

        public void Export2GraFile()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "gra files (*.gra)|*.gra| All files (*.*)|*.*";
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

            string PlacesStr = "P.nr:";
            foreach(PlaceName p in this.m_M0.IncidenceMatrix.PlaceNames)
                PlacesStr += " " + p; 
            
            foreach(KeyValuePair<MarkingName, Dictionary<TransitionName, MarkingName>> item in this.m_Graph)
            {
                sw.WriteLine("State nr.\t" + item.Key.ToString());
                sw.WriteLine(PlacesStr);
                string str = this.m_MarkingNameMapMarking[item.Key].ToString();
                str = str.Remove(str.Length - 2, 2).Remove(0, 1);
                sw.WriteLine("toks: " + str);
                foreach(KeyValuePair<TransitionName, MarkingName> subItem in item.Value)
                {
                    sw.WriteLine("==t" + subItem.Key.ToString() + "=>s" + subItem.Value.ToString());
                }
            }

            sw.Close();
            fs.Close();
        }

        private void AnalyzeMarkingsState()
        {
            Marking co_marking = new Marking(this.m_M0.MarkingName, this.m_M0, -this.m_M0.IncidenceMatrix);
            Reachability co_Reachability = new Reachability(co_marking);

            foreach(KeyValuePair<Marking,MarkingName> item in this.m_MarkingMapMarkingName)
            {
                if (co_Reachability.m_MarkingMapMarkingName.ContainsKey(item.Key))
                {
                    this.m_LegalMarkings.Add(item.Value);
                    bool check = true;
                    foreach(KeyValuePair<TransitionName,MarkingName> subItem in this.m_Graph[item.Value])
                    {
                        if (!co_Reachability.m_MarkingMapMarkingName.ContainsKey(this.m_MarkingNameMapMarking[subItem.Value]))
                        {
                            if(check)   check = false;
                            this.m_CriticalTransitions.Add(subItem.Key);
                            if (!this.m_MTSIs.ContainsKey(item.Value))
                                this.m_MTSIs.Add(item.Value, new HashSet<MarkingName>());
                            this.m_MTSIs[item.Value].Add(subItem.Key);
                            this.m_FBMs.Add(subItem.Value);
                        }  
                    }
                    if (check)
                        this.m_GoodMarkings.Add(item.Value);
                    else
                        this.m_DangerousMarkings.Add(item.Value);
                }
                else
                {
                    this.m_IllegalMarkings.Add(item.Value);
                    if (this.m_Graph[item.Value].Count == 0)
                        this.m_DeadMarkings.Add(item.Value);
                    else
                        this.m_BadMarkings.Add(item.Value);
                }   
            }

            foreach(TransitionName t in this.M0.IncidenceMatrix.TransitionNames)
            {
                if (!this.m_CriticalTransitions.Contains(t))
                    this.m_GoodTransitions.Add(t);
            }
        }

        public Marking GetMarkingFromMarkingName(MarkingName markingName)
        {
            if (!this.m_MarkingNameMapMarking.ContainsKey(markingName))
            {
                return null;
            }
            return this.m_MarkingNameMapMarking[markingName];
        }

        public HashSet<MarkingName> GetTEnabledMarkingsFromTransitionName(TransitionName transitionName)
        {
            if (!this.m_TransitionMapMarkings.ContainsKey(transitionName))
            {
                return null;
            }
            return this.m_TransitionMapMarkings[transitionName];
        }

        public List<PlaceName> GetOperationPlaceNames()
        {
            List<PlaceName> operationPlaceNames = new List<MarkingName>();
            foreach(PlaceName placeName in this.M0.IncidenceMatrix.PlaceNames)
            {
                if (this.M0.GetMarkingValueFromPlaceName(placeName) == 0)
                    operationPlaceNames.Add(placeName);
            }
            return operationPlaceNames;
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

            if (this.m_LegalMarkings.Count == 0 || this.m_IllegalMarkings.Count == 0)
                AnalyzeMarkingsState();

            sw.WriteLine("The total num of state : " + this.Count.ToString());
            sw.WriteLine("The total num of legal state : " + this.m_LegalMarkings.Count);
            sw.WriteLine("The total num of illlegal state : " + this.m_IllegalMarkings.Count);
            sw.WriteLine("The total num of good state : " + this.m_GoodMarkings.Count);
            sw.WriteLine("The total num of dangerous state : " + this.m_DangerousMarkings.Count);
            sw.WriteLine("The total num of bad state : " + this.m_BadMarkings.Count);
            sw.WriteLine("The total num of dead state : " + this.m_DeadMarkings.Count);
            sw.WriteLine("The total num of FBM state : " + this.m_FBMs.Count);
            sw.WriteLine();

            WriteDateToStateFile(ref sw, ref this.m_LegalMarkings, "legal");
            WriteDateToStateFile(ref sw, ref this.m_IllegalMarkings, "illegal");
            WriteDateToStateFile(ref sw, ref this.m_GoodMarkings, "good");
            WriteDateToStateFile(ref sw, ref this.m_DangerousMarkings, "dangerous");
            WriteDateToStateFile(ref sw, ref this.m_BadMarkings, "bad");
            WriteDateToStateFile(ref sw, ref this.m_DeadMarkings, "dead");
            WriteDateToStateFile(ref sw, ref this.m_FBMs, "FBM");

            int count = 1;
            foreach (KeyValuePair<MarkingName,HashSet<TransitionName>> item in this.m_MTSIs)
            {
                foreach(TransitionName t in item.Value)
                {
                    sw.WriteLine("MTSI state " + count.ToString() + ": M = " + item.Key + "; t = " + t);
                    count++;
                }   
            }
            sw.WriteLine("The total num of MTSI state : " + (--count).ToString());
            sw.WriteLine();

            sw.Close();
            fs.Close();
        }

        private void WriteDateToStateFile(ref StreamWriter sw,ref HashSet<MarkingName> set,string name)
        {
            int count = 1;
            foreach (MarkingName m in set)
            {
                sw.WriteLine(name + " state " + count.ToString() + ": " + m);
                count++;
            }
            sw.WriteLine("The total num of " + name + " state : " + set.Count);
            sw.WriteLine();
        }
    }
}
