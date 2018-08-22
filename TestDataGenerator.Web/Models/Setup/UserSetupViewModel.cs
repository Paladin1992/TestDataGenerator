using System;
using System.Collections.Generic;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Web.Models
{
    public class UserSetupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<FieldModel> Fields { get; set; } = new List<FieldModel>();

        public DateTime CreateDate { get; set; }
    }
}