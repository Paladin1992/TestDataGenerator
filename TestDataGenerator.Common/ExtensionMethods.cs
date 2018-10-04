using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TestDataGenerator.Common
{
    public static class ExtensionMethods
    {
        #region [ Enum ]

        /// <summary>
        /// Returns the first attribute of an enum item that is of type TAttr.
        /// </summary>
        /// <typeparam name="TAttr">The type of the Attribute to get.</typeparam>
        /// <param name="enumVal">The enum item to call the method upon.</param>
        /// <returns></returns>
        public static TAttr GetAttributeOfType<TAttr>(this Enum enumVal) where TAttr : Attribute
        {
            var memInfo = enumVal.GetType().GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttr), false);

            return attributes.Length > 0 ? (TAttr)attributes[0] : null;
        }

        /// <summary>
        /// Converts an enum to dictionary, assuming that its items have a Description attribute.
        /// The key will be the enum item's name and the value will be the description that belongs to that item.
        /// If no Description attribute is found, null is stored as value for that particular item.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert.</typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> EnumWithDescriptionToDictionary<TEnum>() where TEnum : Enum
        {
            // Description cannot be key as it may be null, so the enum items' value will be the key instead
            var result = ((IEnumerable<TEnum>)Enum.GetValues(typeof(TEnum))).ToDictionary(
                            k => (int)Enum.Parse(typeof(TEnum), k.ToString()),
                            v => v.GetAttributeOfType<DescriptionAttribute>()?.Description);

            return result;
        }

        /// <summary>
        /// Converts an enum to dictionary.
        /// The key will be the enum item's name (which can optionally be camelCased via the camelCase parameter)
        /// and the value will be the enum item's value.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert.</typeparam>
        /// <param name="camelCase">If true, the keys will be camelCased; otherwise false.</param>
        /// <returns></returns>
        public static Dictionary<string, int> EnumToDictionary<TEnum>(bool camelCase = false) where TEnum : Enum
        {
            var result = Enum.GetNames(typeof(TEnum)).ToDictionary(
                k => camelCase ? k.ToCamelCase() : k,
                v => (int)Enum.Parse(typeof(TEnum), v));

            return result;
        }

        /// <summary>
        /// Converts an enum to a list of key-value pairs, where key will be the enum item's name
        /// (which can optionally be camelCased via the camelCase parameter) and the value will be the enum item's value.
        /// This method should be preferred to EnumToDictionary in a case when iterating through the enum's items in order is necessary,
        /// or when there's a need to get the index of each element.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert.</typeparam>
        /// <param name="camelCase">If true, the keys will be camelCased; otherwise false.</param>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> EnumToList<TEnum>(bool camelCase = false) where TEnum : Enum
        {
            var enumAsDictionary = EnumToDictionary<TEnum>();
            var result = new List<KeyValuePair<string, int>>();

            foreach (var item in enumAsDictionary)
            {
                result.Add(new KeyValuePair<string, int>(camelCase ? item.Key.ToCamelCase() : item.Key, item.Value));
            }

            return result;
        }

        /// <summary>
        /// Converts an enum to a list of key-value pairs, assuming that its items have a Description attribute.
        /// The key will be the enum item's name and the value will be the description that belongs to that item.
        /// If no Description attribute is found, null is stored as value for that particular item.
        /// </summary>
        /// <typeparam name="TEnum">The enum to convert.</typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> EnumWithDescriptionToList<TEnum>() where TEnum : Enum
        {
            var enumAsDictionary = EnumWithDescriptionToDictionary<TEnum>();
            var result = new List<KeyValuePair<string, int>>();

            foreach (var item in enumAsDictionary)
            {
                result.Add(new KeyValuePair<string, int>(item.Value, item.Key)); // Value = Description, Key = item's value!
            }

            return result;
        }
        #endregion

        #region [ Model ]

        /// <summary>
        /// Converts a model object to dictionary, where keys will be the name of the properties.
        /// </summary>
        /// <typeparam name="TModel">The type of the model to convert. Must be a reference type.</typeparam>
        /// <param name="model">The model itself to convert.</param>
        /// <returns></returns>
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
        #endregion

        #region [ String ]

        /// <summary>
        /// Checks whether each character of the given string is lowercase.
        /// The letterOnly optional parameter sets if letters are accepted only.
        /// For example, if letterOnly is true and a digit character is found, then false is returned.
        /// </summary>
        /// <param name="s">The string itself to check.</param>
        /// <param name="letterOnly">If true, the string is tested whether each of its character is a letter; otherwise false.</param>
        /// <returns></returns>
        public static bool IsLowerCaseOnly(this string s, bool letterOnly = false)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (letterOnly && !char.IsLetter(s[i]))
                {
                    return false;
                }

                if (char.IsUpper(s[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether each character of the given string is uppercase.
        /// The letterOnly optional parameter sets if letters are accepted only.
        /// For example, if letterOnly is true and a digit character is found, then false is returned.
        /// </summary>
        /// <param name="s">The string itself to check.</param>
        /// <param name="letterOnly">If true, the string is tested whether each of its character is a letter; otherwise false.</param>
        /// <returns></returns>
        public static bool IsUpperCaseOnly(this string s, bool letterOnly = false)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (letterOnly && !char.IsLetter(s[i]))
                {
                    return false;
                }

                if (char.IsLower(s[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the given string in camelCase format, which means that the very first character is set to lowercase,
        /// while all the rest remains untouched, e.g. MyVariableName will be myVariableName
        /// </summary>
        /// <param name="text">The text to convert to camelCase.</param>
        /// <returns></returns>
        public static string ToCamelCase(this string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }
        #endregion

        #region [ Numeric ]

        /// <summary>
        /// Keeps the given 8-bit signed integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        /// <returns></returns>
        public static sbyte KeepInRange(this sbyte value, sbyte min, sbyte max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 8-bit unsigned integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static byte KeepInRange(this byte value, byte min, byte max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 16-bit unsigned integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static ushort KeepInRange(this ushort value, ushort min, ushort max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 16-bit signed integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static short KeepInRange(this short value, short min, short max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 32-bit signed integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static int KeepInRange(this int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 32-bit unsigned integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static uint KeepInRange(this uint value, uint min, uint max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 64-bit signed integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static long KeepInRange(this long value, long min, long max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 64-bit unsigned integer value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static ulong KeepInRange(this ulong value, ulong min, ulong max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given single-precision floating-point value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static float KeepInRange(this float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given double-precision floating-point value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static double KeepInRange(this double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Keeps the given 128-bit decimal value in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static decimal KeepInRange(this decimal value, decimal min, decimal max)
        {
            return Math.Min(Math.Max(value, min), max);
        }


        /// <summary>
        /// Checks whether the given 8-bit signed integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        /// <returns></returns>
        public static bool IsInRange(this sbyte value, sbyte min, sbyte max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 8-bit unsigned integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this byte value, byte min, byte max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 16-bit unsigned integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this ushort value, ushort min, ushort max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 16-bit signed integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this short value, short min, short max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 32-bit signed integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this int value, int min, int max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 32-bit unsigned integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this uint value, uint min, uint max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 64-bit signed integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this long value, long min, long max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 64-bit unsigned integer value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this ulong value, ulong min, ulong max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given single-precision floating-point value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this float value, float min, float max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given double-precision floating-point value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this double value, double min, double max)
        {
            return min <= value && value <= max;
        }

        /// <summary>
        /// Checks whether the given 128-bit decimal value is in the range of min and max.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="min">The minimum value that the value parameter must be greater than or equal to.</param>
        /// <param name="max">The maximum value that the value parameter must be less than or equal to.</param>
        public static bool IsInRange(this decimal value, decimal min, decimal max)
        {
            return min <= value && value <= max;
        }

        #endregion
    }
}