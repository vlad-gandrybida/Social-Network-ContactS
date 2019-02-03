using ContactS.WEB.Models.Filters;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace ContactS.WEB.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "ErrorRequired",
            ErrorMessageResourceType = typeof(Resources.Resource)
                  )]
        [RegularExpression(@"[A-Za-z0-9]+",
                           ErrorMessageResourceType = typeof(Resources.Resource),
                           ErrorMessageResourceName = "WrongUserName")]
        [StringLength(20, MinimumLength = 3,
                      ErrorMessageResourceType = typeof(Resources.Resource),
                      ErrorMessageResourceName = "LoginLengthError")]
        [Remote("CheckUsername", "Account",
                ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "UserExist")]
        [Display (Name = "Username", ResourceType = typeof(Resources.Resource))]
        public string Login { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", 
                           ErrorMessageResourceType = typeof(Resources.Resource),
                           ErrorMessageResourceName = "WrongEmail")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

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

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Resource))]
        [DataType(DataType.Password)]
        [Compare("Password", 
                  ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "PasswordsDoNotMatch")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }
    }
}