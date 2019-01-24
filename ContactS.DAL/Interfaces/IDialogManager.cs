using ContactS.DAL.Entities;
using System;

namespace ContactS.DAL.Interfaces
{
    public interface IDialogManager:IDisposable
    {
        void Create(Dialog dialog);

        Dialog GetById(int id);

        void Update(Dialog dialog);

        void Delete(Dialog dialog);

        void Delete(int id);
    }
}
