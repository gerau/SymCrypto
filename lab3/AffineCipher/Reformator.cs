using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AffineCipher
{
    public class Reformator
    {

        const string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        public static string ReformatString(string s, bool withSpaces)
        {
            string temp = "";
            foreach (char c in s)
            {
                temp += alphabet.Contains(c) ? c : " ";
            }
            temp = Regex.Replace(temp, @"\s+", withSpaces ? " " : "");

            temp = temp.ToLower();
            temp = temp.TrimStart();
            temp = temp.Replace("ё", "е");
            temp = temp.Replace("ъ", "ь");
            return temp;
        }
        public static void ReformatFile(string inputFile, string outputFile, bool withSpaces = false)
        {
            var reader = new StreamReader(inputFile, Encoding.UTF8);
            using StreamWriter sw = new(outputFile);
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                sw.Write(ReformatString(line, withSpaces));
            }
            sw.Close();
        }
    }
}
