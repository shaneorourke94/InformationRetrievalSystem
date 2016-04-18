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
        // Store normalised TF for every term in Dictionary list
        public static Dictionary<string, Dictionary<string, double>> NormalisedTF(List<List<string>> queryAndDocuments, List<List<string>> uniqueDocTerms)
        {
            ////all normalised term frequencies for every term
            Dictionary<string, Dictionary<string, double>> allTFValues = new Dictionary<string, Dictionary<string, double>>();

            //store all term frequencies for given term
            List<List<double>> allTermFrequencies = new List<List<double>>();
            int docNumber = 0;

            //loop through each doc
            foreach (List<string> docTerms in uniqueDocTerms)
            {
                //variables for storing results
                Dictionary<string, double> termFrequencies = new Dictionary<string, double>();

                string docNum = "doc" + docNumber;
                docNumber++;
                //temp values for storing term frequency and normalised term frequency
                double tf = 0;
                double ntf = 0;

                //loop through each unique term in a doc
                foreach (string docTerm in docTerms)
                {
                    //foreach document loop through each term in each doc
                    foreach (List<string> doc in queryAndDocuments)
                    {
                        double docSize = (double)doc.Count();
                        
                        foreach (string term in doc)
                        {
                            if (term.Equals(docTerm))
                            {
                                tf++;
                            }
                        }

                        //calculate specific normalised term frequency
                        ntf = tf / docSize;

                        if (!termFrequencies.ContainsKey(docTerm) && ntf > 0)
                        {
                            termFrequencies.Add(docTerm, ntf);

                        }

                        tf = 0;
                        ntf = 0;
                        docSize = 0;
                    }
                }
                //save all term frequencies
                if (!allTFValues.ContainsKey(docNum))
                {
                    allTFValues.Add(docNum, termFrequencies);
                }
            }

            //return tfs
            return allTFValues;
        }

        public static Dictionary<string, double> CalculateIDF(List<List<string>> queryAndDocuments, List<string> uniqueTerms)
        {
            double numDocs = queryAndDocuments.Count() - 1;

            List<double> idfs = new List<double>();

            foreach (string term in uniqueTerms)
            {
                double appears = 0;
                foreach (List<string> doc in queryAndDocuments)
                {
                    if (doc.Contains(term))
                    {
                        appears++;
                    }

                }

                double idf = Math.Log10(numDocs / appears);
                idfs.Add(idf);
                appears = 0;
                idf = 0;
            }

            //Return List of Dictionarys

            Dictionary<string, double> allIDFValues = new Dictionary<string, double>();

            for (int i = 0; i < uniqueTerms.Count; i++)
            {
                allIDFValues.Add(uniqueTerms[i], idfs[i]);
            }

            return allIDFValues;
        }

        public static Dictionary<string, Dictionary<string, double>> CalculateTfIdf(Dictionary<string, Dictionary<string, double>> TFs, Dictionary<string, double> IDFs)
        {
            Dictionary<string, Dictionary<string, double>> tfidfResults = new Dictionary<string, Dictionary<string, double>>();

            foreach (KeyValuePair<string, Dictionary<string, double>> docTfs in TFs)
            {
                string docNum = docTfs.Key;
                Dictionary<string, double> termFreqs = docTfs.Value;

                Dictionary<string, double> termTFIDFResults = new Dictionary<string, double>();


                foreach (KeyValuePair<string, double> tf in termFreqs)
                {
                    string term1 = tf.Key;
                    double termFreq = tf.Value;

                    double inverseDocFreq = IDFs[term1];

                    double tf_idf = termFreq * inverseDocFreq;

                    if (!termTFIDFResults.ContainsKey(term1))
                    {
                        termTFIDFResults.Add(term1, tf_idf);
                    }
                    
                }
                if (!termFreqs.ContainsKey(docNum))
                {
                    tfidfResults.Add(docNum, termTFIDFResults);
                }
            }

            return tfidfResults;
        }

        public static Dictionary<string, double> CalculateVectors(Dictionary<string, Dictionary<string, double>> TFIDFs)
        {
            Dictionary<string, double> vectorResults = new Dictionary<string, double>();

            foreach(KeyValuePair<string, Dictionary<string, double>> docTFIDFs in TFIDFs)
            {
                string docNum = docTFIDFs.Key;
                Dictionary<string, double> tf_idfs = docTFIDFs.Value;
                double sqTFIDFs = 0;
                foreach(KeyValuePair<string, double> tfidf in tf_idfs)
                {
                    sqTFIDFs += (tfidf.Value * tfidf.Value);
                }
                double vectorLength = Math.Sqrt(sqTFIDFs);

                if(!vectorResults.ContainsKey(docNum))
                {
                    vectorResults.Add(docNum, vectorLength);
                }
            }

            return vectorResults;
        }

        public static Dictionary<string, double> CalculateDotProducts(Dictionary<string, Dictionary<string, double>> TFIDFs)
        {
            Dictionary<string, double> dotProductResults = new Dictionary<string, double>();

            //query tfidf values
            Dictionary<string, double> qTFIDFs = TFIDFs["doc0"];

            TFIDFs.Remove("doc0");

            foreach (KeyValuePair<string, Dictionary<string, double>> docTFIDFs in TFIDFs)
            {
                string docNum = docTFIDFs.Key;
                double dotProduct = 0;
                Dictionary<string, double> termTFIDFs = docTFIDFs.Value;
                foreach (KeyValuePair<string, double> termTFIDF in termTFIDFs)
                {
                    
                    
                    if (qTFIDFs.ContainsKey(termTFIDF.Key))
                    {
                        dotProduct += (qTFIDFs[termTFIDF.Key] * termTFIDF.Value);
                    }
                    
                }
                dotProductResults.Add(docNum, dotProduct);

            }


            return dotProductResults;
        }

        public static Dictionary<string, double> calculateRelevancy(Dictionary<string, double> dotProducts, Dictionary<string, double> vectors)
        {
            Dictionary<string, double> relevancies = new Dictionary<string, double>();

            foreach(KeyValuePair<string, double> dp in dotProducts)
            {
                double relevance = dp.Value / (vectors["doc0"] * vectors[dp.Key]);
                relevancies.Add(dp.Key, relevance);
            }

            return relevancies;
        }
    }
}
