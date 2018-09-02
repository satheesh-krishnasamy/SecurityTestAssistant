namespace SecurityTestAssistant.Library.Utils
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public static class PropertyDictionaryConverter
    {
        public static IDictionary<string, string> ToDictionary(this object source)
        {
            var result = new Dictionary<string, string>();
            if (source == null)
            {
                return result;
            }

            var properties = TypeDescriptor.GetProperties(source);
            foreach (PropertyDescriptor property in properties)
            {
                var value = property.GetValue(source);
                if (value != null)
                {
                    result.Add(property.Name, value.ToString());
                }
            }

            return result;
        }
    }
}
