using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Autoshop.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Entities.Customer> Customers { get; set; }
        
        public DbSet<Entities.Vehicle> Vehicles { get; set; }
        public DbSet<Entities.Visit> Visits { get; set; }
        public DbSet<Entities.Invoice> Invoices { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DatabaseFacade Database { get; }
        EntityEntry Entry(object entity);

    }
}
