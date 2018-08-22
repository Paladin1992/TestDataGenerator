using System;
using System.Collections.Generic;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public abstract class FieldModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; protected set; }

        public FieldType FieldType { get; set; } = FieldType.None;
    }

    public class LastNameFieldModel : FieldModel
    {
        public LastNameFieldModel()
        {
            Type = typeof(string);
        }
    }

    public class FirstNameFieldModel : FieldModel
    {
        public bool IsMale { get; set; }


        public FirstNameFieldModel()
        {
            Type = typeof(string);
        }
    }

    public class DateTimeFieldModel : FieldModel
    {
        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }


        public DateTimeFieldModel()
        {
            Type = typeof(DateTime);
        }
    }

    public class EmailFieldModel : FieldModel
    {
        public EmailFieldModel()
        {
            Type = typeof(string);
        }
    }

    public class TextFieldModel : FieldModel
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public bool MustContainSpace { get; set; }

        public bool MustContainDigit { get; set; }

        public bool MustContainAccutes { get; set; }

        public List<char> MustContainCustom { get; set; }

        public LetterCase LetterCase { get; set; }


        public TextFieldModel()
        {
            Type = typeof(string);
        }
    }

    public class IntegerFieldModel<T> : FieldModel
    {
        public T MinValue { get; set; }

        public T MaxValue { get; set; }


        public IntegerFieldModel()
        {
            if (typeof(T) == typeof(int) || typeof(T) == typeof(uint))
            {
                Type = typeof(T);
            }
            else
            {
                throw new NotSupportedException($"The given '{typeof(T).Name}' type is not supported. Use 'int' or 'uint'.");
            }
        }
    }

    public class LongIntegerFieldModel<T> : FieldModel
    {
        public T MinValue { get; set; }

        public T MaxValue { get; set; }


        public LongIntegerFieldModel()
        {
            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                Type = typeof(T);
            }
            else
            {
                throw new NotSupportedException($"The given '{typeof(T).Name}' type is not supported. Use 'long' or 'ulong'.");
            }
        }
    }

    public class FloatFieldModel : FieldModel
    {
        public float MinValue { get; set; }

        public float MaxValue { get; set; }


        public FloatFieldModel()
        {
            Type = typeof(float);
        }
    }

    public class DoubleFieldModel : FieldModel
    {
        public double MinValue { get; set; }

        public double MaxValue { get; set; }


        public DoubleFieldModel()
        {
            Type = typeof(double);
        }
    }

    public class HashFieldModel : FieldModel
    {
        public int DesiredLength { get; set; }


        public HashFieldModel()
        {
            Type = typeof(string);
        }
    }

    public class GuidFieldModel : FieldModel
    {
        public bool SeparateWithHyphens { get; set; }


        public GuidFieldModel()
        {
            Type = typeof(Guid);
        }
    }

    public class CustomSetFieldModel : FieldModel
    {
        public List<string> Values { get; set; }


        public CustomSetFieldModel()
        {
            Type = typeof(string);
        }
    }
}