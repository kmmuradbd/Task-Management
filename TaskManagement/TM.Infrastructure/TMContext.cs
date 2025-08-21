using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Domain.DomainObject;

namespace TM.Infrastructure
{
   
    public class TMContext : DbContext
    {
        static TMContext()
        {
            Database.SetInitializer<TMContext>(null);
        }

        public TMContext()
            : base("Name=TMConnection")
        {
            this.Database.CommandTimeout = 380;
        }

        #region Entity
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<MasterMenu> MasterMenus { get; set; }
        public DbSet<Project> Projects { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
