using ContactS.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Interfaces
{
    public interface IDialogManager:IDisposable
    {
        Task Create(Dialog dialog);

        Task<Dialog> GetById(int id);

        Task Update(Dialog dialog);

        Task Delete(Dialog dialog);

        Task Delete(int id);
    }
}
