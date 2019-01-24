using ContactS.DAL.Entities;
using System;

namespace ContactS.DAL.Interfaces
{
    public interface IMessageManager:IDisposable
    {
        void Create(Message message);

        Message GetById(int id);

        void Update(Message message);

        void Delete(Message message);

        void Delete(int id);
    }
}
