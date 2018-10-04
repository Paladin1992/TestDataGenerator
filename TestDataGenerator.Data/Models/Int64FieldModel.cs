namespace TestDataGenerator.Data.Models
{
    public class Int64FieldModel : FieldModel
    {
        public long MinValue { get; set; }

        public long MaxValue { get; set; }


        public Int64FieldModel() : this(null)
        {

        }

        public Int64FieldModel(string name, long minValue = long.MinValue, long maxValue = long.MaxValue)
        {
            Type = typeof(long);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }
}