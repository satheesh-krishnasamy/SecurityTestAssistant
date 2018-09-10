namespace SecurityTestAssistant.Library.Extensions
{
    using System.Collections.Generic;
    using System.Text;

    public static class ExtensionEethods
    {
        public static string ToString(this IEnumerable<string> collection, string seperator = " ")
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in collection)
            {
                result.Append(item + seperator);
            }
            return result.ToString();
        }
        public static string ToString(this IEnumerable<KeyValuePair<string, string>> collection, string keyValueSeparator = " : ", string keyValuePairSeparator = ", ")
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in collection)
            {
                result.Append($"{item.Key} {keyValueSeparator} {item.Value} {keyValuePairSeparator}");
            }
            return result.ToString();
        }
    }
}