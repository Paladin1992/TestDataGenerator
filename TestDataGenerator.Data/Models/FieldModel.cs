using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public interface IFieldModel
    {

    }

    public class FieldModel<T> : IFieldModel
    {
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
