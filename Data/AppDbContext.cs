using Microsoft.EntityFrameworkCore;
using SolarService.Interface;
using SolarService.Models;
using SolarService.Models.Master;

namespace SolarService.Data
{
    public class AppDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public int CurrentTenantId => _tenantProvider.TenantId;

        public AppDbContext(DbContextOptions<AppDbContext> options, ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        // DbSets
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Lead> Leads => Set<Lead>();
        public DbSet<Quotation> Quotations => Set<Quotation>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
        public DbSet<Installation> Installations => Set<Installation>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Tenant> Tenant => Set<Tenant>();
        public DbSet<Customer> Customers => Set<Customer>();
        
        //Master table
        public DbSet<PanelMaster> PanelMasters { get; set; }
        public DbSet<InverterMaster> InverterMasters { get; set; }
        public DbSet<StructureMaster> StructureMasters { get; set; }
        public DbSet<RateMaster> RateMasters { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //------------------------------------------
            // 1️⃣ GLOBAL TENANT FILTER
            //------------------------------------------
            builder.Entity<User>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<Lead>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<Project>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<Quotation>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<Payment>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<PurchaseItem>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<Installation>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);
            builder.Entity<Invoice>().HasQueryFilter(x => x.TenantId == _tenantProvider.TenantId);

            //------------------------------------------
            // 2️⃣ TENANT FOREIGN KEY (NO CASCADE)
            //------------------------------------------
            builder.Entity<User>()
                .HasOne(u => u.Tenant)
                .WithMany()
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Lead>()
                .HasOne(l => l.Tenant)
                .WithMany()
                .HasForeignKey(l => l.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Lead>()
                .HasIndex(l => l.LeadId)
                .IsUnique();

            builder.Entity<Project>()
                .HasOne(p => p.Tenant)
                .WithMany()
                .HasForeignKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Quotation>()
                .HasOne(q => q.Tenant)
                .WithMany()
                .HasForeignKey(q => q.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Payment>()
                .HasOne(pay => pay.Tenant)
                .WithMany()
                .HasForeignKey(pay => pay.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PurchaseItem>()
                .HasOne(pi => pi.Tenant)
                .WithMany()
                .HasForeignKey(pi => pi.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Installation>()
                .HasOne(i => i.Tenant)
                .WithMany()
                .HasForeignKey(i => i.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Invoice>()
                .HasOne(inv => inv.Tenant)
                .WithMany()
                .HasForeignKey(inv => inv.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Customer>()
                .HasOne(c => c.Tenant)
                .WithMany()
                .HasForeignKey(c => c.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Customer>()
                .HasIndex(c => c.CustomerId)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            //------------------------------------------
            // 4️⃣ SEED ROLES
            //------------------------------------------
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin" },
                new Role { Id = 2, Name = "Manager" },
                new Role { Id = 3, Name = "TeamLead" },
                new Role { Id = 4, Name = "FieldBoy" }
            );

            //------------------------------------------
            // 5️⃣ SEED DEFAULT TENANT
            //------------------------------------------
            builder.Entity<Tenant>().HasData(
                new Tenant
                {
                    Id = 1,
                    Name = "Default Tenant",
                    Slug = "default",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );

            //------------------------------------------
            // 6️⃣ SEED ADMIN USER with TenantId = 1
            //------------------------------------------
            var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");

            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Super Admin",
                    Email = "admin@example.com",
                    Mobile = "9999999999",
                    Password = adminHash,
                    RoleId = 1,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    TenantId = 1
                }
            );
        }


        //------------------------------------------
        // SAVE CHANGES → AUTO TENANT-ID
        //------------------------------------------

        public override int SaveChanges()
        {
            ApplyTenantId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantId();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTenantId()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "TenantId"))
                {
                    var tenantProp = entry.Property("TenantId");

                    if (tenantProp.CurrentValue == null || (int)tenantProp.CurrentValue == 0)
                        tenantProp.CurrentValue = CurrentTenantId;
                }
            }
        }
    }
}
