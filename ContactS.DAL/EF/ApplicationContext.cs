using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ContactS.DAL.Entities;

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

            //modelBuilder.Entity<ClientProfile>()
            //    .HasOptional(u => u.ApplicationUser)
            //    .WithRequired(u => u.ClientProfile);
            //						.WillCascadeOnDelete(true);
        }

        //      private void InitializeDbContext()
        //{
        //	Database.SetInitializer(new AppDbInitializer());
        //}
    }

    //public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    //{
    //    public override void InitializeDatabase(ApplicationContext context)
    //    {
    //        base.InitializeDatabase(context);
    //        //			Init(context);

    //        context.SaveChanges();
    //    }

    //    private static void Init(ApplicationContext context)
    //    {

    //    }
    //}
}

