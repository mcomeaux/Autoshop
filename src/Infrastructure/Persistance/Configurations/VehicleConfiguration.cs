using Autoshop.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autoshop.Infrastructure.Persistance.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");
            builder.HasKey(t => t.VehicleId);

            
            builder.HasOne(t => t.Customer)
                .WithMany(r => r.Vehicles)
                .HasForeignKey(t => t.CustomerId);
        }
    }

}
