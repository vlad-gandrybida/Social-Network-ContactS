using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ContactS.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Database { get; set; }

        public ClientManager(ApplicationContext db)
        {
            Database = db;
        }

        public async Task Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<ClientProfile> GetById(string id)
        {
            return await Database.ClientProfiles.FindAsync(id);
        }

        public async Task Update(ClientProfile clientProfile)
        {
            Database.Entry(clientProfile).State = EntityState.Modified;
            await Database.SaveChangesAsync();
        }

        public async Task Delete(ClientProfile clientProfile)
        {
            Database.ClientProfiles.Remove(clientProfile);
            await Database.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            ClientProfile clientProfile = await Database.ClientProfiles.FindAsync(id);
            Database.ClientProfiles.Remove(clientProfile);
            await Database.SaveChangesAsync();
        }
    }
}
