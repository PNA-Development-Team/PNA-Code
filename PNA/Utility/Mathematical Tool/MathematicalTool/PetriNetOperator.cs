using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathematicalTool.PetriNetTTuple;

namespace MathematicalTool.PetriNetOperator
{
    public class PetriNetOperator
    {
        #region Operator with SortedDictionary<int, Place> data
        public static SortedSet<int> GetTransitionNamesFromPlaceDictionary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return null;
            SortedSet<int> transitionNames = new SortedSet<int>();
            foreach (KeyValuePair<int, Place> item in data)
            {
                foreach (int preTransitionName in item.Value.PreTransitions.Keys.ToList())
                    transitionNames.Add(preTransitionName);
                foreach (int postTransitionName in item.Value.PostTransitions.Keys.ToList())
                    transitionNames.Add(postTransitionName);
            }
            return transitionNames;
        }

        private static SortedDictionary<int, Place> FillWithOthersTransition(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return null;
            SortedSet<int> transitionNames = GetTransitionNamesFromPlaceDictionary(data);

            foreach (KeyValuePair<int, Place> item in data)
            {
                foreach (int transition in transitionNames)
                {
                    if (!item.Value.PreTransitions.ContainsKey(transition))
                        item.Value.PreTransitions.Add(transition, 0);
                    if (!item.Value.PostTransitions.ContainsKey(transition))
                        item.Value.PostTransitions.Add(transition, 0);
                }
            }

            return data;
        }

        public static IncidenceMatrix GetPreIncidenceMatrixFromPlaceDictionary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return null;
            SortedDictionary<int, Place> newData = new SortedDictionary<int, Place>(data);
            newData = FillWithOthersTransition(newData);

            List<List<int>> list = new List<List<int>>();
            foreach (KeyValuePair<int, Place> item in newData)
            {
                list.Add(item.Value.PostTransitions.Values.ToList());
            }
            IncidenceMatrix preIncidenceMatrix = new IncidenceMatrix(newData.Keys.ToList(),
                                                                     newData.First().Value.PreTransitions.Keys.ToList(),
                                                                     list);
            return preIncidenceMatrix;
        }

        public static IncidenceMatrix GetPostIncidenceMatrixFromPlaceDictionary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return null;
            SortedDictionary<int, Place> newData = new SortedDictionary<int, Place>(data);
            newData = FillWithOthersTransition(newData);

            List<List<int>> list = new List<List<int>>();
            foreach (KeyValuePair<int, Place> item in newData)
            {
                list.Add(item.Value.PreTransitions.Values.ToList());
            }
            IncidenceMatrix postIncidenceMatrix = new IncidenceMatrix(newData.Keys.ToList(),
                                                                     newData.First().Value.PreTransitions.Keys.ToList(),
                                                                     list);
            return postIncidenceMatrix;
        }

        public static IncidenceMatrix GetIncidenceMatrixFromPlaceDictionary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return null;
            IncidenceMatrix preIncidenceMatrix = GetPreIncidenceMatrixFromPlaceDictionary(data);
            IncidenceMatrix postIncidenceMatrix = GetPostIncidenceMatrixFromPlaceDictionary(data);
            return postIncidenceMatrix - preIncidenceMatrix;
        }

        public static Marking GetInitialMarkingFromPlaceDictionary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return null;
            IncidenceMatrix preIncidenceMatrix = GetPreIncidenceMatrixFromPlaceDictionary(data);
            IncidenceMatrix postIncidenceMatrix = GetPostIncidenceMatrixFromPlaceDictionary(data);
            List<int> initialMarkingData = new List<int>();
            foreach (Place p in data.Values.ToList())
            {
                initialMarkingData.Add(p.InitialTokenNum);
            }
            return new Marking(0, new ColumnVector(initialMarkingData), preIncidenceMatrix,postIncidenceMatrix);
        }

        public static void ExportReachabilityFileFromPlaceDiationary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return;
            Marking M0 = GetInitialMarkingFromPlaceDictionary(data);
            Reachability gra = new Reachability(M0);
            gra.Export2GraFile();
            gra.ExportStateFile();
        }

        #endregion

        #region Operator with pnt file

        public static SortedDictionary<int, Place> GetDataFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            SortedDictionary<int, Place> data = new SortedDictionary<int, Place>();
            FileStream fs = new FileStream(filePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            sr.ReadLine();
            string str = sr.ReadLine();
            while (str != "@")
            {
                Place p = new Place(str);
                data.Add(p.PlaceName, p);
                str = sr.ReadLine();
            }
            sr.Close();
            fs.Close();
            return data;
        }

        public static IncidenceMatrix GetPreIncidenceMatrixFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            SortedDictionary<int, Place> data = GetDataFromPntFile(filePath);
            return PetriNetOperator.GetPreIncidenceMatrixFromPlaceDictionary(data);
        }

        public static IncidenceMatrix GetPostIncidenceMatrixFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            SortedDictionary<int, Place> data = GetDataFromPntFile(filePath);
            return PetriNetOperator.GetPostIncidenceMatrixFromPlaceDictionary(data);
        }

        public static IncidenceMatrix GetIncidenceMatrixFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            SortedDictionary<int, Place> data = GetDataFromPntFile(filePath);
            return PetriNetOperator.GetIncidenceMatrixFromPlaceDictionary(data);
        }

        public static Marking GetInitialMarkingFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            SortedDictionary<int, Place> data = GetDataFromPntFile(filePath);
            return PetriNetOperator.GetInitialMarkingFromPlaceDictionary(data);
        }

        public static void ExportReachabilityFileFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            SortedDictionary<int, Place> data = GetDataFromPntFile(filePath);
            PetriNetOperator.ExportReachabilityFileFromPlaceDiationary(data);
        }

        #endregion
    }
}
