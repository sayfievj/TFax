using System.ComponentModel.DataAnnotations;

namespace TFax.Web.CORE.COMMON.ViewModels
{
    public class LoginViewModel
    {
        [Required] 
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required] 
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}