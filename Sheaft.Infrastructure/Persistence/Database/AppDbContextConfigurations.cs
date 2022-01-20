using Microsoft.EntityFrameworkCore;
using Sheaft.Infrastructure.Persistence.Database.Configurations;

namespace Sheaft.Infrastructure.Persistence
{
    internal partial class AppDbContext
    {
        protected void OnModelCreating(ModelBuilder modelBuilder, bool isAdminContext)
        {
            modelBuilder.HasDefaultSchema("dbo");

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BatchNumberConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new CartConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new CatalogConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new CatalogProductPriceConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new CompanyBillingConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyLegalsConfiguration());
            modelBuilder.ApplyConfiguration(new CompanySettingConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new DeliveryConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new DistributionConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new JobConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new ObservationConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new PickingConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new ProductConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new QuickOrderConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new RatingConfiguration());
            modelBuilder.ApplyConfiguration(new RecallConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new RecallClientConfiguration());
            modelBuilder.ApplyConfiguration(new ReturnableConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration(isAdminContext));
            modelBuilder.ApplyConfiguration(new UserConfiguration(isAdminContext));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder, _isAdminContext);
        }
    }
}