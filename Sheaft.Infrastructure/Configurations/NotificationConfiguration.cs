using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Sheaft.Infrastructure
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> entity)
        {
            entity.Property<long>("Uid");
            entity.Property<long?>("UserUid");
            entity.Property<long?>("GroupUid");

            entity.Property(c => c.CreatedOn);
            entity.Property(c => c.UpdatedOn).IsConcurrencyToken();

            entity.HasOne(c => c.User).WithMany().HasForeignKey("UserUid").OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(c => c.Group).WithMany().HasForeignKey("GroupUid").OnDelete(DeleteBehavior.NoAction);

            entity.HasKey("Uid");

            entity.HasIndex(c => c.Id).IsUnique();
            entity.HasIndex("UserUid");
            entity.HasIndex("GroupUid");
            entity.HasIndex("Uid", "Id", "UserUid", "GroupUid", "CreatedOn");

            entity.ToTable("Notifications");
        }
    }
}
