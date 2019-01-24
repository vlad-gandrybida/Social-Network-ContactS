using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;

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

        public void Create(Message item)
        {
            Database.Messages.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public Message GetById(int id)
        {
            return Database.Messages.Find(id);
        }

        public void Update(Message message)
        {
            Database.Entry(message).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void Delete(Message message)
        {
            Database.Messages.Remove(message);
            Database.SaveChanges();
        }

        public void Delete(int id)
        {
            Message message = Database.Messages.Find(id);
            Database.Messages.Remove(message);
            Database.SaveChanges();
        }
    }
}
