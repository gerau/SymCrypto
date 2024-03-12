using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntropyMeasuring
{
    internal class Program
    {
        static void Main()
        {
            
            var inputfile = "C:\\uni\\6\\SC\\labs\\lab1\\text_with_spaces.txt";
            var withSpaces = true;
            Analyzer an = new Analyzer(inputfile, withSpaces);

            var size = Analyzer.alphabet.Length - (withSpaces ? 0 : 1);

            var lProbabilities = an.LetterProbabilities();
            var lFrequencies = an.LetterFrequencies();

            var bProbabilities = an.BigramProbabilities();
            var bFrequencies = an.BigramFrequencies();

            Dictionary<char, (float p, ulong f)> counts = new Dictionary<char, (float p ,ulong l)>();

            for(int i = 0; i < size; i++)
            {
                counts.Add(Analyzer.alphabet[i], (lProbabilities[i],lFrequencies[i]));
                Console.WriteLine($"{Analyzer.alphabet[i]} & {lFrequencies[i]} & {lProbabilities[i].ToString("0.000").Replace(',', '.')} \\\\");
            }
           
            var sortedCounts = from entry in counts orderby entry.Value descending select entry;
            Console.Write("\n \n ");

            foreach (var entry in sortedCounts )
            {
                Console.WriteLine($"{entry.Key} & {entry.Value.f} & {entry.Value.p.ToString("0.0000").Replace(',', '.')} \\\\");
            }
            
            Console.Write(" \n ");
            for (int i = 0; i < size; i++)
            {
                Console.Write($"& {Analyzer.alphabet[i]}");
            }
            Console.Write("\\\\ \n ");

            for(int i = 0; i < size; i++)
            {
                Console.Write($"{Analyzer.alphabet[i]}");
                for(int j = 0; j < size;  j++)
                {
                    Console.Write($"& {(100 * bProbabilities[i,j]).ToString("0.00")}\\%".Replace(',','.'));
                }
                Console.Write("\\\\ \n ");
            }


            Console.WriteLine("letter table:");
            foreach (var entry in sortedCounts)
            {
                Console.WriteLine($"{entry.Key}: probability: {entry.Value.p.ToString("0.0000").Replace(',', '.')}, count:  {entry.Value.f}");
            }
            Console.Write(" |");
            for(int i = 0; i <size; i++)
            {
                Console.Write($"{Analyzer.alphabet[i]}    |");
            }
            Console.Write("\n");
            for (int i = 0; i < size; i++)
            {
                Console.Write($"{Analyzer.alphabet[i]}|");
                for (int j = 0; j < size; j++)
                {
                    Console.Write($"{(100 * bProbabilities[i, j]).ToString("0.00")}%|".Replace(',', '.'));
                }
                Console.Write("\n");
            }
            
            Console.WriteLine($"bigram entropy: {an.BigramEntropy()}");
            Console.WriteLine($"letter entropy: {an.LetterEntropy()}");
        }
    }
}
