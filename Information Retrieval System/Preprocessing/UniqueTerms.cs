using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Information_Retrieval_System.Preprocessing
{
    class UniqueTerms
    {
        public static List<List<string>> getDocUniqueTerms(List<List<string>> input)
        {
            List<List<string>> uniqueDocTerms = new List<List<string>>();


            foreach (List<string> sa in input)
            {
                List<string> newsa = sa.Distinct().ToList();
                uniqueDocTerms.Add(newsa);
            }

            return uniqueDocTerms;

        }

        public static List<string> getAllUniqueTerms(List<List<string>> input)
        {
            List<string> allUniqueTerms = new List<string>();
            foreach(List<string> sl in input)
            {
                foreach(string s in sl)
                {
                    allUniqueTerms.Add(s);
                }
            }
            allUniqueTerms = allUniqueTerms.Distinct().ToList();
            return allUniqueTerms;
        }
    }
}
