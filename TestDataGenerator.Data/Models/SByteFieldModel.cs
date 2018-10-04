namespace TestDataGenerator.Data.Models
{
    public class SByteFieldModel : FieldModel
    {
        public sbyte MinValue { get; set; }

        public sbyte MaxValue { get; set; }


        public SByteFieldModel() : this(null)
        {

        }

        public SByteFieldModel(string name, sbyte minValue = sbyte.MinValue, sbyte maxValue = sbyte.MaxValue)
        {
            Type = typeof(sbyte);
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