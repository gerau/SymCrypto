using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AffineCipher
{
    public static class LanguageConstants
    {
        private const string _alphabet = "абвгдежзийклмнопрстуфхцчшщыьэюя";
        public static string alphabet = _alphabet;

        public const float LetterEntropy = 4.450032f;
        public const float IndexOfLanguage = 0.056301452f;

        public static void ChangeAlphabet() 
        {
            if(alphabet!= _alphabet)
            {
                alphabet = _alphabet;
                return;
            }
            alphabet = "абвгдежзийклмнопрстуфхцчшщьыэюя";
        }

    
    }
}
