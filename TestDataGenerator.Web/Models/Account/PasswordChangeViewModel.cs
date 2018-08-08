using System.ComponentModel.DataAnnotations;
using TestDataGenerator.Resources;

namespace TestDataGenerator.Web.Models
{
    public class PasswordChangeViewModel : LoginViewModel
    { 
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_StringLength")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_Required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}