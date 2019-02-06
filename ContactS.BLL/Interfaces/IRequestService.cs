using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Interfaces
{
    public interface IRequestService : IDisposable
    {
        Task<RequestListDTO> ListRequests(RequestFilter filter, int page = 0);
        Task<RequestDTO> GetRequestById(int id);
        Task<RequestDTO> GetRequestBySenderReceiver(UserDTO sender, UserDTO receiver);

        Task DeleteRequest(RequestDTO request);
        Task DeleteRequest(UserDTO sender, UserDTO receiver);

        Task<int> SendRequest(RequestDTO request);

        Task ConfirmRequest(RequestDTO request);
    }
}
