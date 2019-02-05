using ContactS.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace ContactS.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString)
        {
            //InitializeDbContext();
        }

        public ApplicationContext() : base("DefaultConnection")
        {
            //InitializeDbContext();
        }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendship>()
                .HasRequired(r => r.User1)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Friendship>()
                .HasRequired(r => r.User2)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>()
                .HasRequired(r => r.Receiver)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>()
                .HasRequired(r => r.Sender)
                .WithMany()
                .WillCascadeOnDelete(true);


            //modelBuilder.Entity<ClientProfile>()
            //    .HasOptional(u => u.ApplicationUser)
            //    .WithRequired(u => u.ClientProfile)
            //	  .WillCascadeOnDelete(true);
        }
    }
}

