using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetVehiclesByCustomerIdQueryTests : UnitTestBase
    {
        private GetVehiclesByCustomerIdQuery.Command _command;
        private GetVehiclesByCustomerIdQuery.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new GetVehiclesByCustomerIdQuery.Command
            {
                CustomerId = 0
            };

            _handler = new GetVehiclesByCustomerIdQuery.CommandHandler(ApplicationDbContext);
        }


        [Test]
        public async Task GetVehiclesByCustomerIdQuery_ShouldReturnEmptyList_WhenTheCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 11;
            _command.CustomerId = customerId;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(0);
        }

        [Test]
        public async Task GetVehicleByCustomerIdQuery_Should_ReturnAListOfVehicles()
        {
            // Arrange
            var customerId = 1;
            _command.CustomerId = customerId;

            var expectedVehicle1 = new Entities.Vehicle {
                VehicleId = 2,
                CustomerId = customerId,
                Make = "Honda",
                Model = "Accord",
                Year = 2005,
                Color = "Tan",
                VIN = "Test VIN1",
                Description = "Vehicle1"
            };

            var expectedVehicle2 = new Entities.Vehicle {
                VehicleId = 3,
                CustomerId = customerId,
                Make = "GMC",
                Model = "Sierra 1500",
                Year = 2024,
                Color = "Black",
                VIN = "Test VIN2",
                Description = "Vehicle2"
            };

            ApplicationDbContext.Vehicles.Add(expectedVehicle1);
            ApplicationDbContext.Vehicles.Add(expectedVehicle2);
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.Contains(expectedVehicle1).ShouldBe(true);
            result.Contains(expectedVehicle2).ShouldBe(true);
        }
    }
}
