using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Information_Retrieval_System.Properties;

namespace Information_Retrieval_System.Preprocessing
{
    class DocProcesser
    {
        public static List<List<string>> GetDocuments() //returns a list of string arrays that will be each document stemmed and have stop word removed
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
            string[] docSeperator = { ".I" };
            //split document string into separate strings for each document
            string[] splitDocuments = result.Split(docSeperator, StringSplitOptions.RemoveEmptyEntries);

            //save each document to list
            List<string[]> initialDocumentsList = new List<string[]>();

            string[] docDelimiters = { ".W", "\r", "\n", " ", ".", ",", "?", "!", "-", "/", "'", "(", ")" };
            foreach(string doc in splitDocuments)
            {
                string[] d = doc.Split(docDelimiters, StringSplitOptions.RemoveEmptyEntries);

                initialDocumentsList.Add(d);
            }

            //take stop words and stem all documents
            List<List<string>> finalDocumentsList = new List<List<string>>();
            foreach (string[] dt in initialDocumentsList)
            {
                string[] removedStopWords = StopWords.RemoveStopWords(dt);
                string[] finalDoc = WordStemmer.QueryStemmer(removedStopWords);
                //remove doc number from beginning of document content
                List<string> temp = new List<string>(finalDoc);
                temp.RemoveAt(0);
                finalDocumentsList.Add(temp);

            }
            //return processed list of documents
            return finalDocumentsList;
        }
    }
}
