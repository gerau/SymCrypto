

using System.Diagnostics;

namespace GeffeGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            LFSR L1 = new(0b1010011, 30);
            LFSR L2 = new(0b1001,31);
            LFSR L3 = new(0b10101111, 32);

            Geffe g = new();

            var limit = L3.currentState;
            var st = new Stopwatch();
            //long i = 0;
            //
            /*do { 
                //Console.Write($"{L2} -> ");
                L3.Step();
                //Console.Write($"{L2} -> {result}");
                //Console.Write('\n');
                i++;
            } while (L3.currentState != limit);
            */

            for(int i = 0; i < 1000; i++)
            {
                Console.Write($"{g} \n");
                var result = g.Step();

                Console.Write($"-> {result}\n");

            }
            st.Start();
            Analyzer.AnalyzeL1();
            //Console.WriteLine(Analyzer.x1);
            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds / 1000);
            //Console.WriteLine(i);
            
        }
    }
}
