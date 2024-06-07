using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeffeGenerator
{
    public class BitSequence
    {
        public ulong size;
        public ulong[] sequence;

        public BitSequence(ulong size)
        {
            this.size = size;
            if(size%64 == 0) 
            {
                sequence = new ulong[size/64];
            }
            else
            {
                sequence = new ulong[size / 64 + 1];
            }
        }
        public BitSequence(BitSequence copy)
        {
            this.size = copy.size;

            if (size % 64 == 0)
            {
                sequence = new ulong[size / 64];
            }
            else
            {
                sequence = new ulong[size / 64 + 1];
            }
            for (ulong i = 0; i < (ulong)sequence.Length; i++)
            {
                sequence[i] = copy.sequence[i];
            }
        }

        public static BitSequence operator ^(BitSequence first, BitSequence second)
        {
            BitSequence result = new(first.size);
            for (ulong i = 0; i < (ulong)first.sequence.Length; i++)
            {
                result.sequence[i] = first.sequence[i] ^ second.sequence[i];
            }
            return result;
        }

        public static BitSequence operator &(BitSequence first, BitSequence second)
        {
            BitSequence result = new(first.size);
            for (ulong i = 0; i < (ulong)first.sequence.Length; i++)
            {
                result.sequence[i] = first.sequence[i] & second.sequence[i];
            }
            return result;
        }
        public int Count()
        {
            var sum = 0;
            for(ulong i = 0; i < (ulong)sequence.Length; i++)
            {
                sum += BitOperations.PopCount(sequence[i]);
            }
            return sum;
        }

        public void SetBit(ulong i)
        {
            sequence[i / 64] |= (ulong)1 << ((int)i % 64);
        }

        public void Clear()
        {
            for (ulong i = 0; i <= size / 64; i++)
            {
                sequence[i] = 0;
            }
        }
        public int GetBit(ulong position)
        {
            ulong count = position / 64;
            ulong shift = position % 64;
            return (int)(sequence[count] >> (int)shift & 1);
        }
        public BitSequence GetNBits(ulong position, ulong n)
        {
            ulong count = position / 64;
            int shift = (int)position % 64;
            var N = n / 64;
            BitSequence result = new(n);
        
            for (ulong i = 0; i < (ulong)result.sequence.Length; i++)
            {
                result.sequence[i] += sequence[i + count] >> shift;
                result.sequence[i] += sequence[i + count + 1] << (64 - shift);
            }

            
            return result;
        }
        public BitSequence ShiftToLeft()
        {
            BitSequence result = new BitSequence(size);
            for (ulong i = 0; i < (ulong)result.sequence.Length - 1; i++)
            {
                result.sequence[i] = sequence[i] >> 1 | sequence[i + 1] << 63;
            }
            result.sequence[result.sequence.Length - 1] = sequence[result.sequence.Length - 1] >> 1 ;
            return result;
        }
        public override string ToString()
        {
            var result = "";
            for (ulong i = 0; i < size; i++)
            {
                if (GetBit((ulong)i) == 1)
                {
                    result += "1";
                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }
        public static BitSequence GetOnes(ulong size)
        {
            var result = new BitSequence(size);
            for(ulong i = 0; i < size; i++)
            {
                result.SetBit(i);
            }
            return result;
        }


    }
}
