using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);

        IMessageService CreateMessageService(string connection);

        IDialogService CreateDialogService(string connection);
    }
}
