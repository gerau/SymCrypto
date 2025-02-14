﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GeffeGenerator
{
    public class Analyzer
    {
        public const string sequence = "10010000011000011000001110001100001101100100111000000110100111101010001001100011110110011001111011110010110011011010000101000011100000111111011000011010111000110110100001101111010011111000110111100011110100110010011110001100011001100111001000000111100100010100110101101011010100110110100001000100001000110000011000111101110001011111111001010001100101010100000010010100001000010010110001000110110111010110001110110101000110000110011101111010111101101010111000000111001101100000100100111101110101011100111000110101111110100100011101101001010010111010100010111101001010110010100010111001111000011000010001001000010111111010110101001101111100011010000101001001010111010010011100000010110000010100010010111111110011100010110100011001100110101111100010000001010111100111011000001101100001101010011110000101010011001110000111000101000101001110101100111000000000110010110001100100011100101010101111100110100111010111011001101110111110000101101000000010011010011010101111010100111011110000000000101101011011101001000010110110011011101100110111101001001011001011011111010001000011101001011010011111000111011111100101010100110011110110111010000000010001010011000100110101100011110101110111111001001111111101010011100111001100000011010101101000100011101111101000100101001111110000101001111010100110011010101101101100000011011101100111010100100110100100001110010010110010001000010100010111111100011010000110100001000000010110111110011111100000011000100011010111110001100001111011101000001001001110111001001110011110100001011001010011011000111000010000100010010100110101110000000111010110001111101010001011001101010011000010000001000000110011010101011011101001110100000111110100100011110011110000011100001111101111110101011101101100101000001100001111111000000010110011010100101011010000011110010010111010111010101101001001000011000110001100001010111010110011011110010000111010010011010011100100110001100100001000101110011011000111001101110110001001000100010010010001110001000011111111011100100110011110100111101000111100100101000001101111000110110010100110010110";

        public const int C1 = 81;
        public const int C2 = 83;

        public const int N1 = 258;
        public const int N2 = 265;
        public const int threads = 7;

        public static BitSequence one = BitSequence.GetOnes((ulong)sequence.Length);

        public static BitSequence x1 = GetBitArrayResult(N1);
        public static BitSequence x2 = GetBitArrayResult(N2);
        public static BitSequence x3 = GetBitArrayResult(sequence.Length);

        public static int L1size = 30;
        public static int L2size = 31;
        public static int L3size = 32;

        public BitSequence L1res = new((1ul << L1size) + (ulong)sequence.Length + (ulong)L1size);
        public BitSequence L2res = new((1ul << L2size) + (ulong)sequence.Length + (ulong)L2size);
        public BitSequence L3res = new((1ul << L3size) + (ulong)sequence.Length + (ulong)L3size);

        public Analyzer()
        {

            LFSR L1 = new LFSR(0b1010011, 1, 30);
            for (ulong i = 0; i < L1res.size; i++)
            {
                if (L1.Step() == 1)
                {
                    L1res.SetBit(i);
                }
            }
            Console.WriteLine("L1 initialized");
            LFSR L2 = new(0b1001, 1, 31);
            for (ulong i = 0; i < L2res.size; i++)
            {
                if (L2.Step() == 1)
                {
                    L2res.SetBit(i);
                }
            }
            Console.WriteLine("L2 initialized");
            LFSR L3 = new(0b10101111, 1, 32);
            for (ulong i = 0; i < L3res.size; i++)
            {
                if (L3.Step() == 1)
                {
                    L3res.SetBit(i);
                }
            }
            Console.WriteLine("L3 initialized");
        }
        public static BitSequence GetBitArrayResult(int size)
        {
            BitSequence bitArray = new BitSequence((ulong)size);
            for (int i = 0; i < size; i++)
            {
                if (sequence[i] == '1')
                {
                    bitArray.SetBit((ulong)i);
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
            for (var i = 0ul; i < (1ul << L1size); i++)
            {
                var R1 = (state ^ x1).Count();
                if (R1 < C1)
                {
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
            for (var i = 0ul; i < (1ul << L2size); i++)
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var L1candidates = AnalyzeL1Parallel();
            stopwatch.Stop();
            Console.WriteLine($"Time taken to analyze L1: {stopwatch.Elapsed.Minutes.ToString().PadLeft(2,'0')}:{stopwatch.Elapsed.Seconds.ToString().PadLeft(2, '0')}");
            stopwatch.Restart();
            var L2candidates = AnalyzeL2Parallel();
            stopwatch.Stop();
            Console.WriteLine($"Time taken to analyze L2: {stopwatch.Elapsed.Minutes.ToString().PadLeft(2, '0')}:{stopwatch.Elapsed.Seconds.ToString().PadLeft(2, '0')}");

            var results = new (uint, uint, uint)[1u << threads];
            Parallel.For(0, 1u << threads, (i,state) =>
            {
                results[i] = proccesL3((uint)i, new(L1candidates), new(L2candidates));
                if ((results[i].Item1 != 0))
                {
                    state.Stop();
                }
            });

            for (int i = 0; i < 1u << threads; i++)
            {
                if ((results[i].Item1 != 0)
                    & (results[i].Item2 != 0)
                    & (results[i].Item3 != 0))
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
                if (L3res.GetBit(i + 64) == 1)
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
            for (int i = 0; i < size; i++)
            {
                var bit = s.GetBit((uint)i);
                result += (uint)bit << i;
            }
            return result;
        }
    } 
}


