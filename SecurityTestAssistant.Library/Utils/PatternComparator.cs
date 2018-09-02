
namespace SecurityTestAssistant.Library.Utils
{
    using SecurityTestAssistant.Library.Config;
    using System;
    using System.Text.RegularExpressions;

    public static class PatternComparator
    {
        public static bool AreMacthesFound(
            string input,
            PatternBase pattern)
        {
            bool AreMatchesWithPattern = false;
            switch (pattern.PatternType)
            {
                case PatternMatchType.StartsWith:
                    AreMatchesWithPattern = input.StartsWith(pattern.PatternText, StringComparison.InvariantCultureIgnoreCase);
                    break;
                case PatternMatchType.RegEx:
                    AreMatchesWithPattern = Regex.IsMatch(input, pattern.PatternText);
                    break;
                case PatternMatchType.PresentsAnywhere:
                    AreMatchesWithPattern = input.ToLower().IndexOf(pattern.PatternText.ToLower(), 0) >= 0;
                    break;
            }

            return AreMatchesWithPattern;
        }
    }
}
