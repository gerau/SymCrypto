using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntropyMeasuring
{
    public class Analyzer
    {
        public const string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        public string inputFile;
        bool WithSpaces;

        public Analyzer(string inputFile, bool withSpaces)
        {
            this.inputFile = inputFile;
            WithSpaces = withSpaces;
        }

        public ulong[] LetterFrequencies()
        {
            ulong[] frequencies = new ulong[alphabet.Length - (WithSpaces ? 0 : 1)];
            if (File.Exists(inputFile))
            {
                string buffer = File.ReadAllText(inputFile);
                foreach (var letter in buffer)
                {
                    frequencies[alphabet.IndexOf(letter)] += 1;
                }
            }
            return frequencies;
        }
        public float[] LetterProbabilities()
        {
            float[] probabilities = new float[alphabet.Length - (WithSpaces ? 0 : 1)];
            ulong[] frequencies = LetterFrequencies();

            var textSize = TextSize();

            for(int i = 0; i < probabilities.Length; i++)
            {
                probabilities[i] = (float)frequencies[i]/textSize;
            }
            return probabilities;
        }
        public (ulong[,] frequencies, ulong size) BigramFrequencies(int step = 1)
        {
            ulong[,] frequencies = new ulong[alphabet.Length - (WithSpaces ? 0 : 1), alphabet.Length - (WithSpaces ? 0 : 1)];
            ulong size = 0;
            if (File.Exists(inputFile))
            {
                string s = File.ReadAllText(inputFile);

                for (int i = 0; i < s.Length - 1; i += step)
                {
                    frequencies[alphabet.IndexOf(s[i]), alphabet.IndexOf(s[i + 1])] += 1;
                    size++;
                }
            }
            return (frequencies,size);
        }

        public float[,] BigramProbabilities(int step = 1)
        {
            float[,] probabilities = new float[alphabet.Length - (WithSpaces ? 0 : 1), alphabet.Length - (WithSpaces ? 0 : 1)];
            var frequencies = BigramFrequencies(step);
            for (int i = 0; i < alphabet.Length - (WithSpaces ? 0 : 1); i++)
            {
                for (int j = 0; j < alphabet.Length - (WithSpaces ? 0 : 1); j++)
                {
                    probabilities[i, j] = (float)frequencies.frequencies[i,j] / (float)frequencies.size;
                }
            }
            return probabilities;
        }


        public float LetterEntropy()
        {
            float result = 0;
            var probabilities = LetterProbabilities();
            for (int i = 0; i < alphabet.Length - (WithSpaces ? 0 : 1); i++)
            {
                var log = probabilities[i] != 0 ? Math.Log(probabilities[i], 2) : 0; 
                result += (float)(log * probabilities[i]);
            }
            return -result;
        }


        public float BigramEntropy(int step = 1) 
        {
            float result = 0;
            var probabilities = BigramProbabilities(step);
            for (int i = 0; i < alphabet.Length - (WithSpaces ? 0 : 1); i++)
            {
                for(int j = 0; j < alphabet.Length - (WithSpaces ? 0 : 1); j++) 
                {
                    var log = probabilities[i,j] != 0 ? Math.Log(probabilities[i,j], 2) : 0;
                    result += (float)(log * probabilities[i,j]);
                }
            }
            return -result/2;
        }


        public ulong TextSize()
        {
            string s = "";
            if (File.Exists(inputFile))
            {
                s = File.ReadAllText(inputFile);
            }
            return (ulong)s.Length;
        }
    }
}
