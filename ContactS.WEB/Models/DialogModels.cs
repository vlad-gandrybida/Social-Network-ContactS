using ContactS.BLL.DTO;
using System.Collections.Generic;

namespace ContactS.WEB.Models
{
    public class OpenDialogModel
    {
        public DialogDTO Dialog { get; set; }

        public List<MessageDTO> Messages { get; set; }

        public string Messages_Code { get; set; }

        public MessageDTO NewMessage { get; set; }

        public int DialogId { get; set; }

        public int Page { get; set; }

        public List<UserDTO> Accounts { get; set; }
    }

    public class DialogViewModel
    {
        public int DialogId { get; set; }
        public string Dialog { get; set; }
        public string Data { get; set; }
        public string LastMessage { get; set; }
        public string Sender { get; set; }
    }

    public class DialogListModel
    {
        public List<DialogViewModel> dialogs = new List<DialogViewModel>();
    }

    public class MessageViewModel
    {
        public string Content { get; set; }
        public int Id { get; set; }
    }

    public class RemovePeopleModel
    {
        public List<SelectModel> Accounts { get; set; }
        public DialogDTO Dialog { get; set; }
    }

    public class SelectModel
    {
        public UserDTO User { get; set; }
        public bool Invited { get; set; }
    }
}