using ContactS.DAL.Entities;
using System;
using System.Threading.Tasks;

namespace ContactS.DAL.Interfaces
{
    public interface IClientManager:IDisposable
    {
        Task Create(ClientProfile clientProfile);

        Task<ClientProfile> GetById(string id);

        Task Update(ClientProfile clientProfile);

        Task Delete(ClientProfile clientProfile);

        Task Delete(string id);
    }
}
