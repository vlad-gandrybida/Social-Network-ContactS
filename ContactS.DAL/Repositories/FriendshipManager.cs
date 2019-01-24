using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;

namespace ContactS.DAL.Repositories
{
    public class FriendshipManager : IFriendshipManager
    {
        public ApplicationContext Database { get; set; }
        public FriendshipManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Create(Friendship item)
        {
            Database.Friendships.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public Friendship GetById(int id)
        {
            return Database.Friendships.Find(id);
        }

        public void Update(Friendship friendship)
        {
            Database.Entry(friendship).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void Delete(Friendship friendship)
        {
            Database.Friendships.Remove(friendship);
            Database.SaveChanges();
        }

        public void Delete(int id)
        {
            Friendship friendship = Database.Friendships.Find(id);
            Database.Friendships.Remove(friendship);
            Database.SaveChanges();
        }
    }
}
