using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestDataGenerator.Data.Enums;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Services
{
    public interface IDataGeneratorService
    {
        string GenerateLastName(LastNameFieldModel model);

        string GenerateFirstName(FirstNameFieldModel model);

        DateTime GenerateDateTime(DateTimeFieldModel model);

        string GenerateEmail(EmailFieldModel model);

        string GenerateText(TextFieldModel model);

        int GenerateSignedInteger(IntegerFieldModel<int> model);

        uint GenerateUnsignedInteger(IntegerFieldModel<uint> model);

        long GenerateSignedLongInteger(LongIntegerFieldModel<long> model);

        ulong GenerateUnsignedLongInteger(LongIntegerFieldModel<ulong> model);

        float GenerateFloat(FloatFieldModel model);

        double GenerateDouble(DoubleFieldModel model);

        string GenerateHash(HashFieldModel model);

        string GenerateGuid(GuidFieldModel model);

        string GenerateFromCustomSet(CustomSetFieldModel model);
    }

    public class DataGeneratorService : IDataGeneratorService
    {
        private readonly Random _random = new Random();

        private readonly List<string> _maleFirstNames = new List<string>();

        private readonly List<string> _femaleFirstNames = new List<string>();

        private readonly List<string> _lastNames = new List<string>();


        public DataGeneratorService()
        {
            _maleFirstNames = File.ReadAllLines("firstnames_hun_male.txt").ToList();
            _femaleFirstNames = File.ReadAllLines("firstnames_hun_female.txt").ToList();
            _lastNames = File.ReadAllLines("lastnames_hun.txt").ToList();
        }

        public string GenerateLastName(LastNameFieldModel model)
        {
            return _lastNames[_random.Next(0, _lastNames.Count)];
        }

        public string GenerateFirstName(FirstNameFieldModel model)
        {
            if (model.IsMale)
            {
                return _maleFirstNames[_random.Next(0, _maleFirstNames.Count)];
            }
            else
            {
                return _femaleFirstNames[_random.Next(0, _femaleFirstNames.Count)];
            }
        }

        public DateTime GenerateDateTime(DateTimeFieldModel model)
        {
            var minDate = model.MinDate <= model.MaxDate ? model.MinDate : model.MaxDate;
            var maxDate = model.MaxDate >= model.MinDate ? model.MaxDate : model.MinDate;

            var year = _random.Next(minDate.Year, maxDate.Year + 1);
            var month = _random.Next(minDate.Month, maxDate.Month + 1);
            var day = _random.Next(minDate.Day, maxDate.Day + 1);

            var hour = _random.Next(minDate.Hour, maxDate.Hour + 1);
            var minute = _random.Next(minDate.Minute, maxDate.Minute + 1);
            var second = _random.Next(minDate.Second, maxDate.Second + 1);

            return new DateTime(year, month, day, hour, minute, second);
        }

        public string GenerateEmail(EmailFieldModel model)
        {
            throw new NotImplementedException();
        }

        public string GenerateText(TextFieldModel model)
        {
            var sb = new StringBuilder();

            for (int i = model.MinLength; i <= model.MaxLength; i++)
            {
                var c = GetRandomChar(
                    letterCase: model.LetterCase,
                    includeLetters: true,
                    includeSpace: model.MustContainSpace,
                    includeDigits: model.MustContainDigit,
                    includeAccutes: model.MustContainAccutes,
                    includeCustom: model.MustContainCustom);

                sb.Append(c);
            }

            return sb.ToString();
        }

        public int GenerateSignedInteger(IntegerFieldModel<int> model)
        {
            throw new NotImplementedException();
        }

        public uint GenerateUnsignedInteger(IntegerFieldModel<uint> model)
        {
            throw new NotImplementedException();
        }

        public long GenerateSignedLongInteger(LongIntegerFieldModel<long> model)
        {
            throw new NotImplementedException();
        }

        public ulong GenerateUnsignedLongInteger(LongIntegerFieldModel<ulong> model)
        {
            throw new NotImplementedException();
        }

        public float GenerateFloat(FloatFieldModel model)
        {
            return (float)(model.MinValue + _random.NextDouble() * (model.MaxValue - model.MinValue + 1));
        }

        public double GenerateDouble(DoubleFieldModel model)
        {
            var min = Math.Min(model.MinValue, model.MaxValue);
            var max = Math.Max(model.MinValue, model.MaxValue);

            return min + _random.NextDouble() * (max - min + 1);
        }        

        public string GenerateHash(HashFieldModel model)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < model.DesiredLength; i++)
            {
                var c = GetRandomChar(
                    LetterCase.LowerCaseOnly,
                    includeDigits: true,
                    includeCustom: new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f' });

                sb.Append(c);
            }

            return sb.ToString();
        }

        public string GenerateGuid(GuidFieldModel model)
        {
            return Guid.NewGuid().ToString(model.SeparateWithHyphens ? "D" : "N");
        }

        public string GenerateFromCustomSet(CustomSetFieldModel model)
        {
            if (model.Values.Count == 0)
            {
                return string.Empty;
            }

            return model.Values[_random.Next(0, model.Values.Count)];
        }


        #region [ Helpers ]

        private char GetRandomChar(
            LetterCase letterCase,
            bool includeLetters = false,
            bool includeSpace = false,
            bool includeDigits = false,
            bool includeAccutes = false,
            List<char> includeCustom = null)
        {
            var choosable = new List<char>();

            if (includeLetters)
            {
                for (char i = 'a'; i <= 'z'; i++)
                {
                    choosable.Add(i);
                }
            }

            if (includeSpace)
            {
                choosable.Add(' ');
            }

            if (includeDigits)
            {
                for (int i = 0; i <= 9; i++)
                {
                    choosable.Add(Convert.ToChar(i.ToString()));
                }
            }

            if (includeAccutes)
            {
                choosable.AddRange(new List<char>() { 'á', 'é', 'í', 'ó', 'ö', 'ő', 'ú', 'ü', 'ű'});
            }

            if (includeCustom != null && includeCustom.Count > 0)
            {
                choosable.AddRange(includeCustom);
            }

            choosable = choosable.Distinct().ToList();

            var c = choosable[_random.Next(0, choosable.Count)];

            switch (letterCase)
            {
                case LetterCase.Ignore: return _random.Next(2) == 0 ? char.ToLower(c) : char.ToUpper(c);
                case LetterCase.LowerCaseOnly: return char.ToLower(c);
                case LetterCase.UpperCaseOnly: return char.ToUpper(c);
                default: return char.MinValue;
            }
        }

        #endregion
    }
}