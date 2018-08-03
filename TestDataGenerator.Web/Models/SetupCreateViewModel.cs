﻿using System;
using System.Collections.Generic;
using TestDataGenerator.Data.Models;

namespace TestDataGenerator.Web.Models
{
    public class SetupCreateViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public List<IFieldModel> Fields { get; set; } = new List<IFieldModel>();

        public DateTime CreateDate { get; set; }
    }
}