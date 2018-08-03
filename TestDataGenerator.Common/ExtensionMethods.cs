using System.Collections.Generic;

namespace TestDataGenerator.Common
{
    public static class ExtensionMethods
    {
        public static Dictionary<string, string> ToDictionary<TModel>(this TModel model) where TModel : class
        {
            // retrieve all public properties from the given model
            var properties = model.GetType().GetProperties();

            // get placeholder key-value pairs, e.g.: (Key: "FullName", Value: "Gipsz Jakab")
            var placeholders = new Dictionary<string, string>();
            for (int i = 0; i < properties.Length; i++)
            {
                var propName = properties[i].Name;
                var propValue = properties[i].GetValue(model, null)?.ToString();

                placeholders.Add(propName, propValue);
            }

            return placeholders;
        }
    }
}