
namespace Autoshop.Application.Entities
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }        
        public string Color { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
        public string Description { get; set; }

        public List<Visit> Visits { get; set; }

        public Customer Customer { get; set; }
    }
}
