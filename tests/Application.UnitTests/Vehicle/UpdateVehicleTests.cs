using Autoshop.Application.Vehicle;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Exceptions;

namespace Autoshop.Application.UnitTests.Vehicle
{
    public class UpdateVehicleTests : UnitTestBase
    {
        private UpdateVehicle.Command _command;
        private UpdateVehicle.CommandHandler _handler;
        private Mock<ILogger<UpdateVehicle.CommandHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<UpdateVehicle.CommandHandler>>();

            _command = new UpdateVehicle.Command();
            _handler = new UpdateVehicle.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        //TODO: Add test for UpdateCustomer when CustomerId doesnt exist

        [Test]
        public async Task UpdateVehicleCommandHandler_ShouldThrowNotFound_WhenVehicleNotFound()
        {
            // Arrange
            var expectedVehicleId = 2;
            var expectedCustomerId = 3;
            var expectedMake = "Honda";
            var expectedModel = "CRX";
            var expectedYear = 1989;
            var expectedColor = "White";
            var expectedDescription = "Test Description";
            
            var vehicleRequest = new VehicleRequest{
                VehicleId = expectedVehicleId,
                CustomerId = expectedCustomerId,
                Make = expectedMake,
                Model = expectedModel,
                Year = expectedYear,
                Color = expectedColor,
                Description = expectedDescription
            };
            _command.UpdateRequest = vehicleRequest;
            string expectedErrorMessage = $"Vehicle with Id [{expectedVehicleId}] was not found.";

            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertVehicleCommand.Command>(
                    c => c.VehicleId == expectedVehicleId
                    && c.CustomerId == expectedCustomerId
                    && c.Make == expectedMake
                    && c.Model == expectedModel
                    && c.Year == expectedYear
                    && c.Color == expectedColor
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult<Entities.Vehicle>(null));

            // Act
            try
            {
                var response = await _handler.Handle(_command, CancellationToken.None);
                Assert.IsTrue(false);
            }
            catch(NotFoundException ex)
            {
                // Assert
                ex.Message.ShouldBe(expectedErrorMessage);

            }
            VerifyAll();
        }

        [Test]
        public async Task UpdateVehicleCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedVehicleId = 2;
            var expectedCustomerId = 3;
            var expectedMake = "Honda";
            var expectedModel = "CRX";
            var expectedYear = 1989;
            var expectedColor = "White";
            var expectedDescription = "Test Description";

            
            var vehicleRequest = new VehicleRequest{
                VehicleId = expectedVehicleId,
                CustomerId = expectedCustomerId,
                Make = expectedMake,
                Model = expectedModel,
                Year = expectedYear,
                Color = expectedColor,
                Description = expectedDescription
            };
            _command.UpdateRequest = vehicleRequest;


            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertVehicleCommand.Command>(
                    c => c.VehicleId == expectedVehicleId
                    && c.CustomerId == expectedCustomerId
                    && c.Make == expectedMake
                    && c.Model == expectedModel
                    && c.Year == expectedYear
                    && c.Color == expectedColor
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult(
                    new Entities.Vehicle { 
                        VehicleId = expectedVehicleId,
                        CustomerId = expectedCustomerId,
                        Make = expectedMake,
                        Model = expectedModel,
                        Year = expectedYear,
                        Color = expectedColor,
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
            response.Description.ShouldBe(expectedDescription);
        }
    }
}
