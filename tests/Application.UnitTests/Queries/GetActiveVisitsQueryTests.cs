using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetActiveVisitsQueryTests : UnitTestBase
    {
        private GetActiveVisitsQuery.Command _command;
        private GetActiveVisitsQuery.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new GetActiveVisitsQuery.Command
            {
            };

            _handler = new GetActiveVisitsQuery.CommandHandler(ApplicationDbContext);
        }


        [Test]
        public async Task GetActiveVisitsQuery_ShouldReturnEmptyList_WhenNoVisitsAreActive()
        {
            // Arrange
            var vehicleId = 1;

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
                DateOfDeparture = DateTime.Now,
                Description = "Visit2"
            };

            ApplicationDbContext.Visits.Add(expectedVisit1);
            ApplicationDbContext.Visits.Add(expectedVisit2);
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(0);
        }

        [Test]
        public async Task GetActiveVisitsQuery_Should_ReturnAListOfVisits()
        {
            // Arrange
            var vehicleId = 1;

            var expectedVisit1 = new Entities.Visit {
                VisitId = 2,
                VehicleId = vehicleId,
                DateOfArrival = DateTime.Now.AddDays(-2),
                DateOfDeparture = null,
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
