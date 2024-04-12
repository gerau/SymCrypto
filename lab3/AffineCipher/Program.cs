

namespace AffineCipher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string prefix = "..\\..\\..\\..\\texts\\";

            var keys = AffineAnalyzer.GetProbableKeys(prefix + "variant_reformated.txt", prefix + "reformated_text.txt", 5);

            var texts = new List<string>();

            var initial_text = File.ReadAllText(prefix + "variant_reformated.txt");

            LanguageConstants.ChangeAlphabet();

            foreach (var key in keys)
            {
                Console.WriteLine($"{key.Item1}, {key.Item2}");
                var probableText = AffineAnalyzer.DecipherString(initial_text, key.Item1, key.Item2);
                if (
                    LanguageIdentifier.CheckBannedBigrams(probableText, 0.95) &&
                    LanguageIdentifier.CheckEntropy(probableText, 0.2) &&
                    LanguageIdentifier.CheckIndex(probableText,0.01)
                    )
                {
                    texts.Add(probableText);
                    File.WriteAllText(prefix + $"deciphered_file_{key.Item1}_{key.Item2}.txt", probableText);
                    Console.WriteLine(probableText[..100]);
                    Console.WriteLine(TextAnalyzer.LetterEntropyOfString(probableText));
                }
            }
        }
    }
}
