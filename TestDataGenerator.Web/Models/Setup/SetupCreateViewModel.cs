﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestDataGenerator.Resources;

namespace TestDataGenerator.Web.Models
{
    public class SetupCreateViewModel
    {
        public string Id { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_StringLength")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_Required")]
        [Display(Name = "Név")]
        public string Name { get; set; }

        //public string Email { get; set; }

        [Display(Name = "Mezők")]
        public List<TemporarySetupViewModel> Fields { get; set; } = new List<TemporarySetupViewModel>();

        public DateTime CreateDate { get; set; }
    }
}