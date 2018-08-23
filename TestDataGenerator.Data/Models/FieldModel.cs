using System;
using System.Collections.Generic;
using System.Globalization;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public abstract class FieldModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; protected set; }

        public FieldType FieldType { get; set; } = FieldType.None;

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Type)}: {Type}, {nameof(FieldType)}: {FieldType}";
        }
    }


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

    public class FirstNameFieldModel : FieldModel
    {
        public bool IsMale { get; set; }


        public FirstNameFieldModel() : this(null, false)
        {
            
        }

        public FirstNameFieldModel(string name, bool isMale)
        {
            Type = typeof(string);
            Name = name;
            IsMale = isMale;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(IsMale)}: {IsMale}";
        }
    }

    public class DateTimeFieldModel : FieldModel
    {
        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }


        public DateTimeFieldModel() : this(null, DateTime.MinValue, DateTime.MaxValue)
        {
            
        }

        public DateTimeFieldModel(string name, DateTime minDate, DateTime maxDate)
        {
            Type = typeof(DateTime);
            Name = name;
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinDate)}: {MinDate.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture)}, "
                + $"{nameof(MaxDate)}: {MaxDate.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture)}";
        }
    }

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

    public class TextFieldModel : FieldModel
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public LetterCase LetterCase { get; set; }

        public bool MustContainSpace { get; set; }

        public bool MustContainDigit { get; set; }

        public bool MustContainAccutes { get; set; }

        public List<char> MustContainCustom { get; set; }


        public TextFieldModel() : this(null, 0, int.MaxValue)
        {
            
        }

        public TextFieldModel(
            string name,
            int minLength,
            int maxLength,
            LetterCase letterCase = LetterCase.Ignore,
            bool mustContainSpace = false,
            bool mustContainDigit = false,
            bool mustContainAccutes = false,
            List<char> mustContainCustom = null)
        {
            Type = typeof(string);
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
            LetterCase = letterCase;
            MustContainSpace = mustContainSpace;
            MustContainDigit = mustContainDigit;
            MustContainAccutes = mustContainAccutes;
            MustContainCustom = mustContainCustom;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinLength)}: {MinLength}, "
                + $"{nameof(MaxLength)}: {MaxLength}, "
                + $"{nameof(MustContainSpace)}: {MustContainSpace}, "
                + $"{nameof(MustContainDigit)}: {MustContainDigit}, "
                + $"{nameof(MustContainAccutes)}: {MustContainAccutes}, "
                + $"{nameof(MustContainCustom)}: {MustContainCustom.Count}, "
                + $"{nameof(LetterCase)}: {LetterCase}";
        }
    }

    public class Int32FieldModel : FieldModel
    {
        public int MinValue { get; set; }

        public int MaxValue { get; set; }


        public Int32FieldModel() : this(null, int.MinValue, int.MaxValue)
        {

        }

        public Int32FieldModel(string name, int minValue, int maxValue)
        {
            Type = typeof(int);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }

    public class UInt32FieldModel : FieldModel
    {
        public uint MinValue { get; set; }

        public uint MaxValue { get; set; }


        public UInt32FieldModel() : this(null, uint.MinValue, uint.MaxValue)
        {

        }

        public UInt32FieldModel(string name, uint minValue, uint maxValue)
        {
            Type = typeof(uint);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }

    public class Int64FieldModel : FieldModel
    {
        public long MinValue { get; set; }

        public long MaxValue { get; set; }


        public Int64FieldModel() : this(null, long.MinValue, long.MaxValue)
        {
            
        }

        public Int64FieldModel(string name, long minValue, long maxValue)
        {
            Type = typeof(long);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }

    public class UInt64FieldModel : FieldModel
    {
        public ulong MinValue { get; set; }

        public ulong MaxValue { get; set; }


        public UInt64FieldModel() : this(null, ulong.MinValue, ulong.MaxValue)
        {

        }

        public UInt64FieldModel(string name, ulong minValue, ulong maxValue)
        {
            Type = typeof(ulong);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }

    public class FloatFieldModel : FieldModel
    {
        public float MinValue { get; set; }

        public float MaxValue { get; set; }


        public FloatFieldModel() : this(null, float.MinValue, float.MaxValue)
        {
            
        }

        public FloatFieldModel(string name, float minValue, float maxValue)
        {
            Type = typeof(float);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }

    public class DoubleFieldModel : FieldModel
    {
        public double MinValue { get; set; }

        public double MaxValue { get; set; }


        public DoubleFieldModel() : this(null, double.MinValue, double.MaxValue)
        {
            
        }

        public DoubleFieldModel(string name, double minValue, double maxValue)
        {
            Type = typeof(double);
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinValue)}: {MinValue}, "
                + $"{nameof(MaxValue)}: {MaxValue}";
        }
    }

    public class HashFieldModel : FieldModel
    {
        public int DesiredLength { get; set; }


        public HashFieldModel() : this(null, 32)
        {
            
        }

        public HashFieldModel(string name, int desiredLength)
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