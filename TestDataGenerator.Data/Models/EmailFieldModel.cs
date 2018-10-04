namespace TestDataGenerator.Data.Models
{
    public class EmailFieldModel : FieldModel
    {
        public EmailFieldModel() : this(null)
        {

        }

        public EmailFieldModel(string name)
        {
            Type = typeof(string);
            Name = name;
        }
    }
}