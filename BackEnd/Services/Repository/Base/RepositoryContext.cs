using Microsoft.EntityFrameworkCore;
using Services.Models;
using Services.Models.Entities;
using System.Net;

namespace Services.Repository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Chip> Chips { get; set; }
        public DbSet<Display> Displays { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftByProduct> GiftByProducts { get; set; }
        public DbSet<ImportProduct> ImportProducts { get; set; }
        public DbSet<Memory> Memorys { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
