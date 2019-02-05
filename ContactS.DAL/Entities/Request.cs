using ContactS.ENUM.Request;
using System;
using System.ComponentModel.DataAnnotations;

namespace ContactS.DAL.Entities
{
    public class Request
    {
        [Required]
        public virtual ClientProfile Sender { get; set; }

        [Required]
        public virtual ClientProfile Receiver { get; set; }

        public RequestType Type { get; set; }

        public DateTime Time { get; set; }

        public int Id { get; set; }

        public RequestStatus Status { get; set; }
    }
}
