using ContactS.WEB.Models.Filters;
using System.ComponentModel.DataAnnotations;

namespace ContactS.WEB.Models
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(@"[A-Za-z0-9]+",
                           ErrorMessageResourceType = typeof(Resources.Resource),
                           ErrorMessageResourceName = "WrongUserName")]
        [StringLength(20, MinimumLength = 3,
                      ErrorMessageResourceType = typeof(Resources.Resource),
                      ErrorMessageResourceName = "LoginLengthError")]
        [Display(Name = "Username", ResourceType = typeof(Resources.Resource))]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [StringLength(16, MinimumLength = 8,
                      ErrorMessageResourceType = typeof(Resources.Resource),
                      ErrorMessageResourceName = "PasswordLengthError")]
        [Password(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "WrongPassword")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}