using ContactS.ENUM.Request;
using System;

namespace ContactS.BLL.DTO
{
    public class RequestDTO
    {
        public int Id { get; set; }

        public RequestType Type { get; set; }

        public UserDTO Sender { get; set; }

        public UserDTO Receiver { get; set; }

        public DateTime Time { get; set; }

        public RequestStatus Status { get; set; }
    }
}
