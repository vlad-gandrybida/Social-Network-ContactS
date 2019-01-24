using Microsoft.AspNet.Identity.EntityFramework;

namespace ContactS.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
