using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetVisitsByVehicleIdQueryTests : UnitTestBase
    {
        private GetVisitsByVehicleIdQuery.Command _command = null!;
        private GetVisitsByVehicleIdQuery.CommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _command = new GetVisitsByVehicleIdQuery.Command
            {
                VehicleId = 0
            };

            _handler = new GetVisitsByVehicleIdQuery.CommandHandler(ApplicationDbContext);
        }


        [Test]
        public async Task GetVisitsByVehicleIdQuery_ShouldReturnEmptyList_WhenTheVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = 11;
            _command.VehicleId = vehicleId;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(0);
        }

        [Test]
        public async Task GetVisitsByVehicleIdQuery_Should_ReturnAListOfVisits()
        {
            // Arrange
            var vehicleId = 1;
            _command.VehicleId = vehicleId;

            var expectedVisit1 = new Entities.Visit {
                VisitId = 2,
                VehicleId = vehicleId,
                DateOfArrival = DateTime.Now.AddDays(-2),
                DateOfDeparture = DateTime.Now.AddDays(-1),
                Description = "Visit1"
            };

            var expectedVisit2 = new Entities.Visit {
                VisitId = 3,
                VehicleId = vehicleId,
                DateOfArrival = DateTime.Now.AddDays(-1),
                DateOfDeparture = null,
                Description = "Visit2"
            };

            ApplicationDbContext.Visits.Add(expectedVisit1);
            ApplicationDbContext.Visits.Add(expectedVisit2);
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.Contains(expectedVisit1).ShouldBe(true);
            result.Contains(expectedVisit2).ShouldBe(true);
        }
    }
}
