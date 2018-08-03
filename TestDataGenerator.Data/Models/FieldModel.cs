using System;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public interface IFieldModel
    {

    }

    public class FieldModel<T> : IFieldModel
    {
        public int Id { get; set; }

        public Type Type => typeof(T);

        public FieldType FieldType { get; set; } = FieldType.None;

        public T Min { get; set; }

        public T Max { get; set; }

        public FieldModel(FieldType type)
        {
            FieldType = type;
        }
    }
}