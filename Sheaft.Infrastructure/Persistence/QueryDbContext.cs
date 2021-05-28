using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.Views;

namespace Sheaft.Infrastructure.Persistence
{
    public partial class QueryDbContext : DbContext
    {
        public QueryDbContext(
            DbContextOptions options)
            : base(options)
        {
        }
        
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DeliveryMode> DeliveryModes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Legal> Legals { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Returnable> Returnables { get; set; }
        public DbSet<Payin> Payins { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        public DbSet<PreAuthorization> PreAuthorizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<QuickOrder> QuickOrders { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Withholding> Withholdings { get; set; }

        public DbSet<ConsumerProduct> ConsumerProducts { get; set; }
        public DbSet<DepartmentProducers> DepartmentProducers { get; set; }
        public DbSet<DepartmentStores> DepartmentStores { get; set; }
        public DbSet<CountryPoints> CountryPoints { get; set; }
        public DbSet<RegionPoints> RegionPoints { get; set; }
        public DbSet<DepartmentPoints> DepartmentPoints { get; set; }
        public DbSet<DepartmentUserPoints> DepartmentUserPoints { get; set; }
        public DbSet<RegionUserPoints> RegionUserPoints { get; set; }
        public DbSet<CountryUserPoints> CountryUserPoints { get; set; }
    }
}