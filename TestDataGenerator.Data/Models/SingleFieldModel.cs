namespace TestDataGenerator.Data.Models
{
    public class SingleFieldModel : FieldModel
    {
        public float MinValue { get; set; }

        public float MaxValue { get; set; }


        public SingleFieldModel() : this(null)
        {

        }

        public SingleFieldModel(string name, float minValue = float.MinValue, float maxValue = float.MaxValue)
        {
            Type = typeof(float);
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