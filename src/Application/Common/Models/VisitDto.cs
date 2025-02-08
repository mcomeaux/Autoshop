namespace Autoshop.Application.Common.Models
{
    public class VisitDto
    {
        public int? VisitId { get; set; }
        public int VehicleId { get; set; }
        public DateTime DateOfArrival { get; set; }
        public DateTime? DateOfDeparture { get; set; }
        public string Description { get; set; }
    }
}