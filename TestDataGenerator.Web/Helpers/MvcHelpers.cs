using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    }
}