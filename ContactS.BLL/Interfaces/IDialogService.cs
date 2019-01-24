using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Interfaces
{
    public interface IDialogService : IDisposable
    {
        Task<int> CreateDialog(DialogDTO DialogDto);

        Task DeleteDialog(int DialogId);


        Task EditDialogName(DialogDTO DialogDto);

        Task RemoveUserFromDialog(DialogDTO Dialog, UserDTO account);

        Task AddUserToDialog(DialogDTO Dialog, UserDTO account);
        Task AddUsersToDialog(DialogDTO Dialog, List<UserDTO> accounts);

        DialogDTO GetDialogById(int id);

        DialogListDTO ListDialogs(DialogFilter filter, int page = 0);

        List<UserDTO> GetUsersInDialog(DialogDTO Dialog);
    }
}
