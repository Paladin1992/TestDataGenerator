using System.ComponentModel;

namespace TestDataGenerator.Data.Enums
{
    public enum Gender
    {
        [Description("")]
        None = 0,

        [Description("Férfi")]
        Male = 1,

        [Description("Nő")]
        Female = 2
    }
}