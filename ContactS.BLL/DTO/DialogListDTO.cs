using ContactS.BLL.DTO.Filtres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
