using ContactS.BLL.DTO;
using System.Collections.Generic;

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
        public UserDTO userInfo { get; set; }
        public int Relation { get; set; }
    }
}
