using System;
using System.Globalization;

namespace TestDataGenerator.Data.Models
{
    public class DateTimeFieldModel : FieldModel
    {
        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }


        public DateTimeFieldModel() : this(null, DateTime.MinValue, DateTime.MaxValue)
        {

        }

        public DateTimeFieldModel(string name, DateTime minDate, DateTime maxDate)
        {
            Type = typeof(DateTime);
            Name = name;
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinDate)}: {MinDate.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture)}, "
                + $"{nameof(MaxDate)}: {MaxDate.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture)}";
        }
    }
}