using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OracleEfDemo.Models;

namespace OracleEfDemo.DbContext
{
    public class AppDbContext : IdentityDbContext<UserApp, RoleApp, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("User Id=EFUSER;Password=EFPASS;Data Source=localhost:1521/test.local;",
                o => o.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("EFUSER");

            builder.Entity<Categories>().ToTable("CATEGORIES");
            builder.Entity<Products>().ToTable("PRODUCTS");
            builder.Entity<Orders>().ToTable("ORDERS");
            builder.Entity<OrderItems>().ToTable("ORDER_ITEMS");
            builder.Entity<Customers>().ToTable("CUSTOMERS");
            builder.Entity<StockLog>().ToTable("STOCK_LOG");
            builder.Entity<UserApp>().ToTable("EMPLOYEES");

            #region Categories data conf
            builder.Entity<Categories>(x =>
            {
                x.Property(x => x.CategoryName).HasMaxLength(25);
                x.HasIndex(x => x.CategoryName).IsUnique();
            });
            #endregion

            #region Products data conf
            builder.Entity<Products>(x =>
            {
                x.Property(x => x.ProductName).HasMaxLength(25);
                x.HasIndex(x => x.ProductName).IsUnique();
            });
            builder.Entity<Products>().Property(x => x.Price).HasPrecision(18, 2);
            builder.Entity<Products>().Property(x => x.StockQuantity).HasPrecision(18, 2);

            builder.Entity<Products>().HasOne(p => p.Categories).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Orders data conf
            builder.Entity<Orders>().Property(x => x.Total).HasPrecision(18, 2);
            builder.Entity<Orders>(x =>
            {
                x.Property(x => x.OrderNumber)
                      .HasMaxLength(36);

                x.HasIndex(x => x.OrderNumber)
                      .IsUnique();
            });

            builder.Entity<Orders>().HasOne(p => p.Customers).WithMany(c => c.Orders).HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region OrderItems data conf
            builder.Entity<OrderItems>().Property(x => x.ProductName).HasMaxLength(25);
            builder.Entity<OrderItems>().Property(x => x.Quantity).HasPrecision(18, 2);
            builder.Entity<OrderItems>().Property(x => x.UnitPrice).HasPrecision(18, 2);

            builder.Entity<OrderItems>().HasOne(oi => oi.Orders).WithMany(o => o.OrderItems).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OrderItems>().HasOne(oi => oi.Products).WithMany(p => p.OrderItems).HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Customers data conf
            builder.Entity<Customers>().Property(x => x.FullName).HasMaxLength(50);
            builder.Entity<Customers>().Property(x => x.Phone).HasMaxLength(25);
            builder.Entity<Customers>().Property(x => x.Address).HasMaxLength(255);
            builder.Entity<Customers>(x =>
            {
                x.Property(x => x.Email)
                      .HasMaxLength(50);

                x.HasIndex(x => x.Email)
                      .IsUnique();
            });
            #endregion

            #region StockLog data conf
            builder.Entity<StockLog>().Property(x => x.ProductName).HasMaxLength(25);
            builder.Entity<StockLog>().Property(x => x.QuantityChange).HasPrecision(18, 2);
            builder.Entity<StockLog>().Property(x => x.StockAfter).HasPrecision(18, 2);
            builder.Entity<StockLog>().Property(x => x.UserName).HasMaxLength(100);
            #endregion

            #region Userapp data conf
            builder.Entity<UserApp>().Property(x => x.FullName).HasMaxLength(50);
            builder.Entity<UserApp>().Property(x => x.Department).HasMaxLength(35);
            builder.Entity<UserApp>().Property(x => x.Salary).HasPrecision(18, 2);
            #endregion

            #region default user seed data
            var roleAdmin = "6352d99e-3c51-4188-8566-309d51888b21";

            var adminId = "5cd9ecaa-4346-4b27-a9f8-a9a54e63a8d6";

            var adminRole = new List<RoleApp>() {
                new() { Id = roleAdmin, Name = "Admin", NormalizedName = "ADMIN"}
            };

            var hasher = new PasswordHasher<IdentityUser>();
            var adminUser = new UserApp
            {
                Id = adminId,
                FullName = "Semih KAYGISIZ",
                UserName = "Administrator",
                NormalizedUserName = "ADMINISTRATOR",
                Email = "kaygsz.semih@gmail.com",
                NormalizedEmail = "KAYGSZ.SEMIH@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123."),
                SecurityStamp = string.Empty,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                CreatedDate = new DateTime(2025, 12, 24),
                UpdatedDate = DateTime.Today,
            };

            builder.Entity<RoleApp>().HasData(adminRole);
            builder.Entity<UserApp>().HasData(adminUser);
            builder.Entity<IdentityUserRole<string>>()
            .HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminId,
                    RoleId = roleAdmin
                }
            );
            #endregion
        }

        #region DbSets
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<StockLog> StockLog { get; set; }
        #endregion

        #region SaveChanges override
        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity baseEntity)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                baseEntity.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(baseEntity).Property(x => x.CreatedDate).IsModified = false;

                                baseEntity.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }

            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;

                                entityReference.UpdatedDate = DateTime.Now;
                                break;
                            }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}
