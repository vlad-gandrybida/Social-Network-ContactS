using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.DTO
{
    public class DialogDTO
    {
        public DialogDTO()
        {
            Users = new List<UserDTO>();
        }

        public List<UserDTO> Users { get; set; }


        public string Name { get; set; }


        public int Id { get; set; }
    }
}
