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
    public class RequestService : IRequestService
    {
        public int RequestPageSize => 15;

        private IUnitOfWork Database { get; set; }

        public RequestService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task DeleteRequest(UserDTO sender, UserDTO receiver)
        {
            RequestFilter requestFilter = new RequestFilter { Sender = sender, Receiver = receiver };
            RequestDTO requestDTO = (GetQuery(requestFilter).Execute()).FirstOrDefault();
            if (requestDTO!=null)
            {
                var req = await Database.RequestManager.GetById(requestDTO.Id);
                await Database.RequestManager.Delete(req);
                await Database.SaveAsync();
            }
            return;
        }

        public async Task DeleteRequest(RequestDTO request)
        {
            var req = await Database.RequestManager.GetById(request.Id);
            await Database.RequestManager.Delete(req);
            await Database.SaveAsync();
        }

        public async Task<RequestDTO> GetRequestById(int id)
        {
            var request = await Database.RequestManager.GetById(id);
            RequestDTO result;
            if (request != null)
            {
                result = new RequestDTO
                {
                    Id = request.Id,
                    Type = request.Type,
                    Time = request.Time,
                    Status = request.Status
                };
                result.Sender = new UserDTO
                {
                    Id = request.Sender.Id,
                    Name = request.Sender.Name,
                    UserName = request.Sender.ApplicationUser.UserName
                };
                result.Receiver = new UserDTO
                {
                    Id = request.Receiver.Id,
                    Name = request.Receiver.Name,
                    UserName = request.Receiver.ApplicationUser.UserName
                };
            }
            else result = null;
            return result;
        }

        public async Task ConfirmRequest(RequestDTO requestDto)
        {
            DAL.Entities.Request request = await Database.RequestManager.GetById(requestDto.Id);
            request.Status = requestDto.Status;
            await Database.RequestManager.Update(request);
            await Database.SaveAsync();
        }

        public async Task<RequestListDTO> ListRequests(RequestFilter filter, int page = 0)
        {
            var query = GetQuery(filter);
            query.Skip = (page > 0 ? page - 1 : 0) * RequestPageSize;
            query.Take = RequestPageSize;

            query.AddSortCriteria(req => req.Time, SortDirection.Descending);
            
            return new RequestListDTO
            {
                RequestedPage = page,
                ResultCount = query.GetTotalRowCount(),
                ResultRequests = query.Execute(),
                Filter = filter
            };
        }

        public async Task<int> SendRequest(RequestDTO request)
        {
            var req = new DAL.Entities.Request
            {
                Time = DateTime.Now,
                Receiver = await Database.ClientManager.GetById(request.Receiver.Id),
                Sender = await Database.ClientManager.GetById(request.Sender.Id),
                Type = request.Type,
                Status = ENUM.Request.RequestStatus.Sended
            };

            await Database.RequestManager.Create(req);
            await Database.SaveAsync();
            return req.Id;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        private IQuery<RequestDTO> GetQuery(RequestFilter filter)
        {
            var query = new RequestListQuery((UnitOfWork)Database);
            query.ClearSortCriterias();
            query.Filter = filter;
            return query;
        }

        public async Task<RequestDTO> GetRequestBySenderReceiver(UserDTO sender, UserDTO receiver)
        {
            RequestFilter requestFilter = new RequestFilter { Sender = sender, Receiver = receiver };
            var query = GetQuery(requestFilter);
            query.AddSortCriteria(req => req.Time, SortDirection.Descending);
            return query.Execute().FirstOrDefault();
        }
    }
}
