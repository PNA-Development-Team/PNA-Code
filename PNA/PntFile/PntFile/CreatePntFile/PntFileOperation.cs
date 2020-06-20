using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PntFile.Forms;
using MathematicalTool;
using MathematicalTool.PetriNetTTuple;
using MathematicalTool.PetriNetOperator;

namespace PntFile
{
    public static class PntFileOperation
    {
        public static SortedDictionary<int, Place> GetDataFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            SortedDictionary<int, Place> data = new SortedDictionary<int, Place>();
            FileStream fs = new FileStream(filePath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            sr.ReadLine();
            string str = sr.ReadLine();
            while(str != "@")
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

        public static void ExportReabilityGraphFileFromPntFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            SortedDictionary<int, Place> data = GetDataFromPntFile(filePath);
            PetriNetOperator.ExportReabilityGraphFileFromPlaceDiationary(data);
        }

    }
}
