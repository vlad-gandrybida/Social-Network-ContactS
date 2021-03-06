﻿using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Infrastructure;
using ContactS.DAL.Entities;
using ContactS.DAL.Repositories;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ContactS.BLL.Queries
{
    public class UserListQuery : QueryBase<UserDTO>
    {
        private UnitOfWork Database;

        public UserListQuery(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }
        
        public UserFilter Filter { get; set; }

        protected override IQueryable<UserDTO> GetQueryable()
        {
            IQueryable<ClientProfile> clients = Database.Context.ClientProfiles;

            Database.Context.Users.Load();

            if (!string.IsNullOrEmpty(Filter.Name))
                clients = clients.Where(u => u.Name.ToLower()
                    .Contains(Filter.Name.ToLower()));

            if (!string.IsNullOrEmpty(Filter.Login))
                clients = clients.Where(u => u.ApplicationUser.UserName.ToLower().Contains(Filter.Login.ToLower()));

            if (!string.IsNullOrEmpty(Filter.Email))
                clients = clients.Where(u => u.ApplicationUser.Email.Equals(Filter.Email));

            if (Filter.Address != null)
                clients = clients.Where(u => u.Address.ToLower().Contains(Filter.Address.ToLower()));

            if (clients == null) return null;

            List<UserDTO> result = new List<UserDTO>();

            foreach (ClientProfile client in clients)
            {
                UserDTO user = new UserDTO
                {
                    Id = client.Id,
                    Email = client.ApplicationUser.Email,
                    Address = client.Address,
                    Name = client.Name,
                    UserName = client.ApplicationUser.UserName,
                    Role = Database.UserManager.GetRoles(client.Id).FirstOrDefault()
                };
                result.Add(user);
            }
            return result.AsQueryable();
        }
    }
}
