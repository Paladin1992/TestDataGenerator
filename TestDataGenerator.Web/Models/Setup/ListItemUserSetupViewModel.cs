using System;

namespace TestDataGenerator.Web.Models
{
    public class ListItemUserSetupViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public int FieldCount { get; set; }
    }
}