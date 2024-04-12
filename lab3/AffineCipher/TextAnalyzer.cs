using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AffineCipher.LanguageConstants;


namespace AffineCipher
{
    public class TextAnalyzer
    {
        public static Dictionary<string, long> LetterFrequenciesOfString(string text) 
        {
            Dictionary<string, long> frequencies = [];
            for (int i = 0; i < alphabet.Length; i++)
            {
                frequencies.Add(alphabet[i].ToString(), 0);
            }
            foreach (var letter in text)
            {
                frequencies[letter.ToString()] += 1;
            }
            return frequencies;
        }
        public static Dictionary<string, float> LetterProbabilitiesOfString(string text)
        {
            Dictionary<string, long> frequencies = LetterFrequenciesOfString(text);
            Dictionary<string, float> probabilities = [];

            foreach (var item in frequencies)
            {

                probabilities.Add(item.Key, (float)item.Value / text.Length);
            }
            return probabilities;
        }
        public static Dictionary<string, long> LetterFrequencies(string inputFile)
        {
            Dictionary<string, long> frequencies = [];
            if (File.Exists(inputFile))
            {
                string buffer = File.ReadAllText(inputFile);
                frequencies = LetterFrequenciesOfString(buffer);
            }
            return frequencies;
        }
        public static Dictionary<string, float> LetterProbabilities(string inputFile)
        {
            Dictionary<string, float> probabilities = [];
            var frequencies = LetterFrequencies(inputFile);

            var textSize = TextSize(inputFile);

            foreach(var item in frequencies) { 

                probabilities.Add(item.Key, (float)item.Value/textSize);
            }
            return probabilities;
        }

        public static Dictionary<string, long> BigramFrequenciesOfString(string text, int step = 1)
        {
            Dictionary<string, long> frequencies = [];
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    frequencies.Add(alphabet[i].ToString() + alphabet[j], 0);
                }
            }
            for (int i = 0; i < text.Length - 1; i += step)
            {
                frequencies[text[i].ToString() + text[i + 1]] += 1;
            }
            return frequencies;
        }

        public static Dictionary<string,long> BigramFrequencies(string inputFile, int step = 1)
        {
            Dictionary<string, long> frequencies = [];
            if (File.Exists(inputFile))
            {
                string s = File.ReadAllText(inputFile);

                frequencies = BigramFrequenciesOfString(s, step);
            }
            return frequencies;
        }

        public Dictionary<string, float> BigramProbabilities(string inputFile, int step = 1)
        {
            Dictionary<string, float> probabilities = [];
            var frequencies = BigramFrequencies(inputFile, step);
            var size = frequencies.Sum(x => x.Value);
            foreach (var entry in frequencies)
            {
                probabilities.Add(entry.Key, (float)entry.Value / size);
            }
            return probabilities;
        }


        public static float LetterEntropyOfString(string text)
        {
            float result = 0;
            var probabilities = LetterProbabilitiesOfString(text);
            foreach (var entry in probabilities)
            {
                var log = entry.Value != 0 ? Math.Log(entry.Value, 2) : 0;
                result += (float)(log * entry.Value);
            }
            return -result;
        }

        public static float LetterEntropy(string inputFile)
        {
            float result = 0;
            var probabilities = LetterProbabilities(inputFile);
            foreach(var entry in probabilities)
            {
                var log = entry.Value != 0 ? Math.Log(entry.Value, 2) : 0;
                result += (float)(log * entry.Value);
            }
            return -result;
        }


        /*public float BigramEntropy(int step = 1)
        {
            float result = 0;
            var probabilities = BigramProbabilities(step);
            for (int i = 0; i < alphabet.Length ; i++)
            {
                for (int j = 0; j < alphabet.Length ; j++)
                {
                    var log = probabilities[i, j] != 0 ? Math.Log(probabilities[i, j], 2) : 0;
                    result += (float)(log * probabilities[i, j]);
                }
            }
            return -result / 2;
        }*/


        public static ulong TextSize(string inputFile)
        {
            string s = "";
            if (File.Exists(inputFile))
            {
                s = File.ReadAllText(inputFile);
            }
            return (ulong)s.Length;
        }
        public static float IndexOfCoincedenceOfString(string initialText)
        {
            var frequencies = LetterFrequenciesOfString(initialText);
            long size = 0;
            float result = 0;
            foreach (var pair in frequencies)
            {
                size += pair.Value;
                result += (pair.Value - 1) * pair.Value;
            }
            return result / (float)(size * (size - 1));
        }



    }
}

