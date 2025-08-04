using Microsoft.AspNet.Identity.EntityFramework;
using Hec.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec
{
    public class HecContext : IdentityDbContext<User>
    {
        public HecContext() : base("HecContext")
        { }

        //-------------------------------------------------------------------------------- 

        public virtual DbSet<Settings.KeyValue> KeyValues { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<OffDay> OffDays { get; set; }
        public virtual DbSet<EmailQueue> EmailQueues { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public new virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<HelpFile> HelpFiles { get; set; }
        public virtual DbSet<TipCategory> TipCategories { get; set; }
        public virtual DbSet<Tip> Tips { get; set; }
        public virtual DbSet<Appliance> Appliances { get; set; }
        public virtual DbSet<UserTip> UserTips { get; set; }
        public virtual DbSet<ContractAccount> ContractAccounts { get; set; }
        public virtual DbSet<HouseType> HouseTypes { get; set; }
        public virtual DbSet<HouseCategory> HouseCategories { get; set; }

        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Tariff> Tariffs { get; set; }

        //-------------------------------------------------------------------------------- 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));

            // Disable automatic cascade delete
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        //-------------------------------------------------------------------------------- 

        public virtual void SetModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetDeleted(object entity)
        {
            this.Entry(entity).State = EntityState.Deleted;
        }

        public virtual IQueryable<T> Query<T>(Func<T, bool> q) where T : Entity
        {
            return this.Set<T>().Where(q).AsQueryable<T>();
        }

        private void SetCreatedOnAndUpdatedOn()
        {
            //
            // Automatically set CreatedOn and UpdatedOn value when SaveChanges.
            //

            foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity.GetType().GetProperty("CreatedOn") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedOn").CurrentValue = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Ignore the CreatedTime updates on Modified entities. 
                    entry.Property("CreatedOn").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(
                e =>
                    e.Entity.GetType().GetProperty("UpdatedOn") != null &&
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Added))
            {
                entry.Property("UpdatedOn").CurrentValue = DateTime.Now;
            }
        }

        public override int SaveChanges()
        {
            SetCreatedOnAndUpdatedOn();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            SetCreatedOnAndUpdatedOn();
            return base.SaveChangesAsync();
        }
    }
}
