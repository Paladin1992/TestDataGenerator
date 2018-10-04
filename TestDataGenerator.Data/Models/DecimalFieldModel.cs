namespace TestDataGenerator.Data.Models
{
    public class DecimalFieldModel : FieldModel
    {
        public decimal MinValue { get; set; }

        public decimal MaxValue { get; set; }


        public DecimalFieldModel() : this(null)
        {

        }

        public DecimalFieldModel(string name, decimal minValue = decimal.MinValue, decimal maxValue = decimal.MaxValue)
        {
            Type = typeof(decimal);
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