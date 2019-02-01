using ContactS.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Interfaces
{
    public interface IFriendshipManager:IDisposable
    {
        Task Create(Friendship friendship);

        Task<Friendship> GetById(int id);

        Task Update(Friendship friendship);

        Task Delete(Friendship friendship);

        Task Delete(int id);
    }
}
