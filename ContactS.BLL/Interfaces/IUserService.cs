using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<int> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task DeleteUser(string id);

        Task EditUser(UserDTO account);
        Task AddUsersToFriends(UserDTO user1Dto, UserDTO user2Dto);
        Task AddUsersToFriends(string id1, string id2);
        Task RemoveUsersFromFriends(UserDTO accountDto, UserDTO user2Dto);
        Task RemoveFriendship(FriendshipDTO friendship);

        
        UserDTO GetUserById(string id);
        UserListDTO ListUsers(UserFilter filter, int page = 0);
        List<UserDTO> ListFriendsOfUser(UserDTO account, int page = 0);

        Task SetInitialData(UserDTO adminDto, List<string> roles);
    }
}
