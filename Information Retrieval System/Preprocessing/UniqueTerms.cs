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
        public static string[] getUniqueTerms(string[] query, List<string[]> documents)
        {
            //Save all documents to single string
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Information_Retrieval_System.Resources.MEDDocuments.txt";

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    //single string that contains all documents
                    result = reader.ReadToEnd();
                }
            }

            string[] docDelimiters = { ".W", "\r", "\n", " ", ".", ",", "?", "!", "-", "/", "'", "(", ")" };

            string[] d = result.Split(docDelimiters, StringSplitOptions.RemoveEmptyEntries);
            List<string> terms = d.ToList();
            terms.RemoveAt(0);
            terms.RemoveAt(0);
            d = terms.ToArray();

            string[] uniqueTerms = query.Union(d).ToArray();
            uniqueTerms = uniqueTerms.Distinct().ToArray();

            uniqueTerms = StopWords.RemoveStopWords(uniqueTerms);
            uniqueTerms = WordStemmer.QueryStemmer(uniqueTerms);

            return uniqueTerms;

        }
    }
}
