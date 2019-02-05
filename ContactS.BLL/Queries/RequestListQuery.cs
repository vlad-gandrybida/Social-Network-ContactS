using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Infrastructure;
using ContactS.DAL.Entities;
using ContactS.DAL.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ContactS.BLL.Queries
{
    public class RequestListQuery : QueryBase<RequestDTO>
    {
        private UnitOfWork Database;
        public RequestListQuery(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public RequestFilter Filter { get; set; }

        protected override IQueryable<RequestDTO> GetQueryable()
        {
            IQueryable<Request> query = Database.Context.Requests;

            Database.Context.ClientProfiles.Load();
            Database.Context.Users.Load();
            
            if (Filter.Type != null)
                query = query.Where(r => r.Type == Filter.Type);
            if (Filter.Sender != null)
                query = query.Where(r => r.Sender.Id == Filter.Sender.Id);
            if (Filter.Receiver != null)
                query = query.Where(r => r.Receiver.Id == Filter.Receiver.Id);
            if (Filter.Time != null)
                query = query.Where(r => r.Time >= Filter.Time);
            if (Filter.Status != null)
                query = query.Where(r => r.Status == Filter.Status);

            if (query == null) return null;

            List<RequestDTO> result = new List<RequestDTO>();
            
            foreach (Request item in query)
            {
                RequestDTO request = new RequestDTO
                {
                    Id = item.Id,
                    Type = item.Type,
                    Time = item.Time,
                    Status = item.Status
                };
                request.Sender = new UserDTO
                {
                    Id = item.Sender.Id,
                    Name = item.Sender.Name,
                    UserName = item.Sender.ApplicationUser.UserName
                };
                request.Receiver = new UserDTO
                {
                    Id = item.Sender.Id,
                    Name = item.Sender.Name,
                    UserName = item.Sender.ApplicationUser.UserName
                };

                result.Add(request);
            }
            return result.AsQueryable();
        }
    }
}
