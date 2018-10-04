namespace TestDataGenerator.Data.Models
{
    public class Int32FieldModel : FieldModel
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }


        public Int32FieldModel() : this(null)
        {

        }

        public Int32FieldModel(string name, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            Type = typeof(int);
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