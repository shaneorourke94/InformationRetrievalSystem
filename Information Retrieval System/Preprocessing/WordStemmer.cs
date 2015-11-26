using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Annytab;

namespace Information_Retrieval_System
{
    class WordStemmer
    {
        public static string[] QueryStemmer(string[] input)
        {
            //create stemmer
            Stemmer stemmer = new EnglishStemmer();

            //store stemmed words in string array
            string[] queryStems = stemmer.GetSteamWords(input);

            return queryStems;
        }       
    }
}
