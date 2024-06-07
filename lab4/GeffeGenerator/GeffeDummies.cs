using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeffeGenerator
{
    internal class GeffeDummies
    {

        public LFSR L1 = new(0b1001, 25);
        public LFSR L2 = new(0b1000111, 26);
        public LFSR L3 = new(0b100111, 27);
        
    
        public GeffeDummies() { }
        public GeffeDummies(uint L1, uint L2, uint L3)
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
