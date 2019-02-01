using ContactS.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Interfaces
{
    public interface IMessageManager:IDisposable
    {
        Task Create(Message message);

        Task<Message> GetById(int id);

        Task Update(Message message);

        Task Delete(Message message);

        Task Delete(int id);
    }
}
