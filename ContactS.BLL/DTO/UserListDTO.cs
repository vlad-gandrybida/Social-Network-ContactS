using ContactS.BLL.DTO.Filtres;
using System.Collections.Generic;

namespace ContactS.BLL.DTO
{
    public class UserListDTO
    {
        public UserFilter Filter { get; set; }

        public int ResultCount { get; set; }

        public IEnumerable<UserDTO> ResultUsers { get; set; }

        public int RequestedPage { get; set; }
    }
}
