using System;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public abstract class FieldModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; protected set; }

        public FieldType FieldType { get; set; } = FieldType.None;

        public int NullChance { get; set; } = 0;

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Type)}: {Type}, {nameof(FieldType)}: {FieldType}";
        }
    }
}