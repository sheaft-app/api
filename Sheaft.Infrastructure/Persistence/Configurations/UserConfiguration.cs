﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property<long>("Uid");

            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.Email).IsRequired();
            entity.Property(c => c.FirstName).IsRequired();
            entity.Property(c => c.LastName).IsRequired();
            entity.Property(c => c.TotalPoints).HasDefaultValue(0);

            entity.HasDiscriminator(c => c.Kind)
                .HasValue<Producer>(ProfileKind.Producer)
                .HasValue<Store>(ProfileKind.Store)
                .HasValue<Consumer>(ProfileKind.Consumer)
                .HasValue<Admin>(ProfileKind.Admin)
                .HasValue<Support>(ProfileKind.Support);

            entity.OwnsOne(c => c.Address, cb =>
            {
                cb.Property<long>("DepartmentUid");
                cb.HasOne(c => c.Department).WithMany().HasForeignKey("DepartmentUid");

                cb.ToTable("UserAddresses");
            });

            entity.HasOne(c => c.ProfileInformation);

            entity.OwnsMany(c => c.Points, p =>
            {
                p.Property<long>("Uid");
                p.HasKey("Uid");
                p.HasIndex(c => c.Id).IsUnique();
                p.ToTable("UserPoints");
            });

            entity.HasMany<Sponsoring>().WithOne(c => c.Sponsor).HasForeignKey("SponsorUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasMany<Order>().WithOne(c => c.User).HasForeignKey("UserUid").OnDelete(DeleteBehavior.Cascade);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex(c => c.Email).IsUnique();
            entity.HasIndex(c => c.Identifier);
            entity.HasIndex("Uid", "Id", "RemovedOn");

            entity.ToTable("Users");
        }
    }
}
