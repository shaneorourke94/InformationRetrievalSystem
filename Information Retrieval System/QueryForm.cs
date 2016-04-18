using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Information_Retrieval_System
{
    public partial class QueryForm : Form
    {
        //query string variable

        public static string query;
        public static List<string> finalResults = new List<string>();
        public static int timerResult;

        public QueryForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            query = textBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //timer for how long it takes
            Stopwatch timer = new Stopwatch();
            timer.Start();
            string lowerCaseQuery = query.ToLower();

            //delimiters for splitting input string
            char[] _delimiters = new char[]
            {
            ' ',
            '.',
            ',',
            ';',
            '!',
            '?',
            '"',
            '/',
            '(',
            ')',
            '-',
            };

            //split string into the seperate words (used array because of stemmer)
            string[] splitArray = lowerCaseQuery.Split(_delimiters, StringSplitOptions.RemoveEmptyEntries);

            string[] q = StopWords.RemoveStopWords(splitArray);
            string[] finalQuery = WordStemmer.QueryStemmer(q);
            List<string> finalQueryList = finalQuery.ToList();

            //get each document as a string list with query as first value
            List<List<string>> queryAndDocs = Preprocessing.DocProcesser.GetDocuments();           
            queryAndDocs.Insert(0, finalQueryList);

            //get unique terms for each doc
            List<List<string>> docUniqueTerms = Preprocessing.UniqueTerms.getDocUniqueTerms(queryAndDocs);

            //get all unique terms
            List<string> allUniqueTerms = Preprocessing.UniqueTerms.getAllUniqueTerms(queryAndDocs);

            //get normalised term frequencies for every term (algorithm O(n^4))
            Dictionary<string, Dictionary<string, double>> termFrequencies = RelevanceChecker.WeightingSchemeCalculation.NormalisedTF(queryAndDocs, docUniqueTerms);

            //get Inverse Document Frequency (algorithm O(n^2))
            Dictionary<string, double> InverseDocumentFrequencies = RelevanceChecker.WeightingSchemeCalculation.CalculateIDF(queryAndDocs, allUniqueTerms);

            //get tf-idf (algorithm O(n^2), uses dictionaries for finding values)
            Dictionary<string, Dictionary<string, double>> TFIDFs = RelevanceChecker.WeightingSchemeCalculation.CalculateTfIdf(termFrequencies, InverseDocumentFrequencies);

            //get vector lengths of query and documents (algorithm O(n^2), uses dictionaries for finding values)
            Dictionary<string, double> Vectors = RelevanceChecker.WeightingSchemeCalculation.CalculateVectors(TFIDFs);

            //get dot Product of each document (algorithm O(n^2), uses dictionaries for finding values)
            Dictionary<string, double> dotProducts = RelevanceChecker.WeightingSchemeCalculation.CalculateDotProducts(TFIDFs);

            //get relevancy (algorithm O(n), uses dictionaries for finding values)
            Dictionary<string, double> relevancy = RelevanceChecker.WeightingSchemeCalculation.calculateRelevancy(dotProducts, Vectors);

            //remove completely irrelevant documents
            foreach (var item in relevancy.Where(kvp => kvp.Value == 0).ToList())
            {
                relevancy.Remove(item.Key);
            }

            //order list of results from most relevant to least relevant
            List<KeyValuePair<string, double>> finalDocList = new List<KeyValuePair<string, double>>();
            finalDocList.AddRange(relevancy);
            List<KeyValuePair<string, double>> sortedDocList = finalDocList.OrderByDescending(o => o.Value).ToList();

            //put ranked list in public string list
            foreach (var doc in sortedDocList)
            {
                finalResults.Add(doc.Key);
            }
            timer.Stop();
            timerResult = (int)timer.Elapsed.TotalMilliseconds/1000;
            //load results form
            var resultsForm = new ResultsForm();
            resultsForm.Show();
            this.Hide();
        }
    }
}
