using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeffeGenerator
{
    public class Geffe
    {
        public LFSR L1 = new(0b1010011, 30);
        public LFSR L2 = new(0b1001, 31);
        public LFSR L3 = new(0b10101111, 32);

        public Geffe() { }
        public Geffe(uint L1, uint L2, uint L3) 
        {
            this.L1.currentState = L1;
            this.L2.currentState = L2;
            this.L3.currentState = L3;
        }

        public uint Step()
        {
            uint x = L1.Step();
            uint y = L2.Step();
            uint s = L3.Step();

            return (s & x) ^ ((s ^ 1) & y);
        }
        public override string ToString()
        {
            return string.Join('\n', [$"L1: {L1}", $"L2: {L2}", $"L3: {L3}"]);
        }
    }
}
