using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContactS.DAL.Entities
{
    public class Dialog
    {
        public Dialog()
        {
            ChatUsers = new List<ClientProfile>();
            Messages = new List<Message>();
        }

        public virtual List<ClientProfile> ChatUsers { get; set; }

        public virtual List<Message> Messages { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }


        public int Id { get; set; }
    }
}
