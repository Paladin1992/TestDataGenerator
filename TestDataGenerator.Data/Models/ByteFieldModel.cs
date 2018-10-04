namespace TestDataGenerator.Data.Models
{
    public class ByteFieldModel : FieldModel
    {
        public byte MinValue { get; set; }

        public byte MaxValue { get; set; }


        public ByteFieldModel() : this(null)
        {

        }

        public ByteFieldModel(string name, byte minValue = byte.MinValue, byte maxValue = byte.MaxValue)
        {
            Type = typeof(byte);
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