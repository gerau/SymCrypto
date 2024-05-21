using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GeffeGenerator
{
    public class LFSR
    {
        public uint mask;
        public uint currentState;
        public int size;

        public LFSR(uint mask, uint currentState, int size)
        {
            this.mask = mask;
            this.currentState = currentState;
            this.size = size;
        }
        public LFSR(uint mask, int size)
        {
            this.mask = mask;
            this.currentState = (uint)1 << (size - 1);
            this.size = size;
        }

        public uint Step()
        {
            uint result = currentState & 1;

            currentState = (uint)((currentState >> 1) ^ ((BitOperations.PopCount(currentState & mask) & 1) << (size-1)));

            return result;
        }

        public override string ToString()
        {
            return currentState.ToString(format:"b").PadLeft(size,'0');
        }


    }
}
