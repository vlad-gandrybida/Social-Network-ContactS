using System;

namespace ContactS.BLL.DTO
{
    public class MessageDTO
    {
        public string Content { get; set; }

        public DateTime Time { get; set; }

        public UserDTO Sender { get; set; }

        public DialogDTO Dialog { get; set; }

        public int Id { get; set; }
    }
}
