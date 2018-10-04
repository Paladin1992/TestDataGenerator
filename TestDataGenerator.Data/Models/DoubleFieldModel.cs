namespace TestDataGenerator.Data.Models
{
    public class DoubleFieldModel : FieldModel
    {
        public double MinValue { get; set; }

        public double MaxValue { get; set; }


        public DoubleFieldModel() : this(null)
        {

        }

        public DoubleFieldModel(string name, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            Type = typeof(double);
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