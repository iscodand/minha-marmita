using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<ApplicationLog> ApplicationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(x => x.Meal)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.MealId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "USERS");
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "ROLES");
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.ToTable(name: "USER_ROLES");

                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable(name: "USER_CLAIMS");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable(name: "USER_LOGINS");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable(name: "ROLE_CLAIMS");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable(name: "USER_TOKENS");
            });

            builder.Entity<PaymentType>().HasData(
                new PaymentType
                {
                    Id = 1,
                    Description = "PIX"
                },
                new PaymentType
                {
                    Id = 2,
                    Description = "Dinheiro"
                }
            );

            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "ADMIN",
                    Description = "Administrador"
                },
                new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "MANAGER",
                    Description = "Gerente"
                },
                new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "EMPLOYEE",
                    Description = "Funcionário"
                }
            );
        }
    }
}