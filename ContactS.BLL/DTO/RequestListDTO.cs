using ContactS.BLL.DTO.Filtres;
using System.Collections.Generic;

namespace ContactS.BLL.DTO
{
    public class RequestListDTO
    {
        public RequestFilter Filter { get; set; }

        public int ResultCount { get; set; }

        public IEnumerable<RequestDTO> ResultRequests { get; set; }

        public int RequestedPage { get; set; }
    }
}
