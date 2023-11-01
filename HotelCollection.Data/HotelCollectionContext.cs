using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using HotelCollection.Data.Entity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HotelCollection.Data
{
    public class HotelCollectionContext : IdentityDbContext<ApplicationUser,Role,int>
    {

        private readonly IHttpContextAccessor _contextAccessor;
        public HotelCollectionContext(DbContextOptions<HotelCollectionContext> options, IHttpContextAccessor contextAccessor) :base(options)
        {
            _contextAccessor = contextAccessor;
        }

        // public HotelCollectionContext(DbContextOptions options) : base(options)
        // {
        // }

        public DbSet<EmailSentLog> EmailSentLogs { get; set; }
        public DbSet<HotelCategory> HotelCategories { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<LocalGovernmentArea> LocalGovernmentAreas { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<RequisitionDetails> RequisitionDetails { get; set; }
        public DbSet<ApprovalConfig> ApprovalConfigs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<HotelCategoryFee> HotelCategoryFees { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           // builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Role>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int saved = 0;
            string currentUser = "";
            if (_contextAccessor.HttpContext != null)
            {
                 currentUser = _contextAccessor.HttpContext.User.Identity.Name;
            }
            
            var currentDate = DateTime.Now;
            try
            {
                foreach (var entry in ChangeTracker.Entries<BaseEntity.Entity>()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.DateCreated = currentDate;
                        entry.Entity.CreatedBy = currentUser;
                        //entry.Entity.LastDateUpdated = currentDate;
                        //entry.Entity.UpdatedBy = currentUser;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastDateUpdated = currentDate;
                        entry.Entity.UpdatedBy = currentUser;
                        //if (entry.Entity.IsDeleted == true && entry.Entity. == null)
                        //{
                        //    entry.Entity.DateDeleted = currentDate;
                        //    entry.Entity.DeletedBy = currentUser;
                        //}
                    }
                }
                saved = await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception Ex)
            {
                //Log.Error(Ex, "An error has occured in SaveChanges");
                //Log.Error(Ex.InnerException, "An error has occured in SaveChanges InnerException");
                //Log.Error(Ex.StackTrace, "An error has occured in SaveChanges StackTrace");
            }

            return saved;
            //   return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            int saved = 0;
            string currentUser = "";
            if (_contextAccessor.HttpContext != null)
            {
                currentUser = _contextAccessor.HttpContext.User.Identity.Name;
            }
            var currentDate = DateTime.Now;
            try
            {
                foreach (var entry in ChangeTracker.Entries<BaseEntity.Entity>()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.DateCreated = currentDate;
                        entry.Entity.CreatedBy = currentUser;
                        //entry.Entity.LastDateUpdated = currentDate;
                        //entry.Entity.UpdatedBy = currentUser;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastDateUpdated = currentDate;
                        entry.Entity.UpdatedBy = currentUser;
                        //if (entry.Entity.IsDeleted == true && entry.Entity.DateDeleted == null)
                        //{
                        //    entry.Entity.DateDeleted = currentDate;
                        //    entry.Entity.DeletedBy = currentUser;
                        //}
                    }
                }
                saved = base.SaveChanges();
            }
            catch (Exception Ex)
            {
                //Log.Error(Ex, "An error has occured in SaveChanges");
                //Log.Error(Ex.InnerException, "An error has occured in SaveChanges InnerException");
                //Log.Error(Ex.StackTrace, "An error has occured in SaveChanges StackTrace");
                throw Ex;
            }

            return saved;
        }

    }


    //public class ContextDesignFactory : IDesignTimeDbContextFactory<HotelCollectionContext>
    //{
    //    public HotelCollectionContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<HotelCollectionContext>()
    //            .UseSqlServer("Server=.;Initial Catalog=HotelCollection;Persist Security Info=False;User ID=sa;Password=pa$$word123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    //        return new HotelCollectionContext(optionsBuilder.Options);
    //    }
    //}
}
  