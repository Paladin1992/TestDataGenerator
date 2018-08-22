using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TestDataGenerator.Common
{
    public static class ExtensionMethods
    {
        public static TAttr GetAttributeOfType<TAttr>(this Enum enumVal) where TAttr : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttr), false);

            return attributes.Length > 0 ? (TAttr)attributes[0] : null;
        }

        public static Dictionary<int, string> EnumWithDescriptionToDictionary<TEnum>() where TEnum : Enum
        {
            var result = ((IEnumerable<TEnum>)Enum.GetValues(typeof(TEnum))).ToDictionary(
                                k => (int)Enum.Parse(typeof(TEnum), k.ToString()),
                                v => v.GetAttributeOfType<DescriptionAttribute>()?.Description);

            return result;
        }

        //public static Dictionary<string, int> EnumToDictionary<TEnum>() where TEnum : struct
        //{
        //    var result = Enum.GetNames(typeof(TEnum)).ToDictionary(k => k, v => (int)Enum.Parse(typeof(TEnum), v));

        //    return result;
        //}

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