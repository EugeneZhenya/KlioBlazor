using System.Text.RegularExpressions;

namespace KlioMobile.Framework.Extensions;

public static class StringExtensions
{
    public static string CleanCacheKey(this string uri) =>
        Regex.Replace((new Regex("[\\~#%&*{}/:<>?|\"-]")).Replace(uri, " "), @"\s+", "_");
}
