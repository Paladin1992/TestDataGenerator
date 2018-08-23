using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestDataGenerator.Data.Enums;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Services
{
    //public interface IDataGeneratorService
    //{
    //    string GenerateLastName(LastNameFieldModel model);

    //    string GenerateFirstName(FirstNameFieldModel model);

    //    DateTime GenerateDateTime(DateTimeFieldModel model);

    //    string GenerateEmail(EmailFieldModel model);

    //    string GenerateText(TextFieldModel model);

    //    int GenerateSignedInteger(Int32FieldModel model);

    //    uint GenerateUnsignedInteger(UInt32FieldModel model);

    //    long GenerateSignedLongInteger(Int64FieldModel model);

    //    ulong GenerateUnsignedLongInteger(UInt64FieldModel model);

    //    float GenerateFloat(FloatFieldModel model);

    //    double GenerateDouble(DoubleFieldModel model);

    //    string GenerateHash(HashFieldModel model);

    //    string GenerateGuid(GuidFieldModel model);

    //    string GenerateFromCustomSet(CustomSetFieldModel model);
    //}


    public class DataGeneratorService
    {
        private readonly Random _random = new Random();

        private readonly List<string> _maleFirstNames = new List<string>();

        private readonly List<string> _femaleFirstNames = new List<string>();

        private readonly List<string> _lastNames = new List<string>();


        public DataGeneratorService()
        {
            _maleFirstNames = File.ReadAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/datasource/firstnames_hun_male.txt")).ToList();
            _femaleFirstNames = File.ReadAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/datasource/firstnames_hun_female.txt")).ToList();
            _lastNames = File.ReadAllLines(System.Web.Hosting.HostingEnvironment.MapPath("~/datasource/lastnames_hun.txt")).ToList();
        }

        public string GenerateLastName(LastNameFieldModel model)
        {
            return GetRandomItemFrom(_lastNames);
        }

        public string GenerateFirstName(FirstNameFieldModel model)
        {
            return GetRandomItemFrom(model.IsMale ? _maleFirstNames : _femaleFirstNames);
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
            // \w{5,10}@\w{3,6}.(hu|com)
            var beforeAtSign = CreateString(_random.Next(5, 11), LetterCase.LowerCaseOnly, includeLetters: true);
            var afterAtSign = CreateString(_random.Next(3, 7), LetterCase.LowerCaseOnly, includeLetters: true);
            var domain = GetRandomItemFrom(new string[] { "hu", "com", "de", "en" });

            return $"{beforeAtSign}@{afterAtSign}.{domain}";
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

        public int GenerateSignedInteger(Int32FieldModel model)
        {
            var min = Math.Min(model.MinValue, model.MaxValue);
            var max = Math.Max(model.MinValue, model.MaxValue);

            return _random.Next(min, max + 1);
        }

        public uint GenerateUnsignedInteger(UInt32FieldModel model)
        {
            var min = Math.Min(model.MinValue, model.MaxValue);
            var max = Math.Max(model.MinValue, model.MaxValue);

            return (uint)(min + _random.NextDouble() * (max - min + 1));
        }

        public long GenerateSignedLongInteger(Int64FieldModel model)
        {
            var min = Math.Min(model.MinValue, model.MaxValue);
            var max = Math.Max(model.MinValue, model.MaxValue);

            return (long)(min + _random.NextDouble() * (max - min + 1));
        }

        public ulong GenerateUnsignedLongInteger(UInt64FieldModel model)
        {
            var min = Math.Min(model.MinValue, model.MaxValue);
            var max = Math.Max(model.MinValue, model.MaxValue);

            return (ulong)(min + _random.NextDouble() * (max - min + 1));
        }

        public float GenerateFloat(FloatFieldModel model)
        {
            var min = Math.Min(model.MinValue, model.MaxValue);
            var max = Math.Max(model.MinValue, model.MaxValue);

            return (float)(min + _random.NextDouble() * (max - min + 1));
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
            if (model.Items.Count == 0)
            {
                return string.Empty;
            }

            return GetRandomItemFrom(model.Items);
        }


        #region [ Helpers ]

        private T GetRandomItemFrom<T>(IReadOnlyList<T> items)
        {
            if (items.Count == 0)
            {
                throw new IndexOutOfRangeException(
                    "It is not possible to return a random item from an empty collection. "
                    + "Ensure that the collection contains at least one item.");
            }

            return items[_random.Next(items.Count)];
        }

        private char GetRandomChar(
            LetterCase letterCase = LetterCase.Ignore,
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

            var c = GetRandomItemFrom(choosable);

            switch (letterCase)
            {
                case LetterCase.Ignore: return _random.Next(2) == 0 ? char.ToLower(c) : char.ToUpper(c);
                case LetterCase.LowerCaseOnly: return char.ToLower(c);
                case LetterCase.UpperCaseOnly: return char.ToUpper(c);
                default: return char.MinValue;
            }
        }

        private string CreateString(
            int desiredLength,
            LetterCase letterCase = LetterCase.Ignore,
            bool includeLetters = false,
            bool includeSpace = false,
            bool includeDigits = false,
            bool includeAccutes = false,
            List<char> includeCustom = null)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < desiredLength; i++)
            {
                var c = GetRandomChar(
                    letterCase,
                    includeLetters,
                    includeSpace,
                    includeDigits,
                    includeAccutes,
                    includeCustom);

                sb.Append(c);
            }

            return sb.ToString();
        }
        #endregion
    }
}