using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using System;
using System.Threading.Tasks;

namespace ContactS.BLL.Interfaces
{
    public interface IMessageService : IDisposable
    {
        Task<int> PostMessageToDialog(DialogDTO Dialog, UserDTO User, MessageDTO message);

        Task EditMessage(MessageDTO messageDto);

        Task DeleteMessage(int messageId);

        Task DeleteMessage(MessageDTO DialogMessage);

        Task<MessageDTO> GetMessageById(int id);

        Task<MessageListDTO> ListDialogMessages(MessageFilter filter, int page = 0);
    }
}
