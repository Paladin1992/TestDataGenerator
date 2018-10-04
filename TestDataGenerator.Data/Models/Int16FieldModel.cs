namespace TestDataGenerator.Data.Models
{
    public class Int16FieldModel : FieldModel
    {
        public short MinValue { get; set; }

        public short MaxValue { get; set; }


        public Int16FieldModel() : this(null)
        {

        }

        public Int16FieldModel(string name, short minValue = short.MinValue, short maxValue = short.MaxValue)
        {
            Type = typeof(short);
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