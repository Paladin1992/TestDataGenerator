using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Web.Models
{
    public class TemporarySetupViewModel
    {
        public LastNameFieldModel LastNameFieldModel { get; set; }

        public FirstNameFieldModel FirstNameFieldModel { get; set; }

        public DateTimeFieldModel DateTimeFieldModel { get; set; }

        public EmailFieldModel EmailFieldModel { get; set; }

        public TextFieldModel TextFieldModel { get; set; }

        public ByteFieldModel ByteFieldModel { get; set; }

        public Int16FieldModel Int16FieldModel { get; set; }

        public Int32FieldModel Int32FieldModel { get; set; }

        public Int64FieldModel Int64FieldModel { get; set; }

        public SingleFieldModel SingleFieldModel { get; set; }

        public DoubleFieldModel DoubleFieldModel { get; set; }

        public DecimalFieldModel DecimalFieldModel { get; set; }

        public HashFieldModel HashFieldModel { get; set; }

        public GuidFieldModel GuidFieldModel { get; set; }

        public Base64FieldModel Base64FieldModel { get; set; }

        public CustomSetFieldModel CustomSetFieldModel { get; set; }
    }
}