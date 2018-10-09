using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Hosting;
using TestDataGenerator.Common;
using TestDataGenerator.Data.Enums;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Services
{
    public class DataGeneratorService
    {
        private readonly Random _random = new Random();

        private readonly List<string> _maleFirstNames = new List<string>();

        private readonly List<string> _femaleFirstNames = new List<string>();

        private readonly List<string> _lastNames = new List<string>();

        private readonly string[] _emailDomains = new string[] { "hu", "com", "de", "en" };


        /// <summary>
        /// Creates a new instance of the DataGeneratorService.
        /// The testMode parameter specifies whether the test paths are to be used for reading the source files (it is designed for UnitTests).
        /// </summary>
        /// <param name="testMode">If true, the test paths are used for reading the source files; otherwise false.
        /// This option is designed for UnitTests.</param>
        public DataGeneratorService(bool testMode = false)
        {
            if (testMode)
            {
                _maleFirstNames = File.ReadAllLines("datasource/firstnames_hun_male.txt", Encoding.Default).ToList();
                _femaleFirstNames = File.ReadAllLines("datasource/firstnames_hun_female.txt", Encoding.Default).ToList();
                _lastNames = File.ReadAllLines("datasource/lastnames_hun.txt", Encoding.Default).ToList();
            }
            else
            {
                _maleFirstNames = File.ReadAllLines(HostingEnvironment.MapPath("~/datasource/firstnames_hun_male.txt"), Encoding.Default).ToList();
                _femaleFirstNames = File.ReadAllLines(HostingEnvironment.MapPath("~/datasource/firstnames_hun_female.txt"), Encoding.Default).ToList();
                _lastNames = File.ReadAllLines(HostingEnvironment.MapPath("~/datasource/lastnames_hun.txt"), Encoding.Default).ToList();
            }
        }

        /// <summary>
        /// Generates a random last name by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateLastName(LastNameFieldModel model)
        {
            return GetRandomItemFrom(_lastNames);
        }

        /// <summary>
        /// Generates a random first name by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateFirstName(FirstNameFieldModel model)
        {
            switch (model.Gender)
            {
                case Gender.None: return GetRandomItemFrom(_random.Next(2) == 0 ? _maleFirstNames : _femaleFirstNames);
                case Gender.Male: return GetRandomItemFrom(_maleFirstNames);
                case Gender.Female: return GetRandomItemFrom(_femaleFirstNames);
                default: return null;
            }
        }

        /// <summary>
        /// Generates a random date and/or time by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public DateTime GenerateDateTime(DateTimeFieldModel model)
        {
            var minDate = model.MinDate <= model.MaxDate ? model.MinDate : model.MaxDate;
            var maxDate = model.MaxDate >= model.MinDate ? model.MaxDate : model.MinDate;

            if (minDate == maxDate) // a particular date
            {
                // It would be too risky to build up a custom DateTime.MinValue or DateTime.MaxValue
                // via the parameters of the DateTime's constructor because of the smaller time periods' inaccuracy,
                // so we just simply return these constants instead.
                if (minDate == DateTime.MinValue)
                {
                    return DateTime.MinValue;
                }

                if (maxDate == DateTime.MaxValue)
                {
                    return DateTime.MaxValue;
                }

                return minDate;
            }

            var year = _random.Next(minDate.Year, maxDate.Year + 1).KeepInRange(1, 9999);
            var month = _random.Next(minDate.Month, maxDate.Month + 1).KeepInRange(1, 12);
            var day = _random.Next(minDate.Day, maxDate.Day + 1).KeepInRange(1, DateTime.DaysInMonth(year, month));

            var hour = _random.Next(minDate.Hour, maxDate.Hour + 1).KeepInRange(0, 23);
            var minute = _random.Next(minDate.Minute, maxDate.Minute + 1).KeepInRange(0, 59);
            var second = _random.Next(minDate.Second, maxDate.Second + 1).KeepInRange(0, 59);

            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// Generates a random e-mail address by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateEmail(EmailFieldModel model)
        {
            // \w{5,10}@\w{3,6}.(hu|com|...)
            var beforeAtSign = CreateString(_random.Next(5, 11), LetterCase.LowerCaseOnly, includeLetters: true);
            var afterAtSign = CreateString(_random.Next(3, 7), LetterCase.LowerCaseOnly, includeLetters: true);
            var domain = GetRandomItemFrom(_emailDomains);

            return $"{beforeAtSign}@{afterAtSign}.{domain}";
        }

        /// <summary>
        /// Generates a random text by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateText(TextFieldModel model)
        {
            if (model.MaxLength <= 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            var length = _random.Next(model.MinLength, model.MaxLength + 1);

            for (int i = 0; i < length; i++)
            {
                var c = GetRandomChar(
                    letterCase: model.LetterCase,
                    includeLetters: model.MustContainLetters,
                    includeDigits: model.MustContainDigit,
                    includeCustom: model.MustContainCustom);

                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates a random 8-bit signed integer (byte) by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public sbyte GenerateSByte(SByteFieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);
            sbyte result;

            if (a == b)
            {
                return a;
            }

            if (a == sbyte.MinValue && b == sbyte.MaxValue) // get a value from the whole range
            {
                //  a                         b
                //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                // MIN           0           MAX

                if (_random.Next(2) == 0) // non-negative
                {
                    result = (sbyte)(_random.NextDouble() * ((long)b + 1));
                }
                else // negative
                {
                    result = (sbyte)(-_random.NextDouble() * (Math.Abs((long)a) + 1));
                }
            }
            else if (a == sbyte.MinValue && b < sbyte.MaxValue)
            {
                if (b < 0)
                {
                    //  a        b
                    //  |▀▀▀▀▀▀▀▀|---|------------|
                    // MIN           0           MAX

                    result = (sbyte)-(Math.Abs(b) + _random.NextDouble() * (Math.Abs((long)a) - Math.Abs(b) + 1));
                }
                else
                {
                    //  a                  b
                    //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀|------|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (sbyte)(_random.NextDouble() * (Math.Abs(b) + 1));
                    }
                    else // negative
                    {
                        result = (sbyte)-(_random.NextDouble() * (Math.Abs((long)a) + 1));
                    }
                }
            }
            else if (a > sbyte.MinValue && b == sbyte.MaxValue)
            {
                if (a < 0)
                {
                    //       a                    b
                    //  |----|▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (sbyte)(_random.NextDouble() * ((long)b + 1));
                    }
                    else // negative
                    {
                        result = (sbyte)-(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
                else
                {
                    //                  a         b
                    //  |------------|--|▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    result = (sbyte)(a + _random.NextDouble() * ((long)b - a + 1));
                }
            }
            else // if (min > sbyte.MinValue && max < sbyte.MaxValue)
            {
                if (a >= 0 && b >= 0)
                {
                    //                  a      b
                    //  |------------|--|▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    result = (sbyte)(a + _random.NextDouble() * (b - a + 1));
                }
                else if (a <= 0 && b <= 0)
                {
                    //     a      b
                    //  |--|▀▀▀▀▀▀|--|------------|
                    // MIN           0           MAX

                    result = (sbyte)-(Math.Abs(b) + _random.NextDouble() * (Math.Abs(a) - Math.Abs(b) + 1));
                }
                else // a <= 0 && b >= 0
                {
                    //        a                b
                    //  |-----|▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (sbyte)(_random.NextDouble() * (b + 1));
                    }
                    else // negative
                    {
                        result = (sbyte)-(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
            }

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 8-bit unsigned integer (byte) by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public byte GenerateByte(ByteFieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);

            if (a == b)
            {
                return a;
            }

            //     a      b
            //  |--|▀▀▀▀▀▀|--|
            //  0           MAX

            byte result = (byte)_random.Next(a, b + 1);

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 16-bit signed integer by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public short GenerateInt16(Int16FieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);
            short result;

            if (a == b)
            {
                return a;
            }

            if (a == short.MinValue && b == short.MaxValue) // get a value from the whole range
            {
                //  a                         b
                //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                // MIN           0           MAX

                if (_random.Next(2) == 0) // non-negative
                {
                    result = (short)(_random.NextDouble() * ((long)b + 1));
                }
                else // negative
                {
                    result = (short)(-_random.NextDouble() * (Math.Abs((long)a) + 1));
                }
            }
            else if (a == short.MinValue && b < short.MaxValue)
            {
                if (b < 0)
                {
                    //  a        b
                    //  |▀▀▀▀▀▀▀▀|---|------------|
                    // MIN           0           MAX

                    result = (short)-(Math.Abs(b) + _random.NextDouble() * (Math.Abs((long)a) - Math.Abs(b) + 1));
                }
                else
                {
                    //  a                  b
                    //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀|------|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (short)(_random.NextDouble() * (Math.Abs(b) + 1));
                    }
                    else // negative
                    {
                        result = (short)-(_random.NextDouble() * (Math.Abs((long)a) + 1));
                    }
                }
            }
            else if (a > short.MinValue && b == short.MaxValue)
            {
                if (a < 0)
                {
                    //       a                    b
                    //  |----|▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (short)(_random.NextDouble() * ((long)b + 1));
                    }
                    else // negative
                    {
                        result = (short)-(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
                else
                {
                    //                  a         b
                    //  |------------|--|▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    result = (short)(a + _random.NextDouble() * ((long)b - a + 1));
                }
            }
            else // if (min > short.MinValue && max < short.MaxValue)
            {
                if (a >= 0 && b >= 0)
                {
                    //                  a      b
                    //  |------------|--|▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    result = (short)(a + _random.NextDouble() * (b - a + 1));
                }
                else if (a <= 0 && b <= 0)
                {
                    //     a      b
                    //  |--|▀▀▀▀▀▀|--|------------|
                    // MIN           0           MAX

                    result = (short)-(Math.Abs(b) + _random.NextDouble() * (Math.Abs(a) - Math.Abs(b) + 1));
                }
                else // a <= 0 && b >= 0
                {
                    //        a                b
                    //  |-----|▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (short)(_random.NextDouble() * (b + 1));
                    }
                    else // negative
                    {
                        result = (short)-(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
            }

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 16-bit unsigned integer by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public ushort GenerateUInt16(UInt16FieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);

            if (a == b)
            {
                return a;
            }

            //     a      b
            //  |--|▀▀▀▀▀▀|--|
            //  0           MAX

            ushort result = (ushort)(a + _random.NextDouble() * ((double)b - a + 1));

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 32-bit signed integer by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public int GenerateInt32(Int32FieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);
            int result;

            if (a == b)
            {
                return a;
            }

            if (a == int.MinValue && b == int.MaxValue) // get a value from the whole range
            {
                //  a                         b
                //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                // MIN           0           MAX

                if (_random.Next(2) == 0) // non-negative
                {
                    result = (int)(_random.NextDouble() * ((long)b + 1));
                }
                else // negative
                {
                    result = -(int)(_random.NextDouble() * (Math.Abs((long)a) + 1));
                }
            }
            else if (a == int.MinValue && b < int.MaxValue)
            {
                if (b < 0)
                {
                    //  a        b
                    //  |▀▀▀▀▀▀▀▀|---|------------|
                    // MIN           0           MAX

                    result = -(int)(Math.Abs(b) + _random.NextDouble() * (Math.Abs((long)a) - Math.Abs(b) + 1));
                }
                else
                {
                    //  a                  b
                    //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀|------|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (int)(_random.NextDouble() * (Math.Abs(b) + 1));
                    }
                    else // negative
                    {
                        result = -(int)(_random.NextDouble() * (Math.Abs((long)a) + 1));
                    }
                }
            }
            else if (a > int.MinValue && b == int.MaxValue)
            {
                if (a < 0)
                {
                    //       a                    b
                    //  |----|▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (int)(_random.NextDouble() * ((long)b + 1));
                    }
                    else // negative
                    {
                        result = -(int)(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
                else
                {
                    //                  a         b
                    //  |------------|--|▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    result = (int)(a + _random.NextDouble() * ((long)b - a + 1));
                }
            }
            else // if (min > int.MinValue && max < int.MaxValue)
            {
                if (a >= 0 && b >= 0)
                {
                    //                  a      b
                    //  |------------|--|▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    result = (int)(a + _random.NextDouble() * (b - a + 1));
                }
                else if (a <= 0 && b <= 0)
                {
                    //     a      b
                    //  |--|▀▀▀▀▀▀|--|------------|
                    // MIN           0           MAX

                    result = -(int)(Math.Abs(b) + _random.NextDouble() * (Math.Abs(a) - Math.Abs(b) + 1));
                }
                else // a <= 0 && b >= 0
                {
                    //        a                b
                    //  |-----|▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (int)(_random.NextDouble() * (b + 1));
                    }
                    else // negative
                    {
                        result = -(int)(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
            }

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 32-bit unsigned integer by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public uint GenerateUInt32(UInt32FieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);

            if (a == b)
            {
                return a;
            }

            //     a      b
            //  |--|▀▀▀▀▀▀|--|
            //  0           MAX
            
            uint result = (uint)(a + _random.NextDouble() * ((double)b - a + 1));
            
            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 64-bit signed integer by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public long GenerateInt64(Int64FieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);
            long result;

            if (a == b)
            {
                return a;
            }

            if (a == long.MinValue && b == long.MaxValue) // get a value from the whole range
            {
                //  a                         b
                //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                // MIN           0           MAX

                if (_random.Next(2) == 0) // non-negative
                {
                    result = (long)Math.Min(_random.NextDouble() * ((double)b + 1), long.MaxValue);
                }
                else // negative
                {
                    result = (long)Math.Max(-_random.NextDouble() * (Math.Abs((double)a) + 1), long.MinValue);
                }
            }
            else if (a == long.MinValue && b < long.MaxValue)
            {
                if (b < 0)
                {
                    //  a        b
                    //  |▀▀▀▀▀▀▀▀|---|------------|
                    // MIN           0           MAX

                    result = (long)Math.Max(-(Math.Abs(b) + _random.NextDouble() * (Math.Abs((double)a) - Math.Abs(b) + 1)), long.MinValue);
                }
                else
                {
                    //  a                  b
                    //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀|------|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (long)(_random.NextDouble() * (Math.Abs(b) + 1));
                    }
                    else // negative
                    {
                        result = (long)Math.Max(-_random.NextDouble() * (Math.Abs((double)a) + 1), long.MinValue);
                    }
                }
            }
            else if (a > long.MinValue && b == long.MaxValue)
            {
                if (a < 0)
                {
                    //       a                    b
                    //  |----|▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (long)Math.Min(_random.NextDouble() * ((double)b + 1), long.MaxValue);
                    }
                    else // negative
                    {
                        result = (long)(-_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
                else
                {
                    //                  a         b
                    //  |------------|--|▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    result = (long)Math.Min((ulong)(a + _random.NextDouble() * ((ulong)b - (ulong)a + 1)), long.MaxValue);
                }
            }
            else // if (min > int.MinValue && max < int.MaxValue)
            {
                if (a >= 0 && b >= 0)
                {
                    //                  a      b
                    //  |------------|--|▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    result = (long)(a + _random.NextDouble() * (b - a + 1));
                }
                else if (a <= 0 && b <= 0)
                {
                    //     a      b
                    //  |--|▀▀▀▀▀▀|--|------------|
                    // MIN           0           MAX

                    result = -(long)(Math.Abs(b) + _random.NextDouble() * (Math.Abs(a) - Math.Abs(b) + 1));
                }
                else // a <= 0 && b >= 0
                {
                    //        a                b
                    //  |-----|▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (long)(_random.NextDouble() * (b + 1));
                    }
                    else // negative
                    {
                        result = -(long)(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
            }

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random 64-bit unsigned integer by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public ulong GenerateUInt64(UInt64FieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);

            if (a == b)
            {
                return a;
            }

            //     a      b
            //  |--|▀▀▀▀▀▀|--|
            //  0           MAX

            double range = (double)((decimal)b - a + 1);
            ulong result = (ulong)Math.Min((decimal)(a + _random.NextDouble() * range), ulong.MaxValue);

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random single-precision floating-point number by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public float GenerateFloat(SingleFieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);
            float result;

            if (a == b)
            {
                return a;
            }

            if (a == float.MinValue && b == float.MaxValue) // get a value from the whole range
            {
                //  a                         b
                //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                // MIN           0           MAX

                if (_random.Next(2) == 0) // non-negative
                {
                    result = (float)(_random.NextDouble() * ((double)b + 1));
                }
                else // negative
                {
                    result = -(float)(_random.NextDouble() * (Math.Abs((double)a) + 1));
                }
            }
            else if (a == float.MinValue && b < float.MaxValue)
            {
                if (b < 0)
                {
                    //  a        b
                    //  |▀▀▀▀▀▀▀▀|---|------------|
                    // MIN           0           MAX

                    result = -(float)(Math.Abs(b) + _random.NextDouble() * (Math.Abs((double)a) - Math.Abs(b) + 1));
                }
                else
                {
                    //  a                  b
                    //  |▀▀▀▀▀▀▀▀▀▀▀▀|▀▀▀▀▀|------|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = Math.Max((float)(_random.NextDouble() * (Math.Abs(b) + 1)), Math.Abs(b));
                    }
                    else // negative
                    {
                        result = -(float)(_random.NextDouble() * (Math.Abs((double)a) + 1));
                    }
                }
            }
            else if (a > float.MinValue && b == float.MaxValue)
            {
                if (a < 0)
                {
                    //       a                    b
                    //  |----|▀▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (float)(_random.NextDouble() * ((double)b + 1));
                    }
                    else // negative
                    {
                        result = -(float)(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
                else
                {
                    //                  a         b
                    //  |------------|--|▀▀▀▀▀▀▀▀▀|
                    // MIN           0           MAX

                    result = (float)(a + _random.NextDouble() * ((double)b - a + 1));
                }
            }
            else // if (min > int.MinValue && max < int.MaxValue)
            {
                if (a >= 0 && b >= 0)
                {
                    //                  a      b
                    //  |------------|--|▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    result = (float)(a + _random.NextDouble() * (b - a + 1));
                }
                else if (a <= 0 && b <= 0)
                {
                    //     a      b
                    //  |--|▀▀▀▀▀▀|--|------------|
                    // MIN           0           MAX

                    result = -(float)(Math.Abs(b) + _random.NextDouble() * (Math.Abs(a) - Math.Abs(b) + 1));
                }
                else // a <= 0 && b >= 0
                {
                    //        a                b
                    //  |-----|▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀|--|
                    // MIN           0           MAX

                    if (_random.Next(2) == 0) // non-negative
                    {
                        result = (float)(_random.NextDouble() * (b + 1));
                    }
                    else // negative
                    {
                        result = -(float)(_random.NextDouble() * (Math.Abs(a) + 1));
                    }
                }
            }

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random double-precision floating-point number by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public double GenerateDouble(DoubleFieldModel model)
        {
            var a = Math.Min(model.MinValue, model.MaxValue);
            var b = Math.Max(model.MinValue, model.MaxValue);

            if (double.IsNegativeInfinity(a))
            {
                a = double.MinValue;
            }
            else if (double.IsPositiveInfinity(a))
            {
                a = double.MaxValue;
            }

            if (double.IsNegativeInfinity(b))
            {
                b = double.MinValue;
            }
            else if (double.IsPositiveInfinity(b))
            {
                b = double.MaxValue;
            }

            if (a == b)
            {
                return a;
            }

            //        a                b
            //  |-----|▀▀▀▀▀▀|▀▀▀▀▀▀▀▀▀|--|
            // MIN           0           MAX

            long long_a = (long)a;
            long long_b = (long)b;
            long integerPart = GenerateInt64(new Int64FieldModel(null, long_a, long_b - 1));
            double fractionPart = _random.NextDouble();

            double result = Math.Min(integerPart + fractionPart, double.MaxValue);

            return result.KeepInRange(a, b);
        }

        /// <summary>
        /// Generates a random hash by the given model's settings. The returned hash is always lowercase.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateHash(HashFieldModel model)
        {
            if (model.DesiredLength <= 0)
            {
                return string.Empty;
            }

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

        /// <summary>
        /// Generates a random GUID by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateGuid(GuidFieldModel model)
        {
            return Guid.NewGuid().ToString(model.SeparateWithHyphens ? "D" : "N");
        }

        /// <summary>
        /// Generates a random Base64 encoded string by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateBase64(Base64FieldModel model)
        {
            if (model.DesiredLength <= 0)
            {
                return string.Empty;
            }

            int RoundDownToBeDivisibleBy4(int value) // a local helper function
            {
                return (int)Math.Floor(value / 4.0) * 3;
            }

            var roundDownDesiredLength = RoundDownToBeDivisibleBy4(model.DesiredLength);
            var sourceText = GenerateText(new TextFieldModel(null, roundDownDesiredLength, roundDownDesiredLength, true, true));

            var stringBytes = Encoding.Default.GetBytes(sourceText.ToCharArray());
            var result = Convert.ToBase64String(stringBytes);

            return result;
        }

        /// <summary>
        /// Returns a random item from a custom collection by the given model's settings.
        /// </summary>
        /// <param name="model">The model whose properties are used to influence the result.</param>
        /// <returns></returns>
        public string GenerateFromCustomSet(CustomSetFieldModel model)
        {
            if (model.Items.Count == 0)
            {
                return string.Empty;
            }

            return GetRandomItemFrom(model.Items);
        }


        #region [ Helpers ]

        /// <summary>
        /// Returns a random item from the given collection.
        /// If the collection is empty, an IndexOutOfRangeException is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="items">The collection which we want to get a random item from.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a single character, randomly chosen from the given character set.
        /// The set can be extended by toggling the parameters,
        /// or you can even add your own custom characters.
        /// </summary>
        /// <param name="letterCase">
        /// With this parameter you can force whether the returned character shall be lowercase (LetterCase.LowerCaseOnly),
        /// uppercase (LetterCase.UpperCaseOnly) or any of them (LetterCase.Ignore).</param>
        /// <param name="includeLetters">If true, letters of the English alphabet are included in the set; otherwise false.</param>
        /// <param name="includeSpace">If true, the space character is included in the set; otherwise false.</param>
        /// <param name="includeDigits">If true, all the digits [0-9] are included in the set; otherwise false.</param>
        /// <param name="includeCustom">With this parameter you can extend the base set with custom characters.</param>
        /// <returns></returns>
        public char GetRandomChar(
            LetterCase letterCase = LetterCase.Ignore,
            bool includeLetters = false,
            bool includeDigits = false,
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

            if (includeDigits)
            {
                for (int i = 0; i <= 9; i++)
                {
                    choosable.Add(Convert.ToChar(i.ToString()));
                }
            }

            if (includeCustom != null && includeCustom.Count > 0)
            {
                choosable.AddRange(includeCustom);
            }

            choosable = choosable.Distinct().ToList();

            // empty array is handled by an exception inside the GetRandomItemFrom() method
            var c = GetRandomItemFrom(choosable);

            switch (letterCase)
            {
                case LetterCase.Ignore: return _random.Next(2) == 0 ? char.ToLower(c) : char.ToUpper(c);
                case LetterCase.LowerCaseOnly: return char.ToLower(c);
                case LetterCase.UpperCaseOnly: return char.ToUpper(c);
                default: return char.MinValue;
            }
        }

        /// <summary>
        /// Creates a string with the desired length, and with the specified type of characters.
        /// NOTE: The function returns a string that CAN contain ANY of the desired characters,
        /// so it does NOT mean that it surely will contain ALL of them.
        /// </summary>
        /// <param name="desiredLength">The length of the returned string.</param>
        /// <param name="letterCase">With this parameter you can force whether the returned string shall contain lowercase (LetterCase.LowerCaseOnly),
        /// uppercase (LetterCase.UpperCaseOnly) or mixed-case (LetterCase.Ignore - this is default) characters.</param>
        /// <param name="includeLetters">If true, the returned string can contain letters of the English alphabet; otherwise false.</param>
        /// <param name="includeSpace">If true, the returned string can contain the space character; otherwise false.</param>
        /// <param name="includeDigits">If true, the returned string can contain any of the digits [0-9]; otherwise false.</param>
        /// <param name="includeCustom">With this parameter you can specify custom characters that the returned string can contain.</param>
        /// <returns></returns>
        private string CreateString(
            int desiredLength,
            LetterCase letterCase = LetterCase.Ignore,
            bool includeLetters = false,
            bool includeSpace = false,
            bool includeDigits = false,
            List<char> includeCustom = null)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < desiredLength; i++)
            {
                var c = GetRandomChar(
                    letterCase,
                    includeLetters,
                    includeDigits,
                    includeCustom);

                sb.Append(c);
            }

            return sb.ToString();
        }

        public bool IsWithinNumericTypeRange(string value, FieldType numericFieldType, bool signed)
        {
            var fixedPointRegex = new Regex(@"^[\+-]?\d+$");
            var floatingPointRegex = new Regex(@"^[\+-]?\d+(?:\.\d+)?(?:e[\+-]?\d+)?[fdm]?$", RegexOptions.IgnoreCase);
            bool conversionSucceeded = false;
            bool valueIsInRange = false;

            // value should be a fixed-point number
            if (numericFieldType == FieldType.Byte
                || numericFieldType == FieldType.Int16
                || numericFieldType == FieldType.Int32
                || numericFieldType == FieldType.Int64)
            {
                if (fixedPointRegex.IsMatch(value))
                {
                    // check whether the desired specific type's range would fit
                    switch (numericFieldType)
                    {
                        case FieldType.Byte:
                        {
                            if (signed)
                            {
                                conversionSucceeded = sbyte.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out sbyte tempValue);
                                valueIsInRange = tempValue.IsInRange(sbyte.MinValue, sbyte.MaxValue);
                            }
                            else
                            {
                                conversionSucceeded = byte.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out byte tempValue);
                                valueIsInRange = tempValue.IsInRange(byte.MinValue, byte.MaxValue);
                            }
                            break;
                        }
                        case FieldType.Int16:
                        {
                            if (signed)
                            {
                                conversionSucceeded = short.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out short tempValue);
                                valueIsInRange = tempValue.IsInRange(short.MinValue, short.MaxValue);
                            }
                            else
                            {
                                conversionSucceeded = ushort.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ushort tempValue);
                                valueIsInRange = tempValue.IsInRange(ushort.MinValue, ushort.MaxValue);
                            }
                            break;
                        }
                        case FieldType.Int32:
                        {
                            if (signed)
                            {
                                conversionSucceeded = int.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out int tempValue);
                                valueIsInRange = tempValue.IsInRange(int.MinValue, int.MaxValue);
                            }
                            else
                            {
                                conversionSucceeded = uint.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out uint tempValue);
                                valueIsInRange = tempValue.IsInRange(uint.MinValue, uint.MaxValue);
                            }
                            break;
                        }
                        case FieldType.Int64:
                        {
                            if (signed)
                            {
                                conversionSucceeded = long.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out long tempValue);
                                valueIsInRange = tempValue.IsInRange(long.MinValue, long.MaxValue);
                            }
                            else
                            {
                                conversionSucceeded = ulong.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out ulong tempValue);
                                valueIsInRange = tempValue.IsInRange(ulong.MinValue, ulong.MaxValue);
                            }
                            break;
                        }
                        default:
                            break;
                    }
                }
                else
                {
                    throw new ArithmeticException($"{value} is not a valid fixed-point number.");
                }
            }
            // value should be a floating-point number
            else if (numericFieldType == FieldType.Single
                    || numericFieldType == FieldType.Double
                    || numericFieldType == FieldType.Decimal)
            {
                if (floatingPointRegex.IsMatch(value))
                {
                    // check whether the desired specific type's range would fit
                    switch (numericFieldType)
                    {
                        case FieldType.Single:
                        {
                            conversionSucceeded = float.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out float tempValue);
                            valueIsInRange = tempValue.IsInRange(float.MinValue, float.MaxValue);
                            break;
                        }
                        case FieldType.Double:
                        {
                            conversionSucceeded = double.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double tempValue);
                            valueIsInRange = tempValue.IsInRange(double.MinValue, double.MaxValue);
                            break;
                        }
                        case FieldType.Decimal:
                        {
                            conversionSucceeded = decimal.TryParse(value, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out decimal tempValue);
                            valueIsInRange = tempValue == 0
                                ? !value.ToLower().Contains("e")
                                : tempValue.IsInRange(decimal.MinValue, decimal.MaxValue);
                            break;
                        }
                        default:
                            break;
                    }
                }
                else
                {
                    throw new ArithmeticException($"{value} is not a valid floating-point number.");
                }
            }

            return conversionSucceeded && valueIsInRange;
        }

        #endregion
    }
}