using ContactS.BLL.DTO.Filtres;
using System.Collections.Generic;

namespace ContactS.BLL.DTO
{
    public class FriendshipListDTO
    {
        public FriendshipFilter Filter { get; set; }

        public int ResultCount { get; set; }

        public IEnumerable<FriendshipDTO> ResultFriendships { get; set; }

        public int RequestedPage { get; set; }
    }
}
