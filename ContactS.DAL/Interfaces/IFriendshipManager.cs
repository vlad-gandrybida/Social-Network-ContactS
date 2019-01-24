using ContactS.DAL.Entities;
using System;

namespace ContactS.DAL.Interfaces
{
    public interface IFriendshipManager:IDisposable
    {
        void Create(Friendship friendship);

        Friendship GetById(int id);

        void Update(Friendship friendship);

        void Delete(Friendship friendship);

        void Delete(int id);
    }
}
