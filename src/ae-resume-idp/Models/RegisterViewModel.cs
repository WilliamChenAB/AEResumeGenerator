using System.ComponentModel.DataAnnotations;

namespace aeresumeidp.Core.Models
{
    public class RegisterViewModel : RegisterModel
    {

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
