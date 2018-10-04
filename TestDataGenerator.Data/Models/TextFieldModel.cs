using System.Collections.Generic;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public class TextFieldModel : FieldModel
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public LetterCase LetterCase { get; set; }

        public bool MustContainLetters { get; set; }

        public bool MustContainDigit { get; set; }

        public List<char> MustContainCustom { get; set; }


        public TextFieldModel() : this(null, 0, int.MaxValue)
        {

        }

        public TextFieldModel(
            string name,
            int minLength,
            int maxLength,
            bool mustContainLetters = true,
            bool mustContainDigit = false,
            List<char> mustContainCustom = null,
            LetterCase letterCase = LetterCase.Ignore)
        {
            Type = typeof(string);
            Name = name;
            MinLength = minLength;
            MaxLength = maxLength;
            MustContainLetters = mustContainLetters;
            LetterCase = letterCase;
            MustContainDigit = mustContainDigit;
            MustContainCustom = mustContainCustom;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, "
                + $"{nameof(MinLength)}: {MinLength}, "
                + $"{nameof(MaxLength)}: {MaxLength}, "
                + $"{nameof(MustContainLetters)}: {MustContainLetters}, "
                + $"{nameof(LetterCase)}: {LetterCase}, "
                + $"{nameof(MustContainDigit)}: {MustContainDigit}, "
                + $"{nameof(MustContainCustom)}.Count: {MustContainCustom.Count}";
        }
    }
}