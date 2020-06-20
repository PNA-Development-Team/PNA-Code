using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IncidenceMatrix incidenceMatrix = GetIncidenceMatrixFromPlaceDictionary(data);
            List<int> initialMarkingData = new List<int>();
            foreach (Place p in data.Values.ToList())
            {
                initialMarkingData.Add(p.InitialTokenNum);
            }
            return new Marking(0, new ColumnVector(initialMarkingData), incidenceMatrix);
        }

        public static void ExportReabilityGraphFileFromPlaceDiationary(SortedDictionary<int, Place> data)
        {
            if (data.Count == 0)
                return;
            Marking M0 = GetInitialMarkingFromPlaceDictionary(data);
            ReabilityGraph gra = new ReabilityGraph(M0);
            gra.Export2GraFile();
        }

        #endregion

        #region Operator with Reability Graph

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reabilityGraph"></param>
        /// <returns>
        /// <para name="Dictionary<int,List<int>>"></para>
        /// <para Key="name of marking of MTSIs"></para>
        /// <para Value="list of transition which is enabled at Key's marking and will create a bad marking."></para>
        /// </returns>
        public static Dictionary<int,List<int>> GetMTSIs(ref ReabilityGraph reabilityGraph)
        {
            Dictionary<int, List<int>> MTSIs = new Dictionary<int, List<int>>();
            foreach (int markingName in reabilityGraph.LegalMarkings)
            {
                List<int> MTSI = new List<int>();
                foreach (KeyValuePair<int, int> item in reabilityGraph.Graph[markingName])
                {
                    if (reabilityGraph.BadMarkings.Contains(item.Value))
                        MTSI.Add(item.Key);
                }
                if (MTSI.Count != 0)
                    MTSIs.Add(markingName,MTSI);
            }
            return MTSIs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reabilityGraph"></param>
        /// <returns>
        /// <para name="HashSet<int>"></para>
        /// A hashset of name of first-met bad markings.
        /// </returns>
        public static HashSet<int> GetFBM(ref ReabilityGraph reabilityGraph)
        {
            HashSet<int> FBM = new HashSet<int>();
            foreach (int markingName in reabilityGraph.LegalMarkings)
            {
                bool check = false;
                foreach (KeyValuePair<int, int> item in reabilityGraph.Graph[markingName])
                {
                    if (reabilityGraph.BadMarkings.Contains(item.Value))
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                    FBM.Add(markingName);
            }
            GetMTSIs(ref reabilityGraph);
            return FBM;
        }

        public static HashSet<int> GetTransitions(ref ReabilityGraph reabilityGraph)
        {
            HashSet<int> transitions = new HashSet<int>();
            foreach(int t in reabilityGraph.TransitionMapMarkings.Keys.ToList())
            {
                transitions.Add(t);
            }
            return transitions;
        }

        public static HashSet<int> GetTCriticalTransitions(ref ReabilityGraph reabilityGraph)
        {
            Dictionary<int, List<int>> MTSIs = GetMTSIs(ref reabilityGraph);
            HashSet<int> TCriticalTransitions = new HashSet<int>();
            foreach(List<int> item in MTSIs.Values.ToList())
            {
                foreach (int t in item)
                    TCriticalTransitions.Add(t);
            }
            return TCriticalTransitions;
        }

        public static HashSet<int> GetTGoodTransitions(ref ReabilityGraph reabilityGraph)
        {
            HashSet<int> Transitions = GetTransitions(ref reabilityGraph);
            HashSet<int> TCriticalTransitions = GetTCriticalTransitions(ref reabilityGraph);
            HashSet<int> TGoodTransitions = new HashSet<int>(Transitions.Where(q => !TCriticalTransitions.Contains(q)).ToList());
            return TGoodTransitions;
        }

        #endregion
    }
}
