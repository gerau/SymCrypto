using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VigenereCipher
{
    static public class Vigenere
    {
        public const string alphabet = "абвгдежзийклмнопрстуфхцчшщъыьэюя";
        public static char SumOfLetters(char letter, char key)
        {
            int index = alphabet.IndexOf(letter) + alphabet.IndexOf(key);
            index %= alphabet.Length;
            return alphabet[index];
        }

        public static char SubOfLetters(char letter, char key)
        {
            int index = alphabet.IndexOf(letter) - alphabet.IndexOf(key) + alphabet.Length;
            index %= alphabet.Length;
            return alphabet[index];
        }

        public static string CipherString(string initialText, string key)
        {
            string cipheredText = "";
            
            for(int i = 0; i < initialText.Length; i++)
            {
                int index = alphabet.IndexOf(initialText[i]) + alphabet.IndexOf(key[i % key.Length]);
                index %= alphabet.Length;
                cipheredText += alphabet[index];
            }
            return cipheredText;
        }

        public static string DecipherString(string cipheredText, string key)
        {
            string initialText = "";

            for (int i = 0; i < cipheredText.Length; i++)
            {
                int index = alphabet.IndexOf(cipheredText[i]) - alphabet.IndexOf(key[i % key.Length]) + alphabet.Length;
                index %= alphabet.Length;
                initialText += alphabet[index];
            }
            return initialText;
        }

        public static void CipherFile(string inputFile, string outputFile, string key)
        {
            string initialText = "";
            if (File.Exists(inputFile))
            {
                initialText = File.ReadAllText(inputFile);
            }
            initialText = CipherString(initialText, key);
            using(StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.Write(initialText);
            }
        }
        public static void DecipherFile(string inputFile, string outputFile, string key)
        {
            string initialText = "";
            if (File.Exists(inputFile))
            {
                initialText = File.ReadAllText(inputFile);
            }
            initialText = DecipherString(initialText, key);
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.Write(initialText);
            }
        }

    }
}
