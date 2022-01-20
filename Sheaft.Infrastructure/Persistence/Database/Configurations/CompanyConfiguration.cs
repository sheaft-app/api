using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.Persistence.Database.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        private readonly bool _isAdmin;

        public CompanyConfiguration(bool isAdmin)
        {
            _isAdmin = isAdmin;
        }

        public void Configure(EntityTypeBuilder<Company> entity)
        {
            if (!_isAdmin)
                entity.HasQueryFilter(p => !p.Removed);

            entity.Ignore(c => c.DomainEvents);

            entity.Property(o => o.Name)
                .UseCollation("Latin1_general_CI_AI")
                .IsRequired();

            entity
                .Property(c => c.UpdatedOn)
                .IsRowVersion();

            entity.OwnsOne(c => c.ShippingAddress);

            entity
                .HasMany(c => c.Settings)
                .WithOne()
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            
            entity
                .HasOne(c => c.Details)
                .WithOne()
                .HasForeignKey<CompanyDetails>(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(c => c.Legals)
                .WithOne()
                .HasForeignKey<CompanyLegals>(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity
                .HasOne(c => c.Billing)
                .WithOne()
                .HasForeignKey<CompanyBilling>(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasKey(c => c.Id);
            entity.ToTable("Companies");
        }
    }

    public class CompanyBillingConfiguration : IEntityTypeConfiguration<CompanyBilling>
    {
        public void Configure(EntityTypeBuilder<CompanyBilling> entity)
        {
            entity.OwnsOne(c => c.Address);

            entity.HasKey(c => c.CompanyId);
            entity.ToTable("CompanyBillings");
        }
    }

    public class CompanyDetailsConfiguration : IEntityTypeConfiguration<CompanyDetails>
    {
        public void Configure(EntityTypeBuilder<CompanyDetails> entity)
        {
            entity.OwnsMany(c => c.OpeningHours, oh =>
            {
                oh.ToTable("CompanyDetailsOpeningHours");
            });

            entity.OwnsMany(c => c.Closings, c =>
            {
                c.ToTable("CompanyDetailsClosings");
            });

            entity.OwnsMany(c => c.Pictures, p =>
            {
                p.ToTable("CompanyDetailsPictures");
            });

            entity.OwnsMany(c => c.Tags, t =>
            {
                t.HasOne(ct => ct.Tag)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                t.ToTable("CompanyDetailsTags");
            });

            entity.HasKey(c => c.CompanyId);
            entity.ToTable("CompanyDetails");
        }
    }

    public class CompanyLegalsConfiguration : IEntityTypeConfiguration<CompanyLegals>
    {
        public void Configure(EntityTypeBuilder<CompanyLegals> entity)
        {
            entity
                .Property(e => e.Identifier)
                .IsRequired();

            entity.OwnsOne(c => c.Address);

            entity.HasKey(c => c.CompanyId);
            entity.HasIndex(c => c.Identifier).IsUnique();
            entity.ToTable("CompanyLegals");
        }
    }

    public class CompanySettingConfiguration : IEntityTypeConfiguration<CompanySetting>
    {
        public void Configure(EntityTypeBuilder<CompanySetting> entity)
        {
            entity
                .HasOne(c => c.Setting)
                .WithMany()
                .HasForeignKey(c => c.SettingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            entity.HasKey(c => new { c.CompanyId, c.SettingId });
            entity.ToTable("CompanySettings");
        }
    }
}
