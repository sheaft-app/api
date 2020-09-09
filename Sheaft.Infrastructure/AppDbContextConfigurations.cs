using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Infrastructure
{
    public partial class AppDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DepartmentProducers>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("ProducersPerDepartment");
            });

            modelBuilder.Entity<DepartmentStores>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("StoresPerDepartment");
            });

            modelBuilder.Entity<CountryPoints>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("PointsPerCountry");
            });

            modelBuilder.Entity<RegionPoints>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("PointsPerRegion");
            });

            modelBuilder.Entity<DepartmentPoints>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("PointsPerDepartment");
            });

            modelBuilder.Entity<CountryUserPoints>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("UserPointsPerCountry");
            });

            modelBuilder.Entity<RegionUserPoints>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("UserPointsPerRegion");
            });

            modelBuilder.Entity<DepartmentUserPoints>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("UserPointsPerDepartment");
            });

            modelBuilder.Entity<Producer>().HasBaseType<User>();
            modelBuilder.Entity<Store>().HasBaseType<User>();
            modelBuilder.Entity<Consumer>().HasBaseType<User>();

            modelBuilder.Entity<CardPayment>().HasBaseType<PaymentMethod>();
            modelBuilder.Entity<TransferPayment>().HasBaseType<PaymentMethod>();

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerTagConfiguration());
            modelBuilder.ApplyConfiguration(new StoreConfiguration());
            modelBuilder.ApplyConfiguration(new StoreTagConfiguration());
            modelBuilder.ApplyConfiguration(new ConsumerConfiguration());
            modelBuilder.ApplyConfiguration(new AgreementConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryModeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration());
            modelBuilder.ApplyConfiguration(new LevelConfiguration());
            modelBuilder.ApplyConfiguration(new RewardConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new PackagingConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderVendorConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderSenderConfiguration());
            modelBuilder.ApplyConfiguration(new QuickOrderConfiguration());
            modelBuilder.ApplyConfiguration(new QuickOrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductTagConfiguration());
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new SponsoringConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new TransferPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new CardPaymentConfiguration());
        }
    }
}