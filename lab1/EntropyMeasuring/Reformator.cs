using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EntropyMeasuring
{
    public static class Reformator
    {
        const string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        private static string ReformatString(string s,bool withSpaces) 
        {
            string temp = "";
            foreach (char c in s)
            {
                temp += alphabet.Contains(c) ? c : " ";
            }
            temp = Regex.Replace(temp, @"\s+", withSpaces ? " " : "");
            temp = temp.ToLower();
            return temp;
        }
        public static void ReformatFile(string inputFile, string outputFile,bool withSpaces = true)
        {
            var reader = new StreamReader(inputFile, Encoding.UTF8);
            using StreamWriter sw = new StreamWriter(outputFile);
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                if (line == null) break;
                sw.Write(ReformatString(line, withSpaces));
            }
        }
    }
}
