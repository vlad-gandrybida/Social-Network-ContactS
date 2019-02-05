using ContactS.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        IMessageManager MessageManager { get; }
        IDialogManager DialogManager { get; }
        IFriendshipManager FriendshipManager { get; }
        IRequestManager RequestManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
