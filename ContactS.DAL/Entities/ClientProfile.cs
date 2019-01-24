using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactS.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual List<Dialog> Dialogs { get; set; }

        public ClientProfile()
        {
            Dialogs = new List<Dialog>();
        }

        private bool Equals(ClientProfile other)
        {
            return other.ApplicationUser.Equals(ApplicationUser);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return (obj.GetType() == GetType()) && Equals((ClientProfile)obj);
        }

        public override int GetHashCode()
        {
            return ApplicationUser.GetHashCode();
        }
    }
}
