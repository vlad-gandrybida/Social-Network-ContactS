using ContactS.DAL.EF;
using ContactS.DAL.Entities;
using ContactS.DAL.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ContactS.DAL.Repositories
{
    public class DialogManager : IDialogManager
    {
        public ApplicationContext Database { get; set; }

        public DialogManager(ApplicationContext db)
        {
            Database = db;
        }

        public async Task Create(Dialog item)
        {
            Database.Dialogs.Add(item);
            await Database.SaveChangesAsync();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<Dialog> GetById(int id)
        {
            return await Database.Dialogs.FindAsync(id);
        }

        public async Task Update(Dialog dialog)
        {
            Database.Entry(dialog).State = EntityState.Modified;
            await Database.SaveChangesAsync();
        }

        public async Task Delete(Dialog dialog)
        {
            Database.Dialogs.Remove(dialog);
            await Database.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Dialog dialog = await Database.Dialogs.FindAsync(id);
            Database.Dialogs.Remove(dialog);
            await Database.SaveChangesAsync();
        }
    }
}
