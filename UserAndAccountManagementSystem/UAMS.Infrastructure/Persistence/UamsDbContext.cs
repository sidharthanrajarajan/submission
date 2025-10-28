using Microsoft.EntityFrameworkCore;
using UAMS.Domain.Entities;

namespace UAMS.Infrastructure.Persistence
{
    public class UamsDbContext : DbContext
    {
        public UamsDbContext(DbContextOptions<UamsDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Bank> Banks => Set<Bank>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global soft-delete filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(UamsDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);
                    method.Invoke(null, new object[] { modelBuilder });
                }
            }

            //user
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "training");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.UserType).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
            });

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "training");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(r => r.Name).IsUnique();
            });

            // Permission
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permissions", "training");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(p => p.Name).IsUnique();
            });

            // userRole (many-to-many: user and role)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles", "training");
                entity.HasKey(ur => ur.Id);

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // RolePermission (many-to-many: role and permission)
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermissions", "training");
                entity.HasKey(rp => rp.Id);

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Bank
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Banks", "training");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(150);
                entity.Property(b => b.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(b => b.Code).IsUnique();
            });

            // Branch
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.ToTable("Branches", "training");
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name).IsRequired().HasMaxLength(150);
                entity.Property(b => b.BranchCode).IsRequired().HasMaxLength(20);
                entity.Property(b => b.IFSCCode).IsRequired().HasMaxLength(20);

                entity.HasOne(b => b.Bank)
                      .WithMany(bk => bk.Branches)
                      .HasForeignKey(b => b.BankId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts", "training");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AccountNumber).IsRequired().HasMaxLength(30);
                entity.HasIndex(a => a.AccountNumber).IsUnique();
                entity.Property(a => a.AccountType).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Currency).IsRequired().HasMaxLength(10);
                entity.Property(a => a.Balance).HasPrecision(18, 2); //added after warning form initial run

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Accounts)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Branch)
                      .WithMany(b => b.Accounts)
                      .HasForeignKey(a => a.BranchId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.PowerOfAttorneyUser)
                      .WithMany(u => u.PowerOfAttorneyAccounts)
                      .HasForeignKey(a => a.PowerOfAttorneyUserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions", "training");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Amount).HasPrecision(18, 2);
                entity.Property(t => t.Currency).IsRequired().HasMaxLength(10);
                entity.Property(t => t.TransactionType).IsRequired().HasMaxLength(50);
                entity.Property(t => t.Status).IsRequired().HasMaxLength(50);

                entity.HasOne(t => t.FromAccount)
                      .WithMany()
                      .HasForeignKey(t => t.FromAccountId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ToAccount)
                      .WithMany()
                      .HasForeignKey(t => t.ToAccountId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        //method to filter out isDeleted==true entities
        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder)
            where TEntity : BaseEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
}

