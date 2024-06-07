
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace GeffeGenerator
{

    
    public class Program
    {
        public static void FindDummies()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            AnalyzeDummies d = new AnalyzeDummies();
            var result = d.AnalyzeL3();

            GeffeDummies geffe = new GeffeDummies(result.L1, result.L2, result.L3);

            Console.WriteLine("Finded candidats:");
            Console.WriteLine("L1 : {0,8} = {1,27}", result.L1, result.L1.ToString(format: "b").PadLeft(25, '0'));
            Console.WriteLine("L2 : {0,8} = {1,27}", result.L2, result.L2.ToString(format: "b").PadLeft(26, '0'));
            Console.WriteLine("L3 : {0,8} = {1,27}", result.L3, result.L3.ToString(format: "b").PadLeft(27, '0'));

            string r = "";
            for (int i = 0; i < AnalyzeDummies.sequence.Length; i++)
            {

                if (geffe.Step() == 1)
                {
                    r += "1";
                }
                else
                {
                    r += "0";
                }
            }

            Console.WriteLine($"Expected sequence: \n{AnalyzeDummies.sequence}");

            Console.WriteLine($"Finded sequence:  \n{r}");

            Console.WriteLine($"Is equal: {r == AnalyzeDummies.sequence}");
            sw.Stop();
            Console.WriteLine($"Time taken on execution: {sw.Elapsed.Hours.ToString().PadLeft(2, '0')}:{sw.Elapsed.Minutes.ToString().PadLeft(2, '0')}:{sw.Elapsed.Seconds.ToString().PadLeft(2, '0')}");
        }
        public static void Find()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Analyzer d = new Analyzer();
            var result = d.AnalyzeL3();

            Geffe geffe = new Geffe(result.L1, result.L2, result.L3);

            Console.WriteLine("Finded candidats:");
            Console.WriteLine("L1 : {0,8} = {1,27}", result.L1, result.L1.ToString(format: "b").PadLeft(30, '0'));
            Console.WriteLine("L2 : {0,8} = {1,27}", result.L2, result.L2.ToString(format: "b").PadLeft(31, '0'));
            Console.WriteLine("L3 : {0,8} = {1,27}", result.L3, result.L3.ToString(format: "b").PadLeft(32, '0'));

            string r = "";
            for (int i = 0; i < Analyzer.sequence.Length; i++)
            {
                if (geffe.Step() == 1)
                {
                    r += "1";
                }
                else
                {
                    r += "0";
                }
            }

            Console.WriteLine($"Expected sequence: \n{Analyzer.sequence}");

            Console.WriteLine($"Finded sequence:  \n{r}");

            sw.Stop();
            Console.WriteLine($"Time taken on execution: {sw.Elapsed.Hours.ToString().PadLeft(2, '0')}:{sw.Elapsed.Minutes.ToString().PadLeft(2, '0')}:{sw.Elapsed.Seconds.ToString().PadLeft(2, '0')}");

        }
        static void Main(string[] args)
        { 

            FindDummies();

        }
    }
}
