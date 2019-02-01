using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ContactS.DAL.Repositories
{
    public class FriendshipManager : IFriendshipManager
    {
        public ApplicationContext Database { get; set; }
        public FriendshipManager(ApplicationContext db)
        {
            Database = db;
        }

        public async Task Create(Friendship item)
        {
            Database.Friendships.Add(item);
            await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<Friendship> GetById(int id)
        {
            return await Database.Friendships.FindAsync(id);
        }

        public async Task Update(Friendship friendship)
        {
            Database.Entry(friendship).State = EntityState.Modified;
            await Database.SaveChangesAsync();
        }

        public async Task Delete(Friendship friendship)
        {
            Database.Friendships.Remove(friendship);
            await Database.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Friendship friendship = await Database.Friendships.FindAsync(id);
            Database.Friendships.Remove(friendship);
            await Database.SaveChangesAsync();
        }
    }
}
