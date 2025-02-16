using Autoshop.Application.Vehicle;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;

namespace Autoshop.Application.UnitTests.Vehicle
{
    public class CreateVehicleTests : UnitTestBase
    {
        private CreateVehicle.Command _command = null!;
        private CreateVehicle.CommandHandler _handler = null!;
        private Mock<ILogger<CreateVehicle.CommandHandler>> _logger = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CreateVehicle.CommandHandler>>();

            _command = new CreateVehicle.Command();
            _handler = new CreateVehicle.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        //TODO: Create test for Not Found when customer is not found

        [Test]
        public async Task CreateVehicleCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedVehicleId = 2;
            var expectedCustomerId = 3;
            var expectedMake = "Honda";
            var expectedModel = "CRX";
            var expectedYear = 1989;
            var expectedColor = "White";
            var expectedVIN = "Test VIN";
            var expectedDescription = "Test Description";
            
            var vehicleRequest = new VehicleDto{
                CustomerId = expectedCustomerId,
                Make = expectedMake,
                Model = expectedModel,
                Year = expectedYear,
                Color = expectedColor,
                VIN = expectedVIN,
                Description = expectedDescription
            };
            _command.CreateRequest = vehicleRequest;


            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertVehicleCommand.Command>(
                    c => c.CustomerId == expectedCustomerId
                    && c.Make == expectedMake
                    && c.Model == expectedModel
                    && c.Year == expectedYear
                    && c.Color == expectedColor
                    && c.VIN == expectedVIN
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult(
                    new Entities.Vehicle { 
                        VehicleId = expectedVehicleId,
                        CustomerId = expectedCustomerId,
                        Make = expectedMake,
                        Model = expectedModel,
                        Year = expectedYear,
                        Color = expectedColor,
                        VIN = expectedVIN,
                        Description = expectedDescription
                    }));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.VehicleId.ShouldBe(expectedVehicleId);
            response.CustomerId.ShouldBe(expectedCustomerId);
            response.Make.ShouldBe(expectedMake);
            response.Model.ShouldBe(expectedModel);
            response.Year.ShouldBe(expectedYear);
            response.Color.ShouldBe(expectedColor);
            response.VIN.ShouldBe(expectedVIN);
            response.Description.ShouldBe(expectedDescription);
        }
    }
}
