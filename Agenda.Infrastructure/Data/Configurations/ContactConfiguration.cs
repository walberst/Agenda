using Agenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agenda.Infrastructure.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(c => c.Email)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(c => c.Phone)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(c => c.Address)
               .HasMaxLength(500);
    }
}
