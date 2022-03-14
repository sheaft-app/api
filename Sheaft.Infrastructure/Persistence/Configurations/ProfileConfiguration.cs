using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.ProfileManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder
            .Property<long>("Id")
            .ValueGeneratedOnAdd();
        
        builder.HasKey("Id");
        
        builder
            .Property<DateTimeOffset>("CreatedOn")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAdd();
        
        builder
            .Property<DateTimeOffset>("UpdatedOn")
            .HasDefaultValueSql("GETUTCDATE()")
            .HasValueGenerator(typeof(DateTimeOffsetValueGenerator))
            .ValueGeneratedOnAddOrUpdate();
        
        builder.Property<long>("AccountId");

        builder
            .Property(p => p.Identifier)
            .HasConversion(name => name.Value, value => new ProfileId(value));

        builder
            .HasIndex(c => c.Identifier)
            .IsUnique();
        
        builder.ToTable("Profiles");
    }
}