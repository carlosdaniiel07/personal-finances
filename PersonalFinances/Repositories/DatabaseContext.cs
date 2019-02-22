using System.Data.Entity;
using PersonalFinances.Models;

using System.Data.Entity.ModelConfiguration.Conventions;

namespace PersonalFinances.Repositories
{
    public class DatabaseContext : DbContext 
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DatabaseContext() 
            : base("ConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}