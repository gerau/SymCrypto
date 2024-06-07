using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GeffeGenerator
{
    public class AnalyzeDummies
    {
        public const string sequence = "00010101000010100010101001111111111011001101010001101110101010000001010111101001110001100111000001000011100010111101010100000101011110101010111100001100000110111011000000000111011101011011000000011110001000110011100001000110001000000000011011000101111011000001110011100011011011111110111110101100000100110101100001001101001100011000011110101010011000101000010011101001000110011011000010100000101100100000100110100011111001100100001110110001001001001011110111010011110111010101010101000111100000000001111010001000010001000100010100100101001000111010001000100100001011010010000111111001001100100001111010001001100000011101001001000110111000010110111111011010011101010010100100001111111111101110010000110000010011111100001011111001010001011011100011100010110011100101101101111100011110101101001011010110111001101010000010100110001111111100110010100110110001011011001100101100101000101010001101101100111001100101110000011000111101111111001100110110111101100101010101011100011001000010111010100010101100010111110111110001000110001110100101110111010000111110011000110010010011011111111011111111011011101101001000000000001011111010101011101010010010001010110011100101010011100101010001111000010001100111100111110111101101101111001000010101001101100001010100011100100001111000110100111000011101011101011001100001110011111010000010010111111010001110001011010100011110000011100100111001100001001111111111001001000110001000111110010011101111001010000010000111011101101011101001000110011000010011010100111100011101000100101011101001011000110010110000111100110011010011110011100101100101101100100101110101010010001010101101011101011100100100110000011010110111100001111101101110011100010101011110111010110101011101110100110011101110000010101000110110111101100010111100001011101101111010111001100110010100000110011101111111000101111110010001011011010001001011000110000000100100101011010110110010001110110011100010101111001101110010010100010001110001110000110010000101111110110011111111001100101000000001110110011010100111001100010101011111000010111000010101101111";

        public const int C1 = 71;
        public const int C2 = 73;

        public const int N1 = 222;
        public const int N2 = 229;
        public const int threads = 8;

        public static BitSequence one = BitSequence.GetOnes((ulong)sequence.Length);

        public static BitSequence x1 = GetBitArrayResult(N1);
        public static BitSequence x2 = GetBitArrayResult(N2);
        public static BitSequence x3 = GetBitArrayResult((ulong)sequence.Length);

        public static int L1size = 25;
        public static int L2size = 26;
        public static int L3size = 27;

        public BitSequence L1res = new((1ul << L1size) + (ulong)(sequence.Length + L1size));
        public BitSequence L2res = new((1ul << L2size) + (ulong)(sequence.Length + L2size));
        public BitSequence L3res = new((1ul << L3size) + (ulong)(2 * sequence.Length + L3size));

        public AnalyzeDummies()
        {

            LFSR L1 = new LFSR(0b1001, 1u, L1size);
            for (uint i = 0; i < (1u << L1size) + sequence.Length + L1size; i++)
            {
                if (L1.Step() == 1)
                {
                    L1res.SetBit(i);
                }
            }
            Console.WriteLine("L1 initialized");
            LFSR L2 = new(0b1000111, 1, L2size);
            for (uint i = 0; i < (1u << L2size) + sequence.Length + L2size; i++)
            {
                if (L2.Step() == 1)
                {
                    L2res.SetBit(i);
                }
            }
            Console.WriteLine("L2 initialized");
            LFSR L3 = new(0b100111, 1, L3size);
            for (uint i = 0; i < (1u << L3size) + 2 * sequence.Length + L3size; i++)
            {
                if (L3.Step() == 1)
                {
                    L3res.SetBit(i);
                }
            }
            Console.WriteLine("L3 initialized");
        }

        public static BitSequence GetBitArray(int size)
        {
            BitSequence bitArray = new BitSequence((ulong)size);
            for (int i = 0; i < size; i++)
            {
                if (sequence[i] == '1')
                {
                    bitArray.SetBit((uint)i);
                }
            }
            return bitArray;
        }
        public static BitSequence GetBitArrayResult(ulong size)
        {
            BitSequence bitArray = new BitSequence(size);
            for (ulong i = 0; i < size; i++)
            {
                if (sequence[(int)i] == '1')
                {
                    bitArray.SetBit(i);
                }
            }
            return bitArray;
        }
        public List<BitSequence> AnalyzeL1Parallel()
        {
            var result = new ConcurrentBag<BitSequence>();
            Parallel.For(0, 1u << threads, i =>
            {
                BitSequence state = new(N1);
                ulong shift = (1ul << (L1size - threads)) * (ulong)i;
                for (uint j = 0; j < N1; j++)
                {
                    if (L1res.GetBit(j + shift) == 1)
                    {
                        state.SetBit(j);
                    }
                }
                for (var j = shift; j < (1ul << (L1size - threads)) * (ulong)(i + 1); j++)
                {
                    var R1 = (state ^ x1).Count();
                    if (R1 < C1)
                    {
                        BitSequence r = L1res.GetNBits(j, (ulong)sequence.Length);
                        result.Add(r);
                    }
                    state = state.ShiftToLeft();
                    if (L1res.GetBit(j + N1) == 1)
                    {
                        state.SetBit((uint)state.size - 1);
                    }
                }
            });
            Console.WriteLine($"L1 analyzed, find {result.Count} variants");
            return result.ToList();
        }

        public List<BitSequence> AnalyzeL2Parallel()
        {
            var result = new ConcurrentBag<BitSequence>();
            Parallel.For(0, 1u << threads, i =>
            {
                BitSequence state = new(N2);
                ulong shift = (1ul << (L2size - threads)) * (ulong)i;
                for (uint j = 0; j < N2; j++)
                {
                    if (L2res.GetBit(j + shift) == 1)
                    {
                        state.SetBit(j);
                    }
                }
                for (var j = shift; j < (1ul << (L2size - threads)) * (ulong)(i + 1); j++)
                {
                    var R2 = (state ^ x2).Count();
                    if (R2 < C2)
                    {
                        BitSequence r = L2res.GetNBits(j, (ulong)sequence.Length);
                        result.Add(r);
                    }
                    state = state.ShiftToLeft();
                    if (L2res.GetBit(j + N2) == 1)
                    {
                        state.SetBit((uint)state.size - 1);
                    }
                }
            });
            Console.WriteLine($"L2 analyzed, find {result.Count} variants");
            return result.ToList();
        }

        public List<BitSequence> AnalyzeL1()
        {
            var result = new List<BitSequence>();
            BitSequence state = new(N1);
            for (uint i = 0; i < N1; i++)
            {
                if (L1res.GetBit(i) == 1)
                {
                    state.SetBit(i);
                }
            }
            for (var i = 0u; i < (1 << 25); i++)
            {
                var R1 = (state ^ x1).Count();

                if (R1 < C1)
                {
                    uint res = 0;
                    for (uint j = 0; j < 25; j++)
                    {
                        var bit = L1res.GetBit(i + j);
                        res += (uint)bit << (int)(j);
                    }
                    BitSequence r = L1res.GetNBits(i, (ulong)sequence.Length);

                    result.Add(r);
                }
                state = state.ShiftToLeft();
                if (L1res.GetBit(i + N1) == 1)
                {
                    state.SetBit((uint)state.size - 1);
                }
            }
            Console.WriteLine($"L1 analyzed, find {result.Count} variants");
            return result;
        }

        public List<BitSequence> AnalyzeL2()
        {
            var result = new List<BitSequence>();
            BitSequence state = new(N2);
            for (uint i = 0u; i < N2; i++)
            {
                if (L2res.GetBit(i) == 1)
                {
                    state.SetBit(i);
                }
            }
            for (var i = 0u; i < (1 << L2size); i++)
            {
                var R2 = (state ^ x2).Count();

                if (R2 < C2)
                {
                    BitSequence r = L2res.GetNBits(i, (ulong)sequence.Length);

                    result.Add(r);
                }
                state = state.ShiftToLeft();
                if (L2res.GetBit(i + N2) == 1)
                {
                    state.SetBit((uint)state.size - 1);
                }

            }
            Console.WriteLine($"L2 analyzed, find {result.Count} variants");
            return result;
        }

        public (uint L1, uint L2, uint L3) AnalyzeL3()
        {

            var L1candidates = AnalyzeL1Parallel();
            var L2candidates = AnalyzeL2Parallel();
            var results = new (uint, uint, uint)[1 << threads];

            Parallel.For(0, 1 << threads, (i, state) =>
            {
                results[i] = proccesL3((uint)i, new(L1candidates), new(L2candidates));
                if (results[i].Item1 != 0)
                {
                    state.Stop();
                }
            });
            for (int i = 0; i < 1 << threads; i++)
            {
                if (results[i].Item1 != 0)
                {
                    return results[i];
                }
            }
            return (0, 0, 0);
        }
        (uint L1, uint L2, uint L3) proccesL3(uint numberOfThread, List<BitSequence> L1, List<BitSequence> L2)
        {
            ulong[] firstL1 = new ulong[L1.Count];
            ulong[] firstL2 = new ulong[L2.Count];
            for (int i = 0; i < L1.Count; i++)
            {
                firstL1[i] = L1[i].sequence[0];
            }
            for (int i = 0; i < L2.Count; i++)
            {
                firstL2[i] = L2[i].sequence[0];
            }
            var x3first = x3.sequence[0];
            ulong firstL3 = 0;
            ulong shift = (1ul << (L3size - threads)) * numberOfThread;
            for (uint i = 0u; i < 64; i++)
            {
                if (L3res.GetBit(i + shift) == 1)
                {
                    firstL3 += (1ul << (int)i);

                }
            }
            for (var i = shift; i < (1ul << (L3size - threads)) * (numberOfThread + 1); i++)
            {
                for (int u = 0; u < L1.Count; u++)
                {
                    for (int v = 0; v < L2.Count; v++)
                    {
                        var temp = ((firstL1[u] & firstL3) ^ ((firstL3 ^ ulong.MaxValue) & firstL2[v]));
                        if (temp != x3first)
                        {
                            continue;
                        }
                        var a = L3res.GetNBits(i, (ulong)sequence.Length);
                        var s = ((L1[u] & a) ^ ((a ^ one) & L2[v])) ^ x3;
                        if (s.Count() == 0)
                        {
                            Console.WriteLine($"Finded in thread {numberOfThread};");
                            return (getCandidate(L1[u], L1size), getCandidate(L2[v], L2size), getCandidate(a, L3size));
                        }
                    }
                }
                firstL3 >>= 1;
                if(L3res.GetBit(i + 64) == 1)
                {
                    firstL3 += 1ul << 63;
                }
            }
            Console.WriteLine($"Finished thread {numberOfThread};");
            return (0, 0, 0);
        }

        public uint getCandidate(BitSequence s, int size)
        {
            uint result = 0;
            for(int i = 0; i < size; i++)
            {
                var bit = s.GetBit((uint)i);
                result += (uint)bit << i;
            }
            return result;
        }
    }
}
