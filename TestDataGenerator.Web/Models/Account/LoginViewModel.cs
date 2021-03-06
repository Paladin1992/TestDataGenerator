﻿using System.ComponentModel.DataAnnotations;
using TestDataGenerator.Resources;

namespace TestDataGenerator.Web.Models
{
    public class LoginViewModel
    {
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_StringLength")]
        [EmailAddress(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_EmailAddress_Invalid")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_Required")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_StringLength")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_Required")]
        [Display(Name = "Jelszó")]
        public string Password { get; set; }
    }
}