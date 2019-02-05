using ContactS.ENUM.Request;
using System;

namespace ContactS.BLL.DTO.Filtres
{
    public class RequestFilter
    {
        public RequestType? Type { get; set; }

        public UserDTO Sender { get; set; }

        public UserDTO Receiver { get; set; }

        public DateTime Time { get; set; }

        public RequestStatus? Status { get; set; }
    }
}
