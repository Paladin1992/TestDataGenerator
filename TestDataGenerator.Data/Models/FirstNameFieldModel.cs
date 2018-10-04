using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public class FirstNameFieldModel : FieldModel
    {
        public Gender Gender { get; set; }


        public FirstNameFieldModel() : this(null)
        {

        }

        public FirstNameFieldModel(string name, Gender gender = Gender.None)
        {
            Type = typeof(string);
            Name = name;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Gender)}: {Gender}";
        }
    }
}