using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeffeGenerator
{
    public class BitSequence
    {
        public int size;
        public ulong[] sequence;

        public BitSequence(int size)
        {
            this.size = size;
            sequence = new ulong[size/64 + 1];
        }

        public static BitSequence operator ^(BitSequence first, BitSequence second)
        {
            BitSequence result = new(first.size);
            for (int i = 0; i <= first.size / 64; i++)
            {
                result.sequence[i] = first.sequence[i] ^ second.sequence[i];
            }
            return result;
        }

        public static BitSequence operator &(BitSequence first, BitSequence second)
        {
            BitSequence result = new(first.size);
            for (int i = 0; i <= first.size / 64; i++)
            {
                result.sequence[i] = first.sequence[i] & second.sequence[i];
            }
            return result;
        }
        public int Count()
        {
            var sum = 0;
            for(int i = 0; i <= size / 64; i++)
            {
                sum += BitOperations.PopCount(sequence[i]);
            }
            return sum;
        }

        public void SetBit(int i)
        {
            sequence[i / 64] |= (ulong)1 << (i % 64);
        }

        public void Clear()
        {
            for (int i = 0; i <= size / 64; i++)
            {
                sequence[i] = 0;
            }
        }
        public int GetBit(int position)
        {
            int count = position / 64;
            int shift = position % 64;
            return (int)(sequence[count] >> shift & 1);
        }
        public override string ToString()
        {
            var result = "";
            for (int i = 0; i < size; i++)
            {
                if (GetBit(i) == 1)
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

    }
}
