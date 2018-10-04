namespace TestDataGenerator.Data.Models
{
    public class GuidFieldModel : FieldModel
    {
        public bool SeparateWithHyphens { get; set; }


        public GuidFieldModel() : this(null, false)
        {

        }

        public GuidFieldModel(string name, bool separateWithHyphens)
        {
            Type = typeof(string);
            Name = name;
            SeparateWithHyphens = separateWithHyphens;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(SeparateWithHyphens)}: {SeparateWithHyphens}";
        }
    }
}