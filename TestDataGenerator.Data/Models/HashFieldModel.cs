namespace TestDataGenerator.Data.Models
{
    public class HashFieldModel : FieldModel
    {
        public int DesiredLength { get; set; }


        public HashFieldModel() : this(null, 32)
        {

        }

        public HashFieldModel(string name, int desiredLength)
        {
            Type = typeof(string);
            Name = name;
            DesiredLength = desiredLength;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(DesiredLength)}: {DesiredLength}";
        }
    }
}