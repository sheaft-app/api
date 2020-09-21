﻿using Sheaft.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sheaft.Infrastructure.Persistence.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> entity)
        {
            entity.Property(c => c.Owner).IsRequired();
            entity.Property(c => c.IBAN).IsRequired();
            entity.Property(c => c.BIC).IsRequired();
            entity.Property(c => c.BIC).IsRequired();
        }
    }
}