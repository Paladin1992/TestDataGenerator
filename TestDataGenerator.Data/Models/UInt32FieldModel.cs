namespace TestDataGenerator.Data.Models
{
    public class UInt32FieldModel : FieldModel
    {
        public uint MinValue { get; set; }

        public uint MaxValue { get; set; }


        public UInt32FieldModel() : this(null)
        {

        }

        public UInt32FieldModel(string name, uint minValue = uint.MinValue, uint maxValue = uint.MaxValue)
        {
            Type = typeof(uint);
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