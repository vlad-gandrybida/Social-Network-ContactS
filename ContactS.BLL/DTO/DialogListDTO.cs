using ContactS.BLL.DTO.Filtres;
using System.Collections.Generic;

namespace ContactS.BLL.DTO
{
    public class DialogListDTO
    {
        public DialogFilter Filter { get; set; }

        public int ResultCount { get; set; }

        public IEnumerable<DialogDTO> ResultDialogs { get; set; }

        public int RequestedPage { get; set; }
    }
}
