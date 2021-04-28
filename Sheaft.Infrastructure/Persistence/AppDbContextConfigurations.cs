using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.Views;
using Sheaft.Infrastructure.Persistence.Configurations;

namespace Sheaft.Infrastructure.Persistence
{
    public partial class AppDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");

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
            modelBuilder.Entity<Domain.Business>().HasBaseType<User>();

            modelBuilder.Entity<Producer>().HasBaseType<Domain.Business>();
            modelBuilder.Entity<Store>().HasBaseType<Domain.Business>();

            modelBuilder.Entity<Card>().HasBaseType<PaymentMethod>();
            modelBuilder.Entity<BankAccount>().HasBaseType<PaymentMethod>();

            modelBuilder.Entity<WebPayin>().HasBaseType<Payin>();
            modelBuilder.Entity<PreAuthorizedPayin>().HasBaseType<Payin>();

            modelBuilder.Entity<PayinRefund>().HasBaseType<Refund>();

            modelBuilder.Entity<BusinessLegal>().HasBaseType<Legal>();
            modelBuilder.Entity<ConsumerLegal>().HasBaseType<Legal>();

            modelBuilder.ApplyConfiguration(new UserConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new BusinessConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerConfiguration());
            modelBuilder.ApplyConfiguration(new ProducerTagConfiguration());
            modelBuilder.ApplyConfiguration(new StoreConfiguration());
            modelBuilder.ApplyConfiguration(new StoreTagConfiguration());
            modelBuilder.ApplyConfiguration(new ConsumerConfiguration());
            modelBuilder.ApplyConfiguration(new AgreementConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new DeliveryModeConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new LevelConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new RewardConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new ReturnableConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new OrderConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDeliveryConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new PurchaseOrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderVendorConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderSenderConfiguration());
            modelBuilder.ApplyConfiguration(new QuickOrderConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new QuickOrderProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new ProductTagConfiguration());
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new RegionConfiguration());
            modelBuilder.ApplyConfiguration(new SponsoringConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new BankAccountConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new WalletConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new PayinConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new WebPayinConfiguration());
            modelBuilder.ApplyConfiguration(new TransferConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new PayoutConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new RefundConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new PayinRefundConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new NationalityConfiguration());
            modelBuilder.ApplyConfiguration(new LegalConfiguration());
            modelBuilder.ApplyConfiguration(new ConsumerLegalConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessLegalConfiguration());
            modelBuilder.ApplyConfiguration(new DonationConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new WithholdingConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new DeliveryClosingConfiguration());
            modelBuilder.ApplyConfiguration(new BusinessClosingConfiguration());
            modelBuilder.ApplyConfiguration(new ProfilePictureConfiguration());
            modelBuilder.ApplyConfiguration(new ProductPictureConfiguration());
            modelBuilder.ApplyConfiguration(new UserSettingConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new CatalogConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new CatalogProductConfiguration());
            modelBuilder.ApplyConfiguration(new PreAuthorizationConfiguration(_isAdminContext));
            modelBuilder.ApplyConfiguration(new PreAuthorizedPayinConfiguration());
            modelBuilder.ApplyConfiguration(new DeclarationConfiguration());
            modelBuilder.ApplyConfiguration(new UboConfiguration());
            modelBuilder.ApplyConfiguration(new OpeningHoursConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryHoursConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new PageConfiguration());
            modelBuilder.ApplyConfiguration(new UserPointConfiguration());
        }
    }
}
