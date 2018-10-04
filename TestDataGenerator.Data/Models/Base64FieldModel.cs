namespace TestDataGenerator.Data.Models
{
    public class Base64FieldModel : FieldModel
    {
        public int DesiredLength { get; set; }


        public Base64FieldModel() : this(null, 32)
        {

        }

        public Base64FieldModel(string name, int desiredLength)
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