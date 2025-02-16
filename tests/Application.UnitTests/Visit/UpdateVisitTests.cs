using Autoshop.Application.Visit;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Exceptions;
using Microsoft.VisualBasic;
using System;

namespace Autoshop.Application.UnitTests.Visit
{
    public class UpdateVisitTests : UnitTestBase
    {
        private UpdateVisit.Command _command = null!;
        private UpdateVisit.CommandHandler _handler = null!;
        private Mock<ILogger<UpdateVisit.CommandHandler>> _logger = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<UpdateVisit.CommandHandler>>();

            _command = new UpdateVisit.Command();
            _handler = new UpdateVisit.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }


        [Test]
        public async Task UpdateVisitCommandHandler_ShouldThrowNotFound_WhenVisitNotFound()
        {
            // Arrange
            var expectedVisitId = 2;
            var expectedVehicleId = 3;
            var expectedDateOfArrival = DateTime.Now.AddDays(-5);
            var expectedDateOfDeparture = DateTime.Now.AddDays(-3);
            var expectedDescription = "Test Description";
            
            var visitRequest = new VisitDto{
                VisitId = expectedVisitId,
                VehicleId = expectedVehicleId,
                DateOfArrival = expectedDateOfArrival,
                DateOfDeparture = expectedDateOfDeparture,
                Description = expectedDescription
            };
            _command.UpdateRequest = visitRequest;
            string expectedErrorMessage = $"Visit with Id [{expectedVisitId}] was not found.";

            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertVisitCommand.Command>(
                    c => c.VisitId == expectedVisitId
                    && c.VehicleId == expectedVehicleId
                    && c.DateOfArrival == expectedDateOfArrival
                    && c.DateOfDeparture == expectedDateOfDeparture
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult<Entities.Visit>(null));

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
        public async Task UpdateVisitCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedVisitId = 2;
            var expectedVehicleId = 3;
            var expectedDateOfArrival = DateTime.Now.AddDays(-5);
            var expectedDateOfDeparture = DateTime.Now.AddDays(-3);
            var expectedDescription = "Test Description";

            
            var visitRequest = new VisitDto{
                VisitId = expectedVisitId,
                VehicleId = expectedVehicleId,
                DateOfArrival = expectedDateOfArrival,
                DateOfDeparture = expectedDateOfDeparture,
                Description = expectedDescription
            };
            _command.UpdateRequest = visitRequest;


            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertVisitCommand.Command>(
                    c => c.VisitId == expectedVisitId
                    && c.VehicleId == expectedVehicleId
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
