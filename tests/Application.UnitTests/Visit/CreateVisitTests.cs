using Autoshop.Application.Visit;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;
using System;

namespace Autoshop.Application.UnitTests.Visit
{
    public class CreateVisitTests : UnitTestBase
    {
        private CreateVisit.Command _command = null!;
        private CreateVisit.CommandHandler _handler = null!;
        private Mock<ILogger<CreateVisit.CommandHandler>> _logger = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CreateVisit.CommandHandler>>();

            _command = new CreateVisit.Command();
            _handler = new CreateVisit.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        //TODO: Create test for Not Found when customer is not found

        [Test]
        public async Task CreateVisitCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedVisitId = 2;
            var expectedVehicleId = 3;
            var expectedDateOfArrival = DateTime.Now.AddDays(-5);
            var expectedDateOfDeparture = DateTime.Now.AddDays(-2);
            var expectedDescription = "Test Description";
            
            var visitRequest = new VisitDto{
                VehicleId = expectedVehicleId,
                DateOfArrival = expectedDateOfArrival,
                DateOfDeparture = expectedDateOfDeparture,
                Description = expectedDescription
            };
            _command.CreateRequest = visitRequest;


            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertVisitCommand.Command>(
                    c => c.VehicleId == expectedVehicleId
                    && c.DateOfArrival == expectedDateOfArrival
                    && c.DateOfDeparture == expectedDateOfDeparture
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult(
                    new Entities.Visit { 
                        VisitId = expectedVisitId,
                        VehicleId = expectedVehicleId,
                        DateOfArrival = expectedDateOfArrival,
                        DateOfDeparture = expectedDateOfDeparture,
                        Description = expectedDescription
                    }));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.VisitId.ShouldBe(expectedVisitId);
            response.VehicleId.ShouldBe(expectedVehicleId);
            response.DateOfArrival.ShouldBe(expectedDateOfArrival);
            response.DateOfDeparture.ShouldBe(expectedDateOfDeparture);
            response.Description.ShouldBe(expectedDescription);
        }
    }
}
