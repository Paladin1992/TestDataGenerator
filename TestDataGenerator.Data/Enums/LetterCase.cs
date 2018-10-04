using System.ComponentModel;

namespace TestDataGenerator.Data.Enums
{
    public enum LetterCase
    {
        [Description("Vegyes")]
        Ignore = 0,

        [Description("kisbetűs")]
        LowerCaseOnly = 1,

        [Description("NAGYBETŰS")]
        UpperCaseOnly = 2
    }
}