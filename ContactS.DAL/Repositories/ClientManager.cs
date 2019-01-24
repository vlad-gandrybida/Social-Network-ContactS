using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;

namespace ContactS.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Database { get; set; }

        public ClientManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public ClientProfile GetById(string id)
        {
            return Database.ClientProfiles.Find(id);
        }

        public void Update(ClientProfile clientProfile)
        {
            Database.Entry(clientProfile).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void Delete(ClientProfile clientProfile)
        {
            Database.ClientProfiles.Remove(clientProfile);
            Database.SaveChanges();
        }

        public void Delete(string id)
        {
            ClientProfile clientProfile = Database.ClientProfiles.Find(id);
            Database.ClientProfiles.Remove(clientProfile);
            Database.SaveChanges();
        }
    }
}
