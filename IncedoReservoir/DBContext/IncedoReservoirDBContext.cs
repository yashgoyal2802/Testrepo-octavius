
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace IncedoReservoir.DBContext
{
    public class IncedoReservoirDBContext : DbContext
    {
        public IncedoReservoirDBContext()
            : base("name=IncedoConnection")
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 300;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<PrivilegeAvilable> PrivilegeAvilable { get; set; }
        public virtual DbSet<ServiceDefinition> ServiceDefinition { get; set; }
        public virtual DbSet<PrivilegeGroup> PrivilegeGroup { get; set; }
        public virtual DbSet<ServiceAndPrivilegeLevelMapping> ServiceAndPrivilegeLevelMapping { get; set; }
        public virtual DbSet<AccountUsers> AccountUsers { get; set; }
        public virtual DbSet<ServiceAccountUserPrivilegeGroupMapping> ServiceAccountUserPrivilegeGroupMapping { get; set; }
        public virtual DbSet<MasterLocation> MasterLocation { get; set; }
        public virtual DbSet<MasterServices> MasterServices { get; set; }
        public virtual DbSet<DevicesDetails> DevicesDetails { get; set; }
        public virtual DbSet<PrivilegeGroupMasterServicesReln> PrivilegeGroupMasterServicesReln { get; set; }
    }
}