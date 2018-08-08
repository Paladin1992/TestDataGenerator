using System.ComponentModel.DataAnnotations;
using TestDataGenerator.Resources;

namespace TestDataGenerator.Web.Models
{
    public class RegisterViewModel : LoginViewModel
    {
        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_StringLength")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_Required")]
        [Display(Name = "Teljes név")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_StringLength")]
        [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Error_Required")]
        [Compare("Password")]
        [Display(Name = "Jelszó ismét")]
        public string ConfirmPassword { get; set; }
    }
}