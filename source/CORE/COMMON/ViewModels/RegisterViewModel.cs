using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TFax.Web.CORE.COMMON.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "E-mail")] 
        public string UserName { get; set; }

        [Required] 
        [Display(Name = "Confirm E-mail")] 
        [Compare("UserName", ErrorMessage = "The E-mail and confirmation E-mail do not match.")]
        public string ConfirmUserName { get; set; }

        [Required] 
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required] 
        [Display(Name = "Account type")]
        public long? RoleId { get; set; }

    }
}
