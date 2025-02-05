using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetVehicleByVehicleIdQueryTests : UnitTestBase
    {
        private GetVehicleByVehicleIdQuery.Command _command;
        private GetVehicleByVehicleIdQuery.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new GetVehicleByVehicleIdQuery.Command
            {
                VehicleId = 0
            };

            _handler = new GetVehicleByVehicleIdQuery.CommandHandler(ApplicationDbContext);
        }

        [Test]
        public async Task GetVehicleByVehicleIdQuery_ShouldReturnNull_WhenTheVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = 11;
            _command.VehicleId = vehicleId;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public async Task GetVehicleByVehicleIdQuery_Should_ReturnAVehicle()
        {
            // Arrange
            var vehicleId = 1;
            var expectedCustomerId = 1;
            var expectedMake = "Honda";
            var expectedModel = "CRX";
            var expectedYear = 1989;
            var expectedColor = "White";
            var expectedVIN = "Test VIN";
            var expectedDescription = "Test Description";
            _command.VehicleId = vehicleId;

            ApplicationDbContext.Vehicles.Add(
                new Entities.Vehicle { 
                    VehicleId = vehicleId,
                    CustomerId = expectedCustomerId,
                    Make = expectedMake,
                    Model = expectedModel,
                    Year = expectedYear,
                    Color = expectedColor,
                    VIN = expectedVIN,
                    Description = expectedDescription
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.VehicleId.ShouldBe(vehicleId);
            result.CustomerId.ShouldBe(expectedCustomerId);
            result.Make.ShouldBe(expectedMake);
            result.Model.ShouldBe(expectedModel);
            result.Year.ShouldBe(expectedYear);
            result.Color.ShouldBe(expectedColor);
            result.VIN.ShouldBe(expectedVIN);
            result.Description.ShouldBe(expectedDescription);
        }
    }
}
