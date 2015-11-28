using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Information_Retrieval_System.Properties;

namespace Information_Retrieval_System.RelevanceChecker
{
    class WeightingSchemeCalculation
    {
        // Store normalised TF for every term in keyValuePair list
        public static List<KeyValuePair<string, List<double>>> NormalisedTF(List<string[]> queryAndDocuments, string[] uniqueTerms)
        {
            //all normalised term frequencies for every term
            List<KeyValuePair<string, List<double>>> allTFValues = new List<KeyValuePair<string, List<double>>>();

            //store all term frequencies for given term
            List<List<double>> allTermFrequencies = new List<List<double>>();

            foreach (string term in uniqueTerms)
            {
                List<double> termFrequencies = new List<double>();

                //temp values for storing term frequency and normalised term frequency
                double tf = 0;
                double ntf = 0;
                foreach (string[] doc in queryAndDocuments)
                {
                    double docSize = (double)doc.Count();
                    foreach(string t in doc)
                    {
                        if(term.Equals(t))
                        {
                            tf++;
                        }
                    }

                    ntf = tf / docSize;

                    termFrequencies.Add(ntf);
                    tf = 0;
                    ntf = 0;
                    docSize = 0;
                }
                allTermFrequencies.Add(termFrequencies);
            }

            for(int i=0; i<uniqueTerms.Length;i++)
            {
                allTFValues.Add(new KeyValuePair<string, List<double>>(uniqueTerms[i], allTermFrequencies[i]));
            }

            return allTFValues;
        }

        public static List<KeyValuePair<string, double>> CalculateIDF(List<string[]> documents, string[] uniqueTerms)
        {
            double numDocs = documents.Count() -1;

            List<double> idfs = new List<double>();

            foreach(string term in uniqueTerms)
            {
                double appears = 0;
                foreach (string[] doc in documents)
                {
                    int pos = Array.IndexOf(doc, term);
                    if(pos > -1)
                    {
                        appears++;
                    }
                    
                }

                double idf = Math.Log10(numDocs / appears);
                idfs.Add(idf);
                appears = 0;
                idf = 0;
            }

            //Return List of KeyValuePairs

            List<KeyValuePair<string, double>> allIDFValues = new List<KeyValuePair<string, double>>();

            for (int i = 0; i < uniqueTerms.Length; i++)
            {
                allIDFValues.Add(new KeyValuePair<string, double>(uniqueTerms[i], idfs[i]));
            }

            return allIDFValues;
        }

        public static List<KeyValuePair<string, List<double>>> CalculateTfIdf (List<KeyValuePair<string, List<double>>> TFs, List<KeyValuePair<string, double>> IDFs, string[] uniqueTerms)
        {
            List<KeyValuePair<string, List<double>>> tfidfResults = new List<KeyValuePair<string, List<double>>>();

            //saving TFs
            List<double> termFreqs = new List<double>();
            List<List<double>> allTFs = new List<List<double>>();

            //saving IDFs
            double InverseDocFreqs;
            List<double> allIDFs = new List<double>();

            //saving TF-IDFs
            List<double> termTFIDFs = new List<double>();
            List<List<double>> allTFIDFs = new List<List<double>>();

            foreach(KeyValuePair<string, List<double>> tf in TFs)
            {
                termFreqs = tf.Value;
                allTFs.Add(termFreqs);
            }

            foreach(KeyValuePair<string, double> idf in IDFs)
            {
                InverseDocFreqs = idf.Value;
                allIDFs.Add(InverseDocFreqs);
            }

            foreach(double idf in allIDFs)
            {
                foreach(List<double> tfs in allTFs)
                {
                    foreach(double tf in tfs)
                    {
                        double tfidf = tf * idf;
                        termTFIDFs.Add(tfidf);
                        tfidf = 0;
                    }
                    allTFIDFs.Add(termTFIDFs);
                    termTFIDFs.Clear();
                }
            }

            for(int i = 0; i < uniqueTerms.Length; i++)
            {
                KeyValuePair<string, List<double>> kvp = new KeyValuePair<string, List<double>>(uniqueTerms[i], allTFIDFs.ElementAt(i));
                tfidfResults.Add(kvp);
            }
            

            return tfidfResults;
        }
    }
}
