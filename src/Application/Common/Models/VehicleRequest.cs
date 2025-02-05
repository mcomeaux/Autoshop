namespace Autoshop.Application.Common.Models
{
    public class VehicleRequest
    {
        public int? VehicleId { get; set; }
        public int CustomerId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
    }
}