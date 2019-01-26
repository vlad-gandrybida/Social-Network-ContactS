using ContactS.BLL.DTO.Filtres;
using System.Collections.Generic;

namespace ContactS.BLL.DTO
{
    public class MessageListDTO
    {
        public MessageFilter Filter { get; set; }

        public int ResultCount { get; set; }

        public IEnumerable<MessageDTO> ResultMessages { get; set; }

        public int RequestedPage { get; set; }
    }
}
