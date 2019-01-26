using System.Collections.Generic;

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
