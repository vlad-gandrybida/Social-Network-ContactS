using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.ENUM.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactS.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<int> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task<bool> AreUserExist(string userName);
        Task DeleteUser(string id);

        Task EditUser(UserDTO account);
        Task AddUsersToFriends(UserDTO user1Dto, UserDTO user2Dto);
        Task AddUsersToFriends(string id1, string id2);
        Task RemoveUsersFromFriends(UserDTO accountDto, UserDTO user2Dto);
        Task RemoveFriendship(FriendshipDTO friendship);


        Task<UserDTO> GetUserById(string id);
        Task<UserListDTO> ListUsers(UserFilter filter, int page = 0);
        Task<List<UserDTO>> ListFriendsOfUser(UserDTO account, int page = 0);
        Task<bool> AreUsersIsFriends(UserDTO User1Id, UserDTO User2Id);

        Task SetInitialData(UserDTO adminDto, List<string> roles);
        Task<FriendshipStatus> FriendshipStatus(string id, string v);
    }
}
