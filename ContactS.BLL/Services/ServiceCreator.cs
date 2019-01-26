using ContactS.BLL.Interfaces;
using ContactS.DAL.Repositories;

namespace ContactS.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IDialogService CreateDialogService(string connection)
        {
            return new DialogService(new UnitOfWork(connection));
        }

        public IMessageService CreateMessageService(string connection)
        {
            return new MessageService(new UnitOfWork(connection));
        }

        public IUserService CreateUserService(string connection)
        {
            return new UserService(new UnitOfWork(connection));
        }
    }
}
