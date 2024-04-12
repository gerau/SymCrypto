using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static AffineCipher.LanguageConstants;
namespace AffineCipher
{
    public class AffineAnalyzer
    {
        private static int modulo = alphabet.Length * alphabet.Length;
        public static int ConvertBigramToNumber(string bigram)
        {
            return alphabet.IndexOf(bigram[0]) * alphabet.Length + alphabet.IndexOf(bigram[1]);
        }
        public static string ConvertNumberToBigram(int number)
        {
            return alphabet.ElementAt(number / alphabet.Length).ToString() + alphabet.ElementAt(number % alphabet.Length);   
        }
       
        public static List<(string x1,string x2)> GetPairs(List<string> list) 
        {

            List<(string, string)> pairs = [];
            for(int i = 0; i < list.Count; i++)
            {
                for(int j = i + 1; j < list.Count; j++)
                {
                    var pair = (list[i], list[j]);
                    pairs.Add(pair);
                }
            }
            return pairs;
        }
        

        public static List<(int, int)> GetProbableKeys(string cipheredFile, string languageFile, int numberOfComparisions)
        {
            var keys = new List<(int,int)>();
            var cipherFrequencies = (from entry in TextAnalyzer.BigramFrequencies(cipheredFile)
                                  orderby entry.Value
                                  descending
                                  select entry.Key).Take(numberOfComparisions).ToList();

            /*var languageFrequencies = (from entry in FrequencyCounter.BigramFrequencies(languageFile)
                                       orderby entry.Value
                                       descending
                                       select entry.Key).Take(numberOfComparisions).ToList();*/

            var languageFrequencies = new List<string>(["ст", "но", "то", "на", "ен"]);


            foreach (var cipherPair in GetPairs(cipherFrequencies))
            {
                foreach (var languagePair in GetPairs(languageFrequencies))
                {
                    var x = ConvertBigramToNumber(languagePair.x1) - ConvertBigramToNumber(languagePair.x2);
                    var y = ConvertBigramToNumber(cipherPair.x1) - ConvertBigramToNumber(cipherPair.x2);
                    try
                    {
                        int[] solutions = Utils.SolveLinearComparision(x, y, modulo);
                        foreach(int a in solutions)
                        {
                            var temp = (a, Utils.Modulo(
                                ConvertBigramToNumber(cipherPair.x1) - a * ConvertBigramToNumber(languagePair.x1),
                                modulo));
                            if (!keys.Contains(temp))
                            {
                                keys.Add(temp);
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return keys;
        }


        public static string DecipherString(string inputText, int a,int b)
        {
            string initialText = "";
            var inversed_a = Utils.ExtentedGCD(a, modulo)[0];
            for(int i = 0; i < inputText.Length - 1; i+=2) 
            { 
                var y = ConvertBigramToNumber(inputText[i].ToString() + inputText[i+1]);
                var x = inversed_a * (y - b);
                x = Utils.Modulo(x, modulo);
                initialText += ConvertNumberToBigram(x);
            }
            return initialText;
        }

        public static void DecipherFile(string inputFile, string outputFile, int a, int b)
        {
            string initialText = "";
            if(File.Exists(inputFile))
            {
                initialText = File.ReadAllText(inputFile);
            }
            initialText = DecipherString(initialText, a, b);
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.Write(initialText);
            }
        }
    }
}
