using Autoshop.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autoshop.Infrastructure.Persistance.Configurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("Visits");
            builder.HasKey(t => t.VisitId);

            
            builder.HasOne(t => t.Vehicle)
                .WithMany(r => r.Visits)
                .HasForeignKey(t => t.VehicleId);
        }
    }

}
