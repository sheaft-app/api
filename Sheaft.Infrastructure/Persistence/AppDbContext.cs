using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Persistence;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence
{
    public static class AppDbContextFactory
    {
        public static IAppDbContext GetUnitTestDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
                .Options;
            
            return new AppDbContext(false, options);
        }
    }
    internal partial class AppDbContext : DbContext, IAppDbContext
    {
        private readonly bool _isAdminContext;

        internal AppDbContext(
            bool isAdminContext,
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _isAdminContext = isAdminContext;
        }
        
        public AppDbContext(
            IConfiguration configuration,
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
            _isAdminContext = configuration.GetValue<bool?>("IsAdminContext") ?? false;
        }

        public DbSet<BatchNumber> BatchNumbers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PickingOrder> PickingOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<QuickOrder> QuickOrders { get; set; }
        public DbSet<Recall> Recalls { get; set; }
        public DbSet<Returnable> Returnables { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
    }
}