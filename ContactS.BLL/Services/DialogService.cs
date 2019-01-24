using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Infrastructure;
using ContactS.BLL.Interfaces;
using ContactS.BLL.Queries;
using ContactS.DAL.Interfaces;
using ContactS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Services
{
    public class DialogService : IDialogService
    {
        public int DialogPageSize => 20;

        IUnitOfWork Database { get; set; }

        public DialogService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task AddUsersToDialog(DialogDTO dialog, List<UserDTO> accounts)
        {
            foreach(var acc in accounts)
            {
                await AddUserToDialog(dialog, acc);
            }
        }

        public async Task AddUserToDialog(DialogDTO dialog, UserDTO account)
        {
            var dialogEnt = Database.DialogManager.GetById(dialog.Id);
            var userEnt = Database.ClientManager.GetById(account.Id);
            dialogEnt.ChatUsers.Add(userEnt);
            Database.DialogManager.Update(dialogEnt);
            await Database.SaveAsync();
        }

        public async Task<int> CreateDialog(DialogDTO dialogDto)
        {
            int id;
            if (dialogDto.Users.Count == 2)
            {
                var tmp = CheckIfPrivateDialogExists(dialogDto);
                if (tmp != -1) return tmp;
            }
            var list = dialogDto.Users
                .Select(User => Database.ClientManager.GetById(User.Id))
                .ToList();

            var dialogName = new StringBuilder();
            if (String.IsNullOrEmpty(dialogDto.Name))
            {
                list.ForEach(u => dialogName.Append(u.Name + ", "));
                dialogDto.Name = dialogName.ToString();
            }
            var dialogEnt = new DAL.Entities.Dialog
            {
                Name = dialogDto.Name
            };
            dialogEnt.ChatUsers.AddRange(list);

            Database.DialogManager.Create(dialogEnt);
            await Database.SaveAsync();
            id = dialogEnt.Id;
            return id;
        }

        public async Task DeleteDialog(int dialogId)
        {
            Database.DialogManager.Delete(dialogId);
            await Database.SaveAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task EditDialogName(DialogDTO dialogDto)
        {
            var dialog = Database.DialogManager.GetById(dialogDto.Id);
            dialog.Name = dialogDto.Name;
            Database.DialogManager.Update(dialog);
            await Database.SaveAsync();
        }

        public DialogDTO GetDialogById(int Id)
        {
            var dialog = Database.DialogManager.GetById(Id);

            var result = new DialogDTO
            {
                Id = dialog.Id,
                Name = dialog.Name
            };

            dialog.ChatUsers.ForEach(m => result.Users.Add(new UserDTO {
                Id = m.Id,
                Email = m.ApplicationUser.Email,
                Address = m.Address,
                Name = m.Name,
                UserName = m.ApplicationUser.UserName
            }));

            return result;
        }

        public List<UserDTO> GetUsersInDialog(DialogDTO dialog)
        {
            var dialogEnt = Database.DialogManager.GetById(dialog.Id);

            List<UserDTO> result = new List<UserDTO>();
            dialogEnt.ChatUsers.ForEach(m => result.Add(new UserDTO
            {
                Id = m.Id,
                Email = m.ApplicationUser.Email,
                Address = m.Address,
                Name = m.Name,
                UserName = m.ApplicationUser.UserName
            }));
            return result;
        }

        public DialogListDTO ListDialogs(DialogFilter filter, int page = 0)
        {
            var query = GetChatQuery(filter);
            query.Skip = (page > 0 ? page - 1 : 0) * DialogPageSize;
            query.Take = DialogPageSize;

            query.AddSortCriteria(s => s.Name, SortDirection.Descending);
            return new DialogListDTO
            {
                RequestedPage = page,
                ResultCount = query.GetTotalRowCount(),
                ResultDialogs = query.Execute(),
                Filter = filter
            };
        }

        public async Task RemoveUserFromDialog(DialogDTO dialog, UserDTO account)
        {
            var dialogEnt = Database.DialogManager.GetById(dialog.Id);
            var userEnt = Database.ClientManager.GetById(account.Id);

            dialogEnt.ChatUsers.Remove(userEnt);

            if (dialogEnt.ChatUsers.Count == 0)
            {
                Database.DialogManager.Delete(dialogEnt);
                return;
            }
            Database.DialogManager.Update(dialogEnt);
            await Database.SaveAsync();
        }

        private IQuery<DialogDTO> GetChatQuery(DialogFilter filter)
        {
            var query = new DialogListQuery((UnitOfWork)Database);
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        private int CheckIfPrivateDialogExists(DialogDTO privateChat)
        {
            var tmpChatList = ListDialogs(new DialogFilter { Account = privateChat.Users.FirstOrDefault() });
            foreach (var chatTmp in tmpChatList.ResultDialogs)
            {
                if (chatTmp.Users.Count != 2) continue;
                if (chatTmp.Users.Contains(privateChat.Users[0])
                    && chatTmp.Users.Contains(privateChat.Users[1]))
                    return chatTmp.Id;
            }

            return -1;
        }
    }
}
