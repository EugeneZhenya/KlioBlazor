using System.Text.RegularExpressions;

namespace KlioBlazor.Helpers
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
            string[] words = Regex.Split(input, @"(?<=[.:])");
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
    }
}
