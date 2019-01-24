using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Infrastructure;
using ContactS.DAL.Entities;
using ContactS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Queries
{
    public class DialogListQuery : QueryBase<DialogDTO>
    {
        UnitOfWork Database;
        public DialogListQuery(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public DialogFilter Filter { get; set; }

        protected override IQueryable<DialogDTO> GetQueryable()
        {
            Database.Context.ClientProfiles.Load();
            Database.Context.Users.Load();
            Database.Context.Dialogs.Load();

            IQueryable<Dialog> query = Database.Context.Dialogs.Local.AsQueryable();

            

            if (!string.IsNullOrEmpty(Filter.Name))
                query = query.Where(u => u.Name.ToLower()
                    .Contains(Filter.Name.ToLower()));

            if (Filter.Account != null)
                query = query.Where(u => u.ChatUsers
                    .Contains(Database.Context.ClientProfiles
                        .FirstOrDefault(user => user.Id == Filter.Account.Id)));

            List<DialogDTO> result = new List<DialogDTO>();
            foreach (var elem in query)
            {
                DialogDTO dialog = new DialogDTO
                {
                    Id = elem.Id,
                    Name = elem.Name
                };
                foreach (var client in elem.ChatUsers)
                {
                    UserDTO user = new UserDTO
                    {
                        Id = client.Id,
                        Email = client.ApplicationUser.Email,
                        Address = client.Address,
                        Name = client.Name,
                        UserName = client.ApplicationUser.UserName
                    };
                    dialog.Users.Add(user);
                }
                result.Add(dialog);
            }

            return result.AsQueryable();
        }
    }
}
