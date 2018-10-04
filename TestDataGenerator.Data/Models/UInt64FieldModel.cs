namespace TestDataGenerator.Data.Models
{
    public class UInt64FieldModel : FieldModel
    {
        public ulong MinValue { get; set; }

        public ulong MaxValue { get; set; }


        public UInt64FieldModel() : this(null)
        {

        }

        public UInt64FieldModel(string name, ulong minValue = ulong.MinValue, ulong maxValue = ulong.MaxValue)
        {
            Type = typeof(ulong);
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