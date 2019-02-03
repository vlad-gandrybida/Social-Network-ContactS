using ContactS.BLL.DTO;
using ContactS.WEB.Models.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactS.WEB.Models
{
    public class UserListItemModel
    {
        public UserDTO user { get; set; }
        public int IsFriend { get; set; }
    }

    public class SearchModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
    }

    public class SearcModelList
    {
        public SearchModel SearchModel { get; set; }
        public List<UserListItemModel> Users = new List<UserListItemModel>();
    }

    public class ClientProfileViewModel
    {
        public UserViewModel userInfo { get; set; }
        public int Relation { get; set; }
    }
    
    public class UserViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(@"[A-Za-z0-9]+",
                           ErrorMessageResourceType = typeof(Resources.Resource),
                           ErrorMessageResourceName = "WrongUserName")]
        [StringLength(20, MinimumLength = 3,
                      ErrorMessageResourceType = typeof(Resources.Resource),
                      ErrorMessageResourceName = "LoginLengthError")]
        [System.Web.Mvc.Remote("CheckUsername", "Account",
                ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "UserExist")]
        [Display(Name = "Username", ResourceType = typeof(Resources.Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
                           ErrorMessageResourceType = typeof(Resources.Resource),
                           ErrorMessageResourceName = "WrongEmail")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Resource))]
        public string Email { get; set; }

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
        [Display(Name = "Address", ResourceType = typeof(Resources.Resource))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                  ErrorMessageResourceName = "ErrorRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        [Required]
        public string Id { get; set; }
    }
}
