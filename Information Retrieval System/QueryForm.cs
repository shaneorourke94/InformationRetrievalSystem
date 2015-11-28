using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            //split string into the seperate words
            string[] splitArray = lowerCaseQuery.Split(_delimiters, StringSplitOptions.RemoveEmptyEntries);

            string[] q = StopWords.RemoveStopWords(splitArray);
            string[] finalQuery = WordStemmer.QueryStemmer(q);

            //get each document as a string array with query as first value
            List<string[]> queryAndDocs = Preprocessing.DocProcesser.GetDocuments();           
            queryAndDocs.Insert(0, finalQuery);

            List<string[]> documents = Preprocessing.DocProcesser.GetDocuments();

            //get all unique terms
            string[] allUniqueTerms = Preprocessing.UniqueTerms.getUniqueTerms(finalQuery, documents);

            //get normalised term frequencies for every term
            List<KeyValuePair<string, List<double>>> termFrequencies = RelevanceChecker.WeightingSchemeCalculation.NormalisedTF(queryAndDocs, allUniqueTerms);

            //get Inverse Document Frequency
            List<KeyValuePair<string, double>> InverseDocumentFrequencies = RelevanceChecker.WeightingSchemeCalculation.CalculateIDF(queryAndDocs, allUniqueTerms);

            //get tf-idf
            //List<KeyValuePair<string, List<double>>> TFIDFs = RelevanceChecker.WeightingSchemeCalculation.CalculateTfIdf(termFrequencies, InverseDocumentFrequencies, allUniqueTerms);
        }
    }
}
