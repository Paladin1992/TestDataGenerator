using System;
using TestDataGenerator.Common;
using TestDataGenerator.Data.Enums;
using TestDataGenerator.Data.Models;
using TestDataGenerator.Services;
using Xunit;
using FluentAssertions;

namespace TestDataGenerator.Tests
{
    public class DataGeneratorTests
    {
        private readonly DataGeneratorService _dataGeneratorService = new DataGeneratorService(true);

        public static readonly object[][] CorrectData = new object[][]
        {
            new object[] { DateTime.MinValue, DateTime.MinValue },
            new object[] { DateTime.MinValue, new DateTime(1899, 12, 31) },
            new object[] { DateTime.MinValue, DateTime.MaxValue },
            new object[] { new DateTime(1900, 1, 1), DateTime.Today },
            new object[] { new DateTime(2000, 1, 1), new DateTime(2000, 1, 1) },
            new object[] { new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 23, 59, 59) },
            new object[] { new DateTime(2000, 1, 1, 18, 0, 0), new DateTime(2000, 1, 1, 19, 0, 0) },
            new object[] { DateTime.Today, DateTime.Today },
            new object[] { DateTime.MaxValue.Date.AddDays(-1), DateTime.MaxValue },
            new object[] { DateTime.MaxValue, DateTime.MaxValue }
        };

        [Theory]
        [MemberData(nameof(CorrectData))]
        public void TestDateTime(DateTime minDate, DateTime maxDate)
        {
            // Arrange
            var model = new DateTimeFieldModel("Dátum", minDate, maxDate);

            // Act
            DateTime result = _dataGeneratorService.GenerateDateTime(model);

            // Assert
            result.Should().BeOnOrAfter(minDate).And.BeOnOrBefore(maxDate);
        }

        [Fact]
        public void EmailGeneratorGeneratesValidEmail()
        {
            // Arrange
            string emailRegexPattern = @"\w{5,10}@\w{3,6}\.(?:hu|com|de|en)";

            // Act
            string result = _dataGeneratorService.GenerateEmail(new EmailFieldModel("E-mail"));

            // Assert
            result.Should().MatchRegex(emailRegexPattern);
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        [InlineData(5, 5)]
        [InlineData(5, 6)]
        [InlineData(5, 10)]
        public void TextGenerationWithMinLengthAndMaxLengthConstraints(int minLength, int maxLength)
        {
            // Arrange
            var model = new TextFieldModel("Text", minLength, maxLength);

            // Act
            string result = _dataGeneratorService.GenerateText(model);

            // Assert
            if (maxLength <= 0)
            {
                result.Should().Be(string.Empty);
            }
            else
            {
                result.Length.Should().BeInRange(minLength, maxLength);
            }
        }

        [Theory]
        [InlineData(false, true, LetterCase.Ignore)]
        [InlineData(true, false, LetterCase.Ignore)]
        [InlineData(true, true, LetterCase.Ignore)]
        [InlineData(false, true, LetterCase.LowerCaseOnly)]
        [InlineData(true, false, LetterCase.LowerCaseOnly)]
        [InlineData(true, true, LetterCase.LowerCaseOnly)]
        [InlineData(false, true, LetterCase.UpperCaseOnly)]
        [InlineData(true, false, LetterCase.UpperCaseOnly)]
        [InlineData(true, true, LetterCase.UpperCaseOnly)]
        public void TextGenerationWithFixLengthAndVariousParametersWithoutCustom(bool mustContainLetters, bool mustContainDigit, LetterCase letterCase)
        {
            // Arrange
            int fixLength = 500;
            var model = new TextFieldModel("Text", fixLength, fixLength, mustContainLetters, mustContainDigit, null, letterCase);

            // Act
            string result = _dataGeneratorService.GenerateText(model);

            // Assert
            result.Length.Should().Be(fixLength);

            if (mustContainLetters)
            {
                result.Should().MatchRegex(@"\w");
            }

            if (mustContainDigit)
            {
                result.Should().MatchRegex(@"\d");
            }

            switch (letterCase)
            {
                case LetterCase.Ignore: // we don't care
                    break;
                case LetterCase.LowerCaseOnly: result.IsLowerCaseOnly().Should().BeTrue();
                    break;
                case LetterCase.UpperCaseOnly: result.IsUpperCaseOnly().Should().BeTrue();
                    break;
                default:
                    break;
            }
        }

        [Theory]
        [InlineData(false, false, LetterCase.Ignore)]
        [InlineData(false, false, LetterCase.LowerCaseOnly)]
        [InlineData(false, false, LetterCase.UpperCaseOnly)]
        public void TextGenerationShouldThrowException(bool mustContainLetters, bool mustContainDigit, LetterCase letterCase)
        {
            // Arrange
            int fixLength = 500;
            var model = new TextFieldModel("Text", fixLength, fixLength, mustContainLetters, mustContainDigit, null, letterCase);

            // Act
            Action act = () => _dataGeneratorService.GenerateText(model);

            // Assert
            act.Should().ThrowExactly<IndexOutOfRangeException>();
        }

        [Theory]
        [InlineData(sbyte.MinValue, sbyte.MinValue)]
        [InlineData(sbyte.MinValue, sbyte.MinValue + 1)]
        [InlineData(sbyte.MinValue, 0)]
        [InlineData(sbyte.MinValue, +10)]
        [InlineData(sbyte.MinValue, sbyte.MaxValue)]
        [InlineData(-10, sbyte.MaxValue)]
        [InlineData(-10, -5)]
        [InlineData(-5, +5)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, sbyte.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(sbyte.MaxValue - 1, sbyte.MaxValue)]
        [InlineData(sbyte.MaxValue, sbyte.MaxValue)]
        public void CheckRangeForSByteGenerator(sbyte minValue, sbyte maxValue)
        {
            // Arrange
            var model = new SByteFieldModel("SByte", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            int result = _dataGeneratorService.GenerateSByte(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, +10)]
        [InlineData(0, byte.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(byte.MaxValue - 1, byte.MaxValue)]
        [InlineData(byte.MaxValue, byte.MaxValue)]
        public void CheckRangeForByteGenerator(byte minValue, byte maxValue)
        {
            // Arrange
            var model = new ByteFieldModel("Byte", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            uint result = _dataGeneratorService.GenerateByte(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(short.MinValue, short.MinValue)]
        [InlineData(short.MinValue, short.MinValue + 1)]
        [InlineData(short.MinValue, 0)]
        [InlineData(short.MinValue, +1000)]
        [InlineData(short.MinValue, short.MaxValue)]
        [InlineData(-1000, short.MaxValue)]
        [InlineData(-10, -5)]
        [InlineData(-5, +5)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, short.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(short.MaxValue - 1, short.MaxValue)]
        [InlineData(short.MaxValue, short.MaxValue)]
        public void CheckRangeForInt16Generator(short minValue, short maxValue)
        {
            // Arrange
            var model = new Int16FieldModel("Int16", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            int result = _dataGeneratorService.GenerateInt16(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, +1000)]
        [InlineData(0, ushort.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(ushort.MaxValue - 1, ushort.MaxValue)]
        [InlineData(ushort.MaxValue, ushort.MaxValue)]
        public void CheckRangeForUInt16Generator(ushort minValue, ushort maxValue)
        {
            // Arrange
            var model = new UInt16FieldModel("UInt16", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            uint result = _dataGeneratorService.GenerateUInt16(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(int.MinValue, int.MinValue)]
        [InlineData(int.MinValue, int.MinValue + 1)]
        [InlineData(int.MinValue, 0)]
        [InlineData(int.MinValue, +1000)]
        [InlineData(int.MinValue, int.MaxValue)]
        [InlineData(-1000, int.MaxValue)]
        [InlineData(-10, -5)]
        [InlineData(-5, +5)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, int.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(int.MaxValue - 1, int.MaxValue)]
        [InlineData(int.MaxValue, int.MaxValue)]
        public void CheckRangeForInt32Generator(int minValue, int maxValue)
        {
            // Arrange
            var model = new Int32FieldModel("Int32", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            int result = _dataGeneratorService.GenerateInt32(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, +1000)]
        [InlineData(0, uint.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(uint.MaxValue - 1, uint.MaxValue)]
        [InlineData(uint.MaxValue, uint.MaxValue)]
        public void CheckRangeForUInt32Generator(uint minValue, uint maxValue)
        {
            // Arrange
            var model = new UInt32FieldModel("UInt32", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            uint result = _dataGeneratorService.GenerateUInt32(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(long.MinValue, long.MinValue)]
        [InlineData(long.MinValue, long.MinValue + 1)]
        [InlineData(long.MinValue, 0)]
        [InlineData(long.MinValue, +1000)]
        [InlineData(long.MinValue, long.MaxValue)]
        [InlineData(-1000, long.MaxValue)]
        [InlineData(-10, -5)]
        [InlineData(-5, +5)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, long.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(long.MaxValue - 1, long.MaxValue)]
        [InlineData(long.MaxValue, long.MaxValue)]
        public void CheckRangeForInt64Generator(long minValue, long maxValue)
        {
            // Arrange
            var model = new Int64FieldModel("Int64", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            long result = _dataGeneratorService.GenerateInt64(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, +1000)]
        [InlineData(0, ulong.MaxValue)]
        [InlineData(5, 5)]
        [InlineData(10, 5)]
        [InlineData(10, 20)]
        [InlineData(ulong.MaxValue - 1, ulong.MaxValue)]
        [InlineData(ulong.MaxValue, ulong.MaxValue)]
        public void CheckRangeForUInt64Generator(ulong minValue, ulong maxValue)
        {
            // Arrange
            var model = new UInt64FieldModel("UInt64", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            ulong result = _dataGeneratorService.GenerateUInt64(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(float.MinValue, float.MinValue)]
        [InlineData(float.MinValue, float.MinValue / 2f)]
        [InlineData(float.MinValue, 0f)]
        [InlineData(float.MinValue, +1000f)]
        [InlineData(float.MinValue, float.MaxValue)]
        [InlineData(-1000f, float.MaxValue)]
        [InlineData(-10f, -5f)]
        [InlineData(-5f, +5f)]
        [InlineData(0f, 0f)]
        [InlineData(0f, 1f)]
        [InlineData(0f, float.MaxValue)]
        [InlineData(5f, 5f)]
        [InlineData(10f, 5f)]
        [InlineData(10f, 20f)]
        [InlineData(float.MaxValue / 2f, float.MaxValue)]
        [InlineData(float.MaxValue, float.MaxValue)]
        public void CheckRangeForFloatGenerator(float minValue, float maxValue)
        {
            // Arrange
            var model = new SingleFieldModel("Float", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            float result = _dataGeneratorService.GenerateFloat(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(double.NegativeInfinity, double.MinValue)]
        [InlineData(double.MinValue, double.MinValue)]
        [InlineData(double.MinValue, double.MinValue / 2d)]
        [InlineData(double.MinValue, 0d)]
        [InlineData(double.MinValue, +1000d)]
        [InlineData(double.MinValue, double.MaxValue)]
        [InlineData(-1000d, double.MaxValue)]
        [InlineData(-10d, -5d)]
        [InlineData(-5d, +5d)]
        [InlineData(0d, 0d)]
        [InlineData(0d, 1d)]
        [InlineData(0d, double.MaxValue)]
        [InlineData(5d, 5d)]
        [InlineData(10d, 5d)]
        [InlineData(10d, 20d)]
        [InlineData(double.MaxValue / 2d, double.MaxValue)]
        [InlineData(double.MaxValue, double.MaxValue)]
        [InlineData(double.MaxValue, double.PositiveInfinity)]
        public void CheckRangeForDoubleGenerator(double minValue, double maxValue)
        {
            // Arrange
            var model = new DoubleFieldModel("Double", minValue, maxValue); // we must pass the values in their original order!
            var correctedMinValue = Math.Min(minValue, maxValue);
            var correctedMaxValue = Math.Max(minValue, maxValue);

            // Act
            double result = _dataGeneratorService.GenerateDouble(model);

            // Assert
            result.Should().BeInRange(correctedMinValue, correctedMaxValue);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(7)]
        [InlineData(32)]
        [InlineData(256)]
        [InlineData(1024)]
        public void TestBase64Generator(int desiredLength)
        {
            // Arrange
            var model = new Base64FieldModel("Base64", desiredLength);
            var roundDownDesiredLength = RoundDownToBeDivisibleBy4(desiredLength);

            int RoundDownToBeDivisibleBy4(int value) // a local helper function
            {
                return (int)Math.Floor(value / 4.0) * 4;
            }

            // Act
            string result = _dataGeneratorService.GenerateBase64(model);

            // Assert
            if (desiredLength <= 0)
            {
                result.Length.Should().Be(0);
                result.Should().BeEmpty();
            }
            else
            {
                (result.Length % 4).Should().Be(0);
                result.Length.Should().Be(roundDownDesiredLength);
                result.Should().MatchRegex(@"[A-Za-z0-9\+\/=]*");
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(32)]
        [InlineData(256)]
        [InlineData(1024)]
        public void TestHashLengthAndItsCharacters(int desiredLength)
        {
            // Arrange
            var model = new HashFieldModel("Hash", desiredLength);

            // Act
            string result = _dataGeneratorService.GenerateHash(model);

            // Assert
            if (desiredLength <= 0)
            {
                result.Length.Should().Be(0);
                result.Should().BeEmpty();
            }
            else
            {
                result.Length.Should().Be(desiredLength);
                result.Should().MatchRegex(@"[0-9a-f]+");
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestGuid(bool useHypens)
        {
            // Arrange
            var model = new GuidFieldModel("Guid", useHypens);

            // Act
            string result = _dataGeneratorService.GenerateGuid(model);

            // Assert
            if (useHypens)
            {
                result.Should().MatchRegex(@"[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}");
            }
            else
            {
                result.Should().MatchRegex(@"[0-9a-f]{32}");
            }
        }

        [Theory]
        [InlineData("0", FieldType.Byte, false)]
        [InlineData("128", FieldType.Byte, false)]
        [InlineData("255", FieldType.Byte, false)]
        [InlineData("-128", FieldType.Byte, true)]
        [InlineData("-100", FieldType.Byte, true)]
        [InlineData("0", FieldType.Byte, true)]
        [InlineData("+100", FieldType.Byte, true)]
        [InlineData("127", FieldType.Byte, true)]
        
        [InlineData("0", FieldType.Int16, false)]
        [InlineData("1024", FieldType.Int16, false)]
        [InlineData("65535", FieldType.Int16, false)]
        [InlineData("-32768", FieldType.Int16, true)]
        [InlineData("-1024", FieldType.Int16, true)]
        [InlineData("0", FieldType.Int16, true)]
        [InlineData("+1024", FieldType.Int16, true)]
        [InlineData("32767", FieldType.Int16, true)]

        [InlineData("0", FieldType.Int32, false)]
        [InlineData("1024", FieldType.Int32, false)]
        [InlineData("4294967295", FieldType.Int32, false)]
        [InlineData("-2147483648", FieldType.Int32, true)]
        [InlineData("-1024", FieldType.Int32, true)]
        [InlineData("0", FieldType.Int32, true)]
        [InlineData("+1024", FieldType.Int32, true)]
        [InlineData("2147483647", FieldType.Int32, true)]

        [InlineData("0", FieldType.Int64, false)]
        [InlineData("1024", FieldType.Int64, false)]
        [InlineData("18446744073709551615", FieldType.Int64, false)]
        [InlineData("-9223372036854775808", FieldType.Int64, true)]
        [InlineData("-1024", FieldType.Int64, true)]
        [InlineData("0", FieldType.Int64, true)]
        [InlineData("1024", FieldType.Int64, true)]
        [InlineData("9223372036854775807", FieldType.Int64, true)]
        public void FixedPointConversionWithinValidRange(string value, FieldType numericFieldType, bool signed)
        {
            // Arrange
            
            // Act
            bool result = _dataGeneratorService.IsWithinNumericTypeRange(value, numericFieldType, signed);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("-1", FieldType.Byte, false)]
        [InlineData("256", FieldType.Byte, false)]
        [InlineData("-129", FieldType.Byte, true)]
        [InlineData("129", FieldType.Byte, true)]
        [InlineData("-1", FieldType.Int16, false)]
        [InlineData("80000", FieldType.Int16, false)]
        [InlineData("-33000", FieldType.Int16, true)]
        [InlineData("33000", FieldType.Int16, true)]
        [InlineData("-1", FieldType.Int32, false)]
        [InlineData("5294967295", FieldType.Int32, false)]
        [InlineData("-3147483647", FieldType.Int32, true)]
        [InlineData("3147483647", FieldType.Int32, true)]
        [InlineData("-1", FieldType.Int64, false)]
        [InlineData("18446744073709551616", FieldType.Int64, false)]
        [InlineData("-9223372036854775809", FieldType.Int64, true)]
        [InlineData("9223372036854775809", FieldType.Int64, true)]
        public void FixedPointConversionOutsideValidRange(string value, FieldType numericFieldType, bool signed)
        {
            // Arrange

            // Act
            bool result = _dataGeneratorService.IsWithinNumericTypeRange(value, numericFieldType, signed);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("hello", FieldType.Byte, false)]
        [InlineData("1.", FieldType.Byte, false)]
        [InlineData("e", FieldType.Byte, false)]
        [InlineData("12e", FieldType.Byte, false)]
        [InlineData("12e+", FieldType.Byte, false)]
        [InlineData("1.2e+", FieldType.Byte, false)]
        [InlineData("hello", FieldType.Byte, true)]
        [InlineData("1.", FieldType.Byte, true)]
        [InlineData("e", FieldType.Byte, true)]
        [InlineData("12e", FieldType.Byte, true)]
        [InlineData("12e+", FieldType.Byte, true)]
        [InlineData("1.2e+", FieldType.Byte, true)]

        [InlineData("hello", FieldType.Int16, false)]
        [InlineData("1.", FieldType.Int16, false)]
        [InlineData("e", FieldType.Int16, false)]
        [InlineData("12e", FieldType.Int16, false)]
        [InlineData("12e+", FieldType.Int16, false)]
        [InlineData("1.2e+", FieldType.Int16, false)]
        [InlineData("hello", FieldType.Int16, true)]
        [InlineData("1.", FieldType.Int16, true)]
        [InlineData("e", FieldType.Int16, true)]
        [InlineData("12e", FieldType.Int16, true)]
        [InlineData("12e+", FieldType.Int16, true)]
        [InlineData("1.2e+", FieldType.Int16, true)]

        [InlineData("hello", FieldType.Int32, false)]
        [InlineData("1.", FieldType.Int32, false)]
        [InlineData("e", FieldType.Int32, false)]
        [InlineData("12e", FieldType.Int32, false)]
        [InlineData("12e+", FieldType.Int32, false)]
        [InlineData("1.2e+", FieldType.Int32, false)]
        [InlineData("hello", FieldType.Int32, true)]
        [InlineData("1.", FieldType.Int32, true)]
        [InlineData("e", FieldType.Int32, true)]
        [InlineData("12e", FieldType.Int32, true)]
        [InlineData("12e+", FieldType.Int32, true)]
        [InlineData("1.2e+", FieldType.Int32, true)]

        [InlineData("hello", FieldType.Int64, false)]
        [InlineData("1.", FieldType.Int64, false)]
        [InlineData("e", FieldType.Int64, false)]
        [InlineData("12e", FieldType.Int64, false)]
        [InlineData("12e+", FieldType.Int64, false)]
        [InlineData("1.2e+", FieldType.Int64, false)]
        [InlineData("hello", FieldType.Int64, true)]
        [InlineData("1.", FieldType.Int64, true)]
        [InlineData("e", FieldType.Int64, true)]
        [InlineData("12e", FieldType.Int64, true)]
        [InlineData("12e+", FieldType.Int64, true)]
        [InlineData("1.2e+", FieldType.Int64, true)]
        public void FixedPointConversionShouldThrowException(string value, FieldType numericFieldType, bool signed)
        {
            // Arrange

            // Act
            Action act = () => _dataGeneratorService.IsWithinNumericTypeRange(value, numericFieldType, signed);

            // Assert
            act.Should().ThrowExactly<ArithmeticException>();
        }

        [Theory]
        [InlineData("-3.40282347E+38", FieldType.Single)]
        [InlineData("-12e+34", FieldType.Single)]
        [InlineData("-12e-34", FieldType.Single)]
        [InlineData("-12e34", FieldType.Single)]
        [InlineData("-1.2e34", FieldType.Single)]
        [InlineData("-1.2", FieldType.Single)]
        [InlineData("-1", FieldType.Single)]
        [InlineData("0", FieldType.Single)]
        [InlineData("1", FieldType.Single)]
        [InlineData("1.2", FieldType.Single)]
        [InlineData("1.2e34", FieldType.Single)]
        [InlineData("12e34", FieldType.Single)]
        [InlineData("12e-34", FieldType.Single)]
        [InlineData("12e+34", FieldType.Single)]
        [InlineData("3.40282347E+38", FieldType.Single)]

        [InlineData("-1.7976931348623157E+308", FieldType.Double)]
        [InlineData("-12e+34", FieldType.Double)]
        [InlineData("-12e-34", FieldType.Double)]
        [InlineData("-12e34", FieldType.Double)]
        [InlineData("-1.2e34", FieldType.Double)]
        [InlineData("-1.2", FieldType.Double)]
        [InlineData("-1", FieldType.Double)]
        [InlineData("0", FieldType.Double)]
        [InlineData("1", FieldType.Double)]
        [InlineData("1.2", FieldType.Double)]
        [InlineData("1.2e34", FieldType.Double)]
        [InlineData("12e34", FieldType.Double)]
        [InlineData("12e-34", FieldType.Double)]
        [InlineData("12e+34", FieldType.Double)]
        [InlineData("1.7976931348623157E+308", FieldType.Double)]

        [InlineData("-79228162514264337593543950335", FieldType.Decimal)]
        [InlineData("-1.2", FieldType.Decimal)]
        [InlineData("-1", FieldType.Decimal)]
        [InlineData("0", FieldType.Decimal)]
        [InlineData("1", FieldType.Decimal)]
        [InlineData("1.2", FieldType.Decimal)]
        [InlineData("79228162514264337593543950335", FieldType.Decimal)]
        public void FloatingPointConversionWithinValidRange(string value, FieldType numericFieldType)
        {
            // Arrange
            
            // Act
            bool result = _dataGeneratorService.IsWithinNumericTypeRange(value, numericFieldType, false);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData("-3.40282347E+40", FieldType.Single)]
        [InlineData("3.40282347E+40", FieldType.Single)]

        [InlineData("-1.7976931348623157E+408", FieldType.Double)]
        [InlineData("1.7976931348623157E+408", FieldType.Double)]

        [InlineData("-79228162514264337593543950336", FieldType.Decimal)]
        [InlineData("-12e+34", FieldType.Decimal)]
        [InlineData("-12e-345", FieldType.Decimal)]
        [InlineData("-12e34", FieldType.Decimal)]
        [InlineData("-1.2e34", FieldType.Decimal)]
        [InlineData("1.2e34", FieldType.Decimal)]
        [InlineData("12e34", FieldType.Decimal)]
        [InlineData("12e-345", FieldType.Decimal)]
        [InlineData("12e+34", FieldType.Decimal)]
        [InlineData("79228162514264337593543950336", FieldType.Decimal)]
        public void FloatingPointConversionOutsideValidRange(string value, FieldType numericFieldType)
        {
            // Arrange
            
            // Act
            bool result = _dataGeneratorService.IsWithinNumericTypeRange(value, numericFieldType, false);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("hello", FieldType.Single)]
        [InlineData("1.", FieldType.Single)]
        [InlineData("e", FieldType.Single)]
        [InlineData("12e", FieldType.Single)]
        [InlineData("12e+", FieldType.Single)]
        [InlineData("1.2e+", FieldType.Single)]

        [InlineData("hello", FieldType.Double)]
        [InlineData("1.", FieldType.Double)]
        [InlineData("e", FieldType.Double)]
        [InlineData("12e", FieldType.Double)]
        [InlineData("12e+", FieldType.Double)]
        [InlineData("1.2e+", FieldType.Double)]

        [InlineData("hello", FieldType.Decimal)]
        [InlineData("1.", FieldType.Decimal)]
        [InlineData("e", FieldType.Decimal)]
        [InlineData("12e", FieldType.Decimal)]
        [InlineData("12e+", FieldType.Decimal)]
        [InlineData("1.2e+", FieldType.Decimal)]
        public void FloatingPointConversionShouldThrowException(string value, FieldType numericFieldType)
        {
            // Arrange

            // Act
            Action act = () => _dataGeneratorService.IsWithinNumericTypeRange(value, numericFieldType, false);

            // Assert
            act.Should().ThrowExactly<ArithmeticException>();
        }
    }
}