using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Models;
using Sheaft.Domain.Views;
using Sheaft.Infrastructure.Persistence.Configurations;

namespace Sheaft.Infrastructure.Persistence
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

            modelBuilder.Entity<Consumer>().HasBaseType<User>();
            modelBuilder.Entity<Business>().HasBaseType<User>();

            modelBuilder.Entity<Producer>().HasBaseType<Business>();
            modelBuilder.Entity<Store>().HasBaseType<Business>();

            modelBuilder.Entity<Card>().HasBaseType<PaymentMethod>();
            modelBuilder.Entity<BankAccount>().HasBaseType<PaymentMethod>();

            modelBuilder.Entity<CardPayinTransaction>().HasBaseType<PayinTransaction>();
            modelBuilder.Entity<WebPayinTransaction>().HasBaseType<PayinTransaction>();

            modelBuilder.Entity<RefundPayinTransaction>().HasBaseType<RefundTransaction>();
            modelBuilder.Entity<RefundTransferTransaction>().HasBaseType<RefundTransaction>();

            modelBuilder.Entity<BusinessLegal>().HasBaseType<Legal>();
            modelBuilder.Entity<ConsumerLegal>().HasBaseType<Legal>();

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessConfiguration());
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
            modelBuilder.ApplyConfiguration(new ReturnableConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDeliveryConfiguration());
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
            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new WalletConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PayinTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new WebPayinTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new CardPayinTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new TransferTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new PayoutTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new RefundTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new RefundPayinTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new RefundTransferTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new NationalityConfiguration());
            modelBuilder.ApplyConfiguration(new LegalConfiguration());
            modelBuilder.ApplyConfiguration(new ConsumerLegalConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessLegalConfiguration());
            modelBuilder.ApplyConfiguration(new UboDeclarationConfiguration());
            modelBuilder.ApplyConfiguration(new UboConfiguration());
        }
    }
}