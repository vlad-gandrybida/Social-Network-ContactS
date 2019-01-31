using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Infrastructure;
using ContactS.BLL.Interfaces;
using ContactS.BLL.Queries;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using ContactS.DAL.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ContactS.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<int> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByNameAsync(userDto.UserName);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.UserName };
                IdentityResult result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return -1;

                await Database.UserManager.AddToRoleAsync(user.Id, "user");

                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Address = userDto.Address, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool AreUserExist(string userName)
        {
            return (Database.UserManager.FindByName(userName) == null);
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            ApplicationUser user = await Database.UserManager.FindAsync(userDto.UserName, userDto.Password);

            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public async Task DeleteUser(string id)
        {
            ClientProfile user = Database.ClientManager.GetById(id);
            Database.ClientManager.Delete(id);
            Database.UserManager.Delete(Database.UserManager.FindById(id));
            await Database.SaveAsync();
        }

        public async Task EditUser(UserDTO account)
        {
            ClientProfile acc = Database.ClientManager.GetById(account.Id);
            acc.Address = account.Address;
            acc.Name = account.Name;
            Database.ClientManager.Update(acc);
            ApplicationUser user = Database.UserManager.Users.FirstOrDefault(u => u.Id == account.Id);
            if (user == null) return;

            if (account.Email != null)
                user.Email = account.Email;


            if (account.UserName != null)
                user.UserName = account.UserName;

            if (account.Password != null)
                user.PasswordHash = Database.UserManager.PasswordHasher.HashPassword(account.Password);

            if (account.Role != null)
            {
                Database.UserManager.AddToRole(user.Id, "admin");
                Database.UserManager.RemoveFromRole(user.Id, "user");
            }
                

            Database.UserManager.Update(user);
            await Database.SaveAsync();
        }

        public async Task AddUsersToFriends(UserDTO user1Dto, UserDTO user2Dto)
        {
            await AddUsersToFriends(user1Dto.Id, user2Dto.Id);
        }

        public async Task AddUsersToFriends(string id1, string id2)
        {
            ClientProfile user1 = Database.ClientManager.GetById(id1);
            ClientProfile user2 = Database.ClientManager.GetById(id2);

            Database.FriendshipManager.Create(new Friendship(user1, user2));
            await Database.SaveAsync();
        }

        public async Task RemoveUsersFromFriends(UserDTO accountDto, UserDTO user2Dto)
        {
            IQuery<FriendshipDTO> query = GetQuery(new FriendshipFilter { Account = accountDto, Account2 = user2Dto });

            FriendshipDTO frship = query.Execute().FirstOrDefault();
            await RemoveFriendship(frship);
            await Database.SaveAsync();
        }

        public async Task RemoveFriendship(FriendshipDTO friendship)
        {
            Database.FriendshipManager.Delete(Database.FriendshipManager.GetById(friendship.Id));
            await Database.SaveAsync();
        }

        public UserDTO GetUserById(string id)
        {
            ClientProfile client = Database.ClientManager.GetById(id);
            return new UserDTO
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Email = client.ApplicationUser.Email,
                UserName = client.ApplicationUser.UserName,
                Role = client.ApplicationUser.Roles.Any(x=>x.RoleId == "user")?"user":"admin"
            };
        }

        private int UserPageSize => 15;

        public UserListDTO ListUsers(UserFilter filter, int page = 0)
        {
            IQuery<UserDTO> query = GetQuery(filter);
            query.Skip = (page > 0 ? page - 1 : 0) * UserPageSize;
            query.Take = UserPageSize;
            query.AddSortCriteria(x => x.Name);


            return new UserListDTO
            {
                RequestedPage = page,
                ResultCount = query.GetTotalRowCount(),
                ResultUsers = query.Execute(),
                Filter = filter
            };
        }

        public List<UserDTO> ListFriendsOfUser(UserDTO account, int page = 0)
        {
            FriendshipFilter filter = new FriendshipFilter { Account = account };

            IQuery<FriendshipDTO> query = GetQuery(filter);
            query.Skip = (page > 0 ? page - 1 : 0) * UserPageSize;
            query.Take = UserPageSize;


            FriendshipListDTO queryRes = new FriendshipListDTO
            {
                RequestedPage = page,
                ResultCount = query.GetTotalRowCount(),
                ResultFriendships = query.Execute(),
                Filter = filter
            };

            List<UserDTO> retList = new List<UserDTO>();
            foreach (FriendshipDTO friendship in queryRes.ResultFriendships)
            {
                ClientProfile tmpUs = null;


                if (friendship.User1Id.Equals(account.Id)) tmpUs = Database.ClientManager.GetById(friendship.User2Id);
                else if (friendship.User2Id.Equals(account.Id)) tmpUs = Database.ClientManager.GetById(friendship.User1Id);

                retList.Add(new UserDTO
                {
                    Id = tmpUs.Id,
                    Name = tmpUs.Name,
                    Address = tmpUs.Address,
                    Email = tmpUs.ApplicationUser.Email,
                    UserName = tmpUs.ApplicationUser.UserName
                });
            }
            return retList;
        }

        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                ApplicationRole role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private IQuery<UserDTO> GetQuery(UserFilter filter)
        {
            UserListQuery query = new UserListQuery((UnitOfWork)Database)
            {
                Filter = filter
            };
            query.ClearSortCriterias();
            return query;
        }

        private IQuery<FriendshipDTO> GetQuery(FriendshipFilter filter)
        {
            FriendListQuery query = new FriendListQuery((UnitOfWork)Database)
            {
                Filter = filter
            };
            query.ClearSortCriterias();
            return query;
        }

        public bool AreUsersIsFriends(UserDTO User1Id, UserDTO User2Id)
        {
            FriendshipFilter filter = new FriendshipFilter { Account = User1Id, Account2 = User2Id };
            return GetQuery(filter).Execute().Any();
        }

    }
}
