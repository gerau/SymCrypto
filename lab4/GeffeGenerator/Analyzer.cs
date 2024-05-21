using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GeffeGenerator
{
    public class Analyzer
    {
        public const string sequence = "1001000001100001100000111000110000110110010011100000011010011110101000100110001111011001100111101111001011001101101000010100001110000011111101100001101011100011011010000110111101001111100011011110001111010011001001111000110001100110011100100000011110010001010011010110101101010011";

        public const int C1 = 81;
        public const int C2 = 84;

        public const int N1 = 258;
        public const int N2 = 265;

        public static BitSequence x1 = GetBitArray(N1);
        public static BitSequence x2 = GetBitArray(N2);



        public static BitSequence GetBitArray(int size)
        {
            BitSequence bitArray = new BitSequence(size);
            for(int i =0; i < size; i++)
            {
                if (sequence[i] == '1')
                {
                    bitArray.SetBit(i);
                }
            }
            return bitArray;
        }

        public static void AnalyzeL1()
        {
            object lockObj = new object();
            Parallel.For(0, 1 << 30, i =>
            {
                BitSequence localTemp = new BitSequence(N1);
                LFSR L1 = new LFSR(0b1010011, (uint)i, 30);
                for (int j = 0; j < N1; j++)
                {
                    var bit = L1.Step();
                    if (bit == 1)
                    {
                        localTemp.SetBit(j);
                    }
                }

                var t = localTemp ^ x1;
                var R1 = t.Count();
                if (R1 <= C1)
                {
                    lock (lockObj)
                    {
                        Console.WriteLine($"{i.ToString(format: "b").PadLeft(30, '0')}; R1 = {R1}");
                    }
                }
            });
        }

    }
}
