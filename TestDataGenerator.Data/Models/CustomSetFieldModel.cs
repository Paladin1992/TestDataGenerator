using System.Collections.Generic;

namespace TestDataGenerator.Data.Models
{
    public class CustomSetFieldModel : FieldModel
    {
        public List<string> Items { get; set; } = new List<string>();


        public CustomSetFieldModel()
        {
            Type = typeof(string);
        }

        public CustomSetFieldModel(string name, List<string> items)
        {
            Type = typeof(string);
            Name = name;
            Items = items;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Items)}: {string.Join(", ", Items)}";
        }
    }
}