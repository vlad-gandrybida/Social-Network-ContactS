using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ContactS.DAL.Repositories
{
    public class MessageManager : IMessageManager
    {
        public ApplicationContext Database { get; set; }

        public MessageManager(ApplicationContext db)
        {
            Database = db;
            Database.ClientProfiles.Load();
            Database.Users.Load();
            Database.Dialogs.Load();
        }

        public async Task Create(Message item)
        {
            Database.Messages.Add(item);
            await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<Message> GetById(int id)
        {
            return await Database.Messages.FindAsync(id);
        }

        public async Task Update(Message message)
        {
            Database.Entry(message).State = EntityState.Modified;
            await Database.SaveChangesAsync();
        }

        public async Task Delete(Message message)
        {
            Database.Messages.Remove(message);
            await Database.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Message message = await Database.Messages.FindAsync(id);
            Database.Messages.Remove(message);
            await Database.SaveChangesAsync();
        }
    }
}
