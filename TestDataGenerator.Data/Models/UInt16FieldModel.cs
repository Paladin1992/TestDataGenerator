namespace TestDataGenerator.Data.Models
{
    public class UInt16FieldModel : FieldModel
    {
        public ushort MinValue { get; set; }

        public ushort MaxValue { get; set; }


        public UInt16FieldModel() : this(null)
        {

        }

        public UInt16FieldModel(string name, ushort minValue = ushort.MinValue, ushort maxValue = ushort.MaxValue)
        {
            Type = typeof(ushort);
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