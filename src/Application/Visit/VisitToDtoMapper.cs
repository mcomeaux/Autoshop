using Autoshop.Application.Common.Models;

namespace Autoshop.Application.Visit
{
    public static class VisitToDtoMapper
    {
        public static VisitDto ToVisitDto(this Entities.Visit visit)
        {
            return new VisitDto{
                VisitId = visit.VisitId,
                VehicleId = visit.VehicleId,
                DateOfArrival = visit.DateOfArrival,
                DateOfDeparture = visit.DateOfDeparture,
                Description = visit.Description
            };
        }
    }
}