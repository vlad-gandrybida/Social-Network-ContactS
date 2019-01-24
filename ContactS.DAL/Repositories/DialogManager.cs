using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;

namespace ContactS.DAL.Repositories
{
    public class DialogManager : IDialogManager
    {
        public ApplicationContext Database { get; set; }

        public DialogManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Create(Dialog item)
        {
            Database.Dialogs.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public Dialog GetById(int id)
        {
            return Database.Dialogs.Find(id);
        }

        public void Update(Dialog dialog)
        {
            Database.Entry(dialog).State = EntityState.Modified;
            Database.SaveChanges();
        }

        public void Delete(Dialog dialog)
        {
            Database.Dialogs.Remove(dialog);
            Database.SaveChanges();
        }

        public void Delete(int id)
        {
            Dialog dialog = Database.Dialogs.Find(id);
            Database.Dialogs.Remove(dialog);
            Database.SaveChanges();
        }
    }
}
