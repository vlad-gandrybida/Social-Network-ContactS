using ContactS.DAL.Entities;
using System;

namespace ContactS.DAL.Interfaces
{
    public interface IClientManager:IDisposable
    {
        void Create(ClientProfile clientProfile);

        ClientProfile GetById(string id);

        void Update(ClientProfile clientProfile);

        void Delete(ClientProfile clientProfile);

        void Delete(string id);
    }
}
