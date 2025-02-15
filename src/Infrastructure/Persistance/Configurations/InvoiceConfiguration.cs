using Autoshop.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autoshop.Infrastructure.Persistance.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(t => t.InvoiceId);

            
            builder.HasOne(t => t.Visit)
                .WithOne(r => r.Invoice)
                .HasForeignKey<Invoice>(t => t.VisitId);

            builder.HasOne(t => t.Customer)
                .WithMany(r => r.Invoices)
                .HasForeignKey(t => t.CustomerId);
        }
    }

}
