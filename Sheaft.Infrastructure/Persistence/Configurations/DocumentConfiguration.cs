using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;

namespace Sheaft.Infrastructure.Persistence.Configurations;

internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder
            .Property(p => p.OwnerId)
            .HasMaxLength(Constants.IDS_LENGTH)
            .HasConversion(vat => vat.Value, value => new OwnerId(value));
        
        builder
            .Property("_params");

        builder
            .Property(p => p.Name)
            .HasMaxLength(DocumentName.MAXLENGTH)
            .HasConversion(vat => vat.Value, value => new DocumentName(value));
            
        builder
            .Property(c => c.Id)
            .HasMaxLength(Constants.IDS_LENGTH);
        
        builder.ToTable("Document");
    }
}