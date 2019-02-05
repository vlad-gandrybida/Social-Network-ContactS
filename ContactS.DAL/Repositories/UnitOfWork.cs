using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Identity;
using ContactS.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;
        private IMessageManager messageManager;
        private IDialogManager dialogManager;
        private IFriendshipManager friendshipManager;
        private IRequestManager requestManager;

        public UnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
            messageManager = new MessageManager(db);
            dialogManager = new DialogManager(db);
            friendshipManager = new FriendshipManager(db);
            requestManager = new RequestManager(db);
        }

        public ApplicationUserManager UserManager => userManager;

        public IClientManager ClientManager => clientManager;

        public IMessageManager MessageManager => messageManager;

        public IDialogManager DialogManager => dialogManager;

        public IFriendshipManager FriendshipManager => friendshipManager;

        public IRequestManager RequestManager => requestManager;

        public ApplicationRoleManager RoleManager => roleManager;

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public ApplicationContext Context => db;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                    dialogManager.Dispose();
                    friendshipManager.Dispose();
                }
                disposed = true;
            }
        }
    }
}
