namespace TestDataGenerator.Services.Models
{
    public class FieldTypeInfo
    {
        public int Value { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool HasMinValue { get; set; }

        public bool HasMaxValue { get; set; }
    }
}