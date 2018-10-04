namespace TestDataGenerator.Data.Models
{
    public class LastNameFieldModel : FieldModel
    {
        public LastNameFieldModel() : this(null)
        {

        }

        public LastNameFieldModel(string name)
        {
            Type = typeof(string);
            Name = name;
        }
    }
}