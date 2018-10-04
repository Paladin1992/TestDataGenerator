using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestDataGenerator.Web.Helpers
{
    public static class MvcHelpers
    {
        public static IHtmlString RawJsonModel(this HtmlHelper html, object model)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy.MM.dd" };
            return html.Raw(JsonConvert.SerializeObject(model, dateTimeConverter));
        }

        public static string EnumDictionaryToJS(string enumName, List<KeyValuePair<string, int>> enumAsList)
        {
            /*
             enumAsList =
             [
                { Key: "item1Name", Value: item1Value },
                { Key: "item2Name", Value: item2Value },
                :
                { Key: "itemNName", Value: itemNValue }
             ]
             
             result = "enumName: { item1Name: item1Value, item2Name: item2Value, ... , itemNName: itemNValue }"
            */

            string itemsAsString = string.Join(", ", enumAsList.OrderBy(x => x.Value).Select(x => $"{x.Key}: {x.Value}"));
            string result = $"{enumName}: {{ {itemsAsString} }}";

            return result;
        }

        public static string GetNumberTypeExtremes(bool useTrailingComma = false)
        {
            var nfi = new NumberFormatInfo
            {
                NumberDecimalSeparator = "."
            };

            var items = new List<KeyValuePair<string, (string minValue, string maxValue)>>
            {
                new KeyValuePair<string, (string, string)>(
                    typeof(sbyte).Name.ToLower(),
                    (sbyte.MinValue.ToString(nfi), sbyte.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(byte).Name.ToLower(),
                    (byte.MinValue.ToString(nfi), byte.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(short).Name.ToLower(),
                    (short.MinValue.ToString(nfi), short.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(ushort).Name.ToLower(),
                    (ushort.MinValue.ToString(nfi), ushort.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(int).Name.ToLower(),
                    (int.MinValue.ToString(nfi), int.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(uint).Name.ToLower(),
                    (uint.MinValue.ToString(nfi), uint.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(long).Name.ToLower(),
                    (long.MinValue.ToString(nfi), long.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(ulong).Name.ToLower(),
                    (ulong.MinValue.ToString(nfi), ulong.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(float).Name.ToLower(),
                    (float.MinValue.ToString(nfi), float.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(double).Name.ToLower(),
                    (double.MinValue.ToString(nfi), double.MaxValue.ToString(nfi))),

                new KeyValuePair<string, (string, string)>(
                    typeof(decimal).Name.ToLower(),
                    (decimal.MinValue.ToString(nfi), decimal.MaxValue.ToString(nfi)))
            };

            IEnumerable<string> formattedItems = items.Select(x => $"{x.Key}: {{ minValue: \"{x.Value.minValue}\", maxValue: \"{x.Value.maxValue}\" }}");
            string result = string.Join(",\r\n\r\n", formattedItems) + (useTrailingComma ? "," : "");

            return result;
        }
    }
}