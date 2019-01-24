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
    class MessageListQuery : QueryBase<MessageDTO>
    {
        UnitOfWork Database;
        public MessageListQuery(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public MessageFilter Filter { get; set; }

        protected override IQueryable<MessageDTO> GetQueryable()
        {
            IQueryable<Message> query = Database.Context.Messages;

            Database.Context.ClientProfiles.Load();
            Database.Context.Dialogs.Load();
            Database.Context.Users.Load();

            if (!string.IsNullOrEmpty(Filter.Text))
                query = query.Where(u => u.Content.ToLower()
                    .Contains(Filter.Text.ToLower()));

            if (Filter.Sender != null)
                query = query.Where(u => u.Sender.Id == Filter.Sender.Id);
            if (Filter.Chat != null)
                query = query.Where(c => c.Dialog.Id == Filter.Chat.Id);

            List<MessageDTO> result = new List<MessageDTO>();

            foreach (var item in query)
            {
                MessageDTO message = new MessageDTO
                {
                    Id = item.Id,
                    Content = item.Content,
                    Time = item.Time
                };
                message.Sender = new UserDTO
                {
                    Id = item.Sender.Id,
                    Name = item.Sender.Name,
                    UserName = item.Sender.ApplicationUser.UserName
                };
                message.Dialog = new DialogDTO
                {
                    Id = item.Dialog.Id,
                    Name = item.Dialog.Name
                };

                result.Add(message);
            }

            return result.AsQueryable();
        }
    }
}
