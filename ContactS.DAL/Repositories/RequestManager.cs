using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ContactS.DAL.Repositories
{
    public class RequestManager : IRequestManager
    {
        public ApplicationContext Database { get; set; }

        public RequestManager(ApplicationContext db)
        {
            Database = db;
            Database.ClientProfiles.Load();
            Database.Users.Load();
        }

        public async Task Create(Request request)
        {
            Database.Requests.Add(request);
            await Database.SaveChangesAsync();
        }

        public async Task Delete(Request request)
        {
            Database.Requests.Remove(request);
            await Database.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Request request = await Database.Requests.FindAsync(id);
            Database.Requests.Remove(request);
            await Database.SaveChangesAsync();
        }

        public async Task<Request> GetById(int id)
        {
            return await Database.Requests.FindAsync(id);
        }

        public async Task Update(Request request)
        {
            Database.Entry(request).State = EntityState.Modified;
            await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
