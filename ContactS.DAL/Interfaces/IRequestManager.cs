using ContactS.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Interfaces
{
    public interface IRequestManager : IDisposable
    {
        Task Create(Request request);

        Task<Request> GetById(int id);

        Task Update(Request request);

        Task Delete(Request request);

        Task Delete(int id);
    }
}
