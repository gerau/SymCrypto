using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffineCipher
{
    public class LanguageIdentifier
    {
        public static List<string> bannedBigrams = [
            "аы",
            "аь",
            "бй",
            "бф",
            "вй",
            "гй",
            "дй",
            "еы",
            "жй",
            "жщ",
            "жы",
            "зй",
            "иы",
            "йы",
            "йь",
            "кы",
            "кь",
            "мй",
            "оы",
            "пз",
            "пй",
            "пх",
            "сй",
            "уы",
            "уь",
            "фй",
            "фц",
            "фщ",
            "хы",
            "цж",
            "цй",
            "цф",
            "цщ",
            "ць",
            "цю",
            "чй",
            "чф",
            "чщ",
            "чы",
            "чю",
            "шй",
            "шф",
            "шщ",
            "шы",
            "щб",
            "щг",
            "щж",
            "щз",
            "щй",
            "щк",
            "щс",
            "щт",
            "щх",
            "щц",
            "щч",
            "щш",
            "щщ",
            "яь"];


       


        public static bool CheckBannedBigrams(string text, double limit = 0.95)
        {
            double sum = 0;

            var bigramsOfText = TextAnalyzer.BigramFrequenciesOfString(text,2);
            bigramsOfText = (from item in bigramsOfText
                             orderby item.Value
                             descending
                             select item).Where(x => x.Value != 0).
                             ToDictionary();
            foreach(var item in bannedBigrams)
            {
                sum += bigramsOfText.ContainsKey(item) ? 1 : 0;
            }
            double average = sum / bannedBigrams.Count;
            return average <= 1  - limit;
        }
        public static double GetBannedBigrams(string text)
        {
            double sum = 0;
            var bigramsOfText = TextAnalyzer.BigramFrequenciesOfString(text, 2);
            bigramsOfText = (from item in bigramsOfText
                             orderby item.Value
                             descending
                             select item).Where(x => x.Value != 0).
                             ToDictionary();
            foreach (var item in bannedBigrams)
            {
                sum += bigramsOfText.ContainsKey(item) ? 1 : 0;
            }
            double average = sum / bannedBigrams.Count;
            return average;
        }



        public static bool CheckEntropy(string text, double epsilon = 0.1)
        {
            double textEntropy = TextAnalyzer.LetterEntropyOfString(text);
            return Math.Abs(textEntropy - LanguageConstants.LetterEntropy) < epsilon;
        }

        public static bool CheckIndex(string text, double epsilon = 0.1)
        {
            double textIndex = TextAnalyzer.IndexOfCoincedenceOfString(text);
            return Math.Abs(textIndex - LanguageConstants.IndexOfLanguage) < epsilon;
        }
    }


}
