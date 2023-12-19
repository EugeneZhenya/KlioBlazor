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
            return words;
        }
    }
}
