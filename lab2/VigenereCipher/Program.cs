namespace VigenereCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] keys =
            [
                "об",
                "лак",
                "аиду",
                "тсюга",
                "ичтоделать",
                "санчизесбоярка",
                "любясъешьщипцывздохнет",
            ];

            string prefix = "..\\..\\..\\..\\";
            string outputFile = prefix + "initial_text.txt";


            Reformator.ReformatFile(outputFile, prefix + "reformated_text.txt", false);

            Dictionary<string, float> IndexesOfCoincedence = new Dictionary<string, float>();

            Console.WriteLine(Analyzer.IndexOfCoincedenceOfFile(prefix + "reformated_text.txt").ToString().Replace(',','.'));

            foreach (var key in keys)
            {
                Vigenere.CipherFile(prefix + "reformated_text.txt", prefix + $"ciphered_text_{key.Length}.txt", key);
                IndexesOfCoincedence[key] = Analyzer.IndexOfCoincedenceOfFile(prefix + $"ciphered_text_{key.Length}.txt");
            }

            foreach (var key in keys)
            {
                Vigenere.DecipherFile(prefix + $"ciphered_text_{key.Length}.txt", prefix + $"deciphered_text_{key.Length}.txt", key);
            }
            string graphicsString = "";

            foreach (var pair in IndexesOfCoincedence)
            {
                Console.WriteLine($"Index of coincedence of key: {pair.Key} = {pair.Value}");
                graphicsString += $"({pair.Key.ToString().Length}, {pair.Value.ToString().Replace(',','.')}) ";
            }
            Console.WriteLine(graphicsString);
            foreach (var pair in IndexesOfCoincedence)
            {
                Console.WriteLine($"{pair.Key} & {pair.Key.Length} & {pair.Value.ToString().Replace(',','.')} \\\\");
            }

            var numberOfBlocks = Analyzer.FindIndex(prefix + "reformated_variant.txt", 30);
            

            var firstKey = Analyzer.FrequencyAnalysisMethod(prefix + "reformated_variant.txt", numberOfBlocks);
            var secondKey = Analyzer.MoreStableAnalysisMethod(prefix + "reformated_variant.txt", numberOfBlocks);


            Console.WriteLine("Key: " + firstKey);

            for (int i = 2; i < 30; i++)
            {
                Console.WriteLine($"key {i} = " + Analyzer.MoreStableAnalysisMethod(prefix + "reformated_variant.txt", i));
            }

            Vigenere.DecipherFile(prefix + "reformated_variant.txt", prefix + "deciphered_variant.txt", firstKey);
        }
    }
}
