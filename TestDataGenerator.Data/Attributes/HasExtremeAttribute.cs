using System;

namespace TestDataGenerator.Data
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class HasExtremeAttribute : Attribute
    {
        public bool HasMinValue { get; set; }

        public bool HasMaxValue { get; set; }
    }
}