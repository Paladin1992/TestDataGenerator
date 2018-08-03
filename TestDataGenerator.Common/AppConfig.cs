using System;
using System.Configuration;

namespace TestDataGenerator.Common
{
    public static class AppConfig
    {
        public static class Account
        {
            private static string ClassName => typeof(Account).Name;

            public static TimeSpan DefaultAccountLockoutTimeSpan =>
                TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings[$"{ClassName}:DefaultAccountLockoutTimeSpan"]));

            public static int SecurityCodeValidMinutes =>
                Convert.ToInt32(ConfigurationManager.AppSettings["Account:SecurityCodeValidMinutes"]);
        }
    }
}