using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Helpers
{
    public class StringUtilities
    {
        public static string CustomToUpper(string value) => value.ToUpper();

        public static string FirstCharToUpper(string input) => input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };

        public static string[] SeparateString(string input)
        {
            string[] words = Regex.Split(input, @"(?<=[.:…])");
            if (words.Length == 1 || words.Length == 0)
            {
                string[] allWords = input.Split(' ');
                string part1 = string.Empty;
                string part2 = string.Empty;
                for (int i = 0; i < (int)(allWords.Length / 2); i++)
                {
                    part1 += allWords[i] + " ";
                }
                for (int i = (int)(allWords.Length / 2); i < allWords.Length; i++)
                {
                    part2 += allWords[i] + " ";
                }
                List<string> list = new List<string>();
                list.Add(part1);
                list.Add(part2);
                words = list.ToArray();
            }
            return words;
        }

        public static string Translit(string source)
        {
            StringBuilder ret = new StringBuilder();
            string[] cyr = { " ", "+", "-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g", "H", "h", "I", "i", "J", "j", "K", "k", "L", "l", "M", "m", "N", "n", "O", "o", "P", "p", "Q", "q", "R", "r", "S", "s", "T", "t", "U", "u", "V", "v", "W", "w", "X", "x", "Y", "y", "Z", "z", "А", "а", "Б", "б", "В", "в", "Г", "г", "Ґ", "ґ", "Д", "д", "Е", "е", "Є", "є", "Ж", "ж", "З", "з", "И", "и", "І", "і", "Ї", "ї", "Й", "й", "К", "к", "Л", "л", "М", "м", "Н", "н", "О", "о", "П", "п", "Р", "р", "С", "с", "Т", "т", "У", "у", "Ф", "ф", "Х", "х", "Ц", "ц", "Ч", "ч", "Ш", "ш", "Щ", "щ", "Ю", "ю", "Я", "я" };
            string[] lat = { "_", "+", "-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "A", "a", "B", "b", "C", "c", "D", "d", "E", "e", "F", "f", "G", "g", "H", "h", "I", "i", "J", "j", "K", "k", "L", "l", "M", "m", "N", "n", "O", "o", "P", "p", "Q", "q", "R", "r", "S", "s", "T", "t", "U", "u", "V", "v", "W", "w", "X", "x", "Y", "y", "Z", "z", "A", "a", "B", "b", "V", "v", "H", "h", "G", "g", "D", "d", "E", "e", "Ye", "ie", "Zh", "zh", "Z", "z", "Y", "y", "I", "i", "Yi", "i", "Y", "y", "K", "k", "L", "l", "M", "m", "N", "n", "O", "o", "P", "p", "R", "r", "S", "s", "T", "t", "U", "u", "F", "f", "Kh", "kh", "Ts", "ts", "Ch", "ch", "Sh", "sh", "Shch", "shch", "Yu", "iu", "Ya", "ia" };

            for (int j = 0; j < source.Length; j++)
            {
                for (int i = 0; i < cyr.Length; i++)
                {
                    if (source.Substring(j, 1) == cyr[i]) ret.Append(lat[i]);
                }
            }

            return ret.ToString();
        }

        public static string DateToString(DateTime? dt, string format) => dt == null ? "---" : ((DateTime)dt).ToString(format);

        public static string SecondsToMinutes(decimal seconds)
        {
            var minutes = Math.Round(seconds / 60, 0, MidpointRounding.AwayFromZero);
            return minutes + " хв";
        }

        public static String ToHex(System.Drawing.Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

        public static String ToRGB(System.Drawing.Color c) => $"RGB({c.R},{c.G},{c.B})";

        /// <summary>
        /// Повертає слова у відмінку, залежному від вхідного числа
        /// </summary>
        /// <param name="number">Число від якого залежить слово</param>
        /// <param name="nominativ">Називний відмінок слова. Наприклад, "день"</param>
        /// <param name="genetiv">Родовий відмінок слова. Наприклад, "дня"</param>
        /// <param name="plural">Множина слова. Наприклад, "днів"</param>
        /// <returns></returns>
        public static string NumDeclension(int number, string nominative, string plural, string genitive = null)
        {
            if (string.IsNullOrEmpty(genitive))
            {
                return number == 1 ? nominative : plural;
            }

            var titles = new[] { nominative, plural, genitive };
            var cases = new[] { 2, 0, 1, 1, 1, 2 };

            return titles[number % 100 > 4 && number % 100 < 20 ? 2 : cases[(number % 10 < 5) ? number % 10 : 5]];
        }
    }
}
