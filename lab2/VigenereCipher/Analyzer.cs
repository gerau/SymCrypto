using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher
{
    internal class Analyzer
    {

        public static Dictionary<char, float> probabilitiesOfLanguage = new Dictionary<char, float>
        {
            {'о', 0.115618154f},
            {'е', 0.08484142f},
            {'а', 0.081847705f},
            {'и', 0.067659505f},
            {'н', 0.06667096f},
            {'т', 0.059215903f},
            {'с', 0.053033203f},
            {'л', 0.050281003f},
            {'в', 0.046754025f},
            {'р', 0.042343237f},
            {'к', 0.033968024f},
            {'д', 0.030273866f},
            {'м', 0.029032568f},
            {'у', 0.027271885f},
            {'п', 0.02560636f},
            {'я', 0.021827288f},
            {'ь', 0.019546565f},
            {'ы', 0.019039737f},
            {'г', 0.019031808f},
            {'б', 0.017322669f},
            {'з', 0.016818153f},
            {'ч', 0.015550423f},
            {'ж', 0.0110157365f},
            {'й', 0.010723337f},
            {'ш', 0.008890959f},
            {'х', 0.008160784f},
            {'ю', 0.006156271f},
            {'э', 0.0032758776f},
            {'ц', 0.0032659655f},
            {'щ', 0.0029071553f},
            {'ф', 0.0016691611f},
            {'ъ', 0.00038028593f}
        };
        
        public const float IndexOfLanguage = 0.0562869f;
        public static Dictionary<char, int> GetLetterFrequencies(string initialText)
        {
            Dictionary<char,int> result = new Dictionary<char,int>();
            foreach(char letter in Vigenere.alphabet)
            {
                result[letter] = 0;
            }
            foreach (char c in initialText)
            {
                result[c]++;
            }
            return result;
        }
        public static float CalculateIndexOfLanguage()
        {
            float result = 0;
            foreach(var probability in probabilitiesOfLanguage)
            {
                result += probability.Value*probability.Value;
            }
            return result;
        }
        public static float IndexOfCoincedenceOfText(string initialText) 
        {
            var frequencies = GetLetterFrequencies(initialText);
            var size = 0;
            float result = 0;
            foreach(var pair in frequencies)
            {
                size += pair.Value;
                result += (pair.Value - 1) * pair.Value; 
            }
            return result / (float)(size * (size - 1));
        }

        public static float IndexOfCoincedenceOfFile(string inputFile)
        {
            string initialText = "";
            if (File.Exists(inputFile))
            {
                initialText = File.ReadAllText(inputFile);
            }
            var frequencies = GetLetterFrequencies(initialText);
            var size = 0;
            float result = 0;
            foreach (var pair in frequencies)
            {
                size += pair.Value;
                result += (pair.Value - 1) * pair.Value;
            }
            return result / (size * (size - 1));
        }

        public static string[] DivideIntoBlocks(string initialText,int sizeOfBlock)
        {
            string[] blocks = new string[sizeOfBlock];

            for(int i = 0; i < initialText.Length; i++) 
            {
                blocks[i % sizeOfBlock] += initialText[i];
            }
            return blocks;
        }


        public static int FindIndex(string inputFile, int maxKeyLength) 
        {
            int index = 0;
            float closest = float.PositiveInfinity;
            string cipheredText = "";
            if (File.Exists(inputFile))
            {
                cipheredText = File.ReadAllText(inputFile);
            }
            for (int r = 2; r < maxKeyLength; r++)
            {
                float average = 0;
                var blocks = DivideIntoBlocks(cipheredText, r);
                for (int i = 0; i < r; i++)
                {
                    average += IndexOfCoincedenceOfText(blocks[i]);
                }
                average /= r;
                Console.WriteLine($"for block size of {r}: {average}");
                /*Console.Write($" ({r},{average.ToString().Replace(',', '.')}) ");
                Console.WriteLine($" {r} & {average.ToString().Replace(',', '.')} \\\\ ");*/

                if (closest > Math.Abs(average - IndexOfLanguage))
                {
                    index = r;
                    closest = Math.Abs(average - IndexOfLanguage);
                }
            }
            return index;
        }
        public static string FrequencyAnalysisMethod(string inputFile, int numberOfBlocks)
        {
            string cipheredText = "";
            string probableKey = "";


            if (File.Exists(inputFile))
            {
                cipheredText = File.ReadAllText(inputFile);
            }
            var blocks = DivideIntoBlocks(cipheredText, numberOfBlocks);
            for (int i = 0; i < numberOfBlocks; i++)
            {
                var frequencies = GetLetterFrequencies(blocks[i]);
                frequencies = (from entry in frequencies orderby entry.Value descending select entry).ToDictionary();
                char[] probableKeys = new char[3];
                string printLine = $"Block {i}, probable keys: ";

                for (int j = 0; j < 3; j++)
                {
                    var k = Vigenere.SubOfLetters(frequencies.ElementAt(j).Key, probabilitiesOfLanguage.ElementAt(j).Key);
                    probableKeys[j] = k;
                    printLine += $"{k}, ";
                }
                Console.WriteLine(printLine);
                probableKey += probableKeys[0];
            }
            return probableKey;
        }

        public static string MoreStableAnalysisMethod(string inputFile, int numberOfBlocks)
        {
            string key = "";
            string cipheredText = "";
            if (File.Exists(inputFile))
            {
                cipheredText = File.ReadAllText(inputFile);
            }
            var blocks = DivideIntoBlocks(cipheredText, numberOfBlocks);
            
            for (int i = 0; i < numberOfBlocks; i++)
            {
                var frequencies = GetLetterFrequencies(blocks[i]);
                float maxValue = 0;
                int maxg = 0;
                for(int g = 0; g < Vigenere.alphabet.Length; g++)
                {
                    float sum = 0;
                    foreach(var pair in probabilitiesOfLanguage)
                    {
                        var index = Vigenere.SumOfLetters(pair.Key, Vigenere.alphabet.ElementAt(g));
                        sum += pair.Value * frequencies[index];
                    }
                    if(sum > maxValue)
                    {
                        maxg = g;
                        maxValue = sum;
                    }
                }
                key += Vigenere.alphabet.ElementAt(maxg);
            }
            return key;
        }





    }
}
