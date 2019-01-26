using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;

namespace ContactS.DAL.Entities
{
    public class Friendship
    {
        [Required]
        public virtual ClientProfile User1 { get; set; }

        [Required]
        public virtual ClientProfile User2 { get; set; }

        public int Id { get; set; }

        #region Methods

        protected bool Equals(Friendship other)
        {
            return (User1.Equals(other.User1) && User2.Equals(other.User2)) ||
                    (User2.Equals(other.User1) && User1.Equals(other.User2));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return ((obj.GetType() == GetType()) || (obj.GetType() == ObjectContext.GetObjectType(GetType()))) &&
                    Equals((Friendship)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                hash = hash * 31 + User1.GetHashCode();
                hash = hash * 31 + User2.GetHashCode();
                return hash;
            }
        }


        public Friendship(ClientProfile user1, ClientProfile user2)
        {
            if ((user1 == null) || (user2 == null)) return;
            if (user1.Equals(user2)) return;

            User1 = user1;
            User2 = user2;
        }

        public Friendship()
        {

        }

        #endregion
    }
}
