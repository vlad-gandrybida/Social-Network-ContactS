using System;
using ContactS.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace ContactS.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        {
        }
    }
}
