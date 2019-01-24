using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using ContactS.DAL.Identity;

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

        public UnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
            messageManager = new MessageManager(db);
            dialogManager = new DialogManager(db);
            friendshipManager = new FriendshipManager(db);
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public IMessageManager MessageManager
        {
            get { return messageManager; }
        }

        public IDialogManager DialogManager
        {
            get { return dialogManager; }
        }

        public IFriendshipManager FriendshipManager
        {
            get { return friendshipManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public ApplicationContext Context
        {
            get { return db; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
