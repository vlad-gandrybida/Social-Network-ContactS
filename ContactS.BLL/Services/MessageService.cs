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
    public class MessageService : IMessageService
    {
        public int ChatMessagePageSize => 15;

        IUnitOfWork Database { get; set; }

        public MessageService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task DeleteMessage(int messageId)
        {
            Database.MessageManager.Delete(messageId);
            await Database.SaveAsync();
        }

        public async Task DeleteMessage(MessageDTO dialogMessage)
        {
            await DeleteMessage(dialogMessage.Id);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task EditMessage(MessageDTO messageDto)
        {
            var message = Database.MessageManager.GetById(messageDto.Id);
            message.Content = messageDto.Content;
            Database.MessageManager.Update(message);
            await Database.SaveAsync();
        }

        public MessageDTO GetMessageById(int id)
        {
            var message = Database.MessageManager.GetById(id);
            MessageDTO result;
            if (message != null)
            {
                result = new MessageDTO
                {
                    Id = message.Id,
                    Content = message.Content,
                    Time = message.Time
                };
                result.Sender = new UserDTO
                {
                    Id = message.Sender.Id,
                    Name = message.Sender.Name,
                    UserName = message.Sender.ApplicationUser.UserName
                };
                result.Dialog = new DialogDTO
                {
                    Id = message.Dialog.Id,
                    Name = message.Dialog.Name
                };
            }
            else result = null;
            return result;
        }

        public MessageListDTO ListDialogMessages(MessageFilter filter, int page = 0)
        {
            var query = GetMessageQuery(filter);

            query.Skip = (page > 0 ? page - 1 : 0) * ChatMessagePageSize;
            query.Take = ChatMessagePageSize;

            query.AddSortCriteria(s => s.Time, SortDirection.Descending);

            return new MessageListDTO
            {
                RequestedPage = page,
                ResultCount = query.GetTotalRowCount(),
                ResultMessages = query.Execute(),
                Filter = filter
            };
        }

        private IQuery<MessageDTO> GetMessageQuery(MessageFilter filter = null)
        {
            var query = new MessageListQuery((UnitOfWork)Database);
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        public async Task<int> PostMessageToDialog(DialogDTO dialog, UserDTO user, MessageDTO message)
        {
            var dialogEnt = Database.DialogManager.GetById(dialog.Id);
            var userEnt = Database.ClientManager.GetById(user.Id);

            var newMessage = new DAL.Entities.Message {
                Content = message.Content
            };
            newMessage.Time = DateTime.Now;
            newMessage.Sender = userEnt;
            newMessage.Dialog = dialogEnt;
            Database.MessageManager.Create(newMessage);
            await Database.SaveAsync();
            return newMessage.Id;
        }
    }
}
