using Autoshop.Application.Common.Models;

namespace Autoshop.Application.Vehicle
{
    public static class VehicleToDtoMapper
    {
        public static VehicleDto ToVehicleDto(this Entities.Vehicle vehicle)
        {
            return new VehicleDto{
                VehicleId = vehicle.VehicleId,
                CustomerId = vehicle.CustomerId,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color,
                VIN = vehicle.VIN,
                Description = vehicle.Description
            };
        }
    }
}