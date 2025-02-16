using Autoshop.Application.Commands;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Autoshop.Application.UnitTests.Commands
{
    public class UpsertVisitCommandTests : UnitTestBase
    {
        private UpsertVisitCommand.Command _command = null!;
        private UpsertVisitCommand.CommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _command = new UpsertVisitCommand.Command
            {
                VehicleId = 1,
                DateOfArrival = DateTime.Now.AddDays(-3),
                DateOfDeparture = null,
                Description = ""
            };

            _handler = new UpsertVisitCommand.CommandHandler(ApplicationDbContext);
        }

        //TODO: Add test for VisitId Not found
        //TODO: Add test for VehicleId Not found


        [Test]
        public async Task UpsertVisitCommand_ShouldAddNewVisit_WithValidData()
        {
            // Arrange
            int expectedVehicleId = 1;
            DateTime expectedDateOfArrival = DateTime.Now.AddDays(-3);
            DateTime? expectedDateOfDeparture = null;
            string expectedDescription = "Test Description";
            _command.VehicleId = expectedVehicleId;
            _command.DateOfArrival = expectedDateOfArrival;
            _command.DateOfDeparture = expectedDateOfDeparture;
            _command.Description = expectedDescription;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.VehicleId.ShouldBe(expectedVehicleId);
            result.DateOfArrival.ShouldBe(expectedDateOfArrival);
            result.DateOfDeparture.ShouldBe(expectedDateOfDeparture);
            result.Description.ShouldBe(expectedDescription);

            var savedValue = ApplicationDbContext
                .Visits
                .FirstOrDefault(e => e.VisitId == result.VisitId);
            
            savedValue.ShouldNotBeNull();
            savedValue.VehicleId.ShouldBe(expectedVehicleId);
            savedValue.DateOfArrival.ShouldBe(expectedDateOfArrival);
            savedValue.DateOfDeparture.ShouldBe(expectedDateOfDeparture);
            savedValue.Description.ShouldBe(expectedDescription);
        }

        [Test]
        public async Task UpsertVisitCommand_ShouldUpdateAnExistingVisit_WithValidData()
        {
            // Arrange
            var visitId = 1;
            var expectedVehicleId = 2;
            DateTime expectedDateOfArrival = DateTime.Now.AddDays(-3);
            DateTime? expectedDateOfDeparture = DateTime.Now;
            var expectedDescription = "Test Description";
            _command.VisitId = visitId;
            _command.VehicleId = expectedVehicleId;
            _command.DateOfArrival = expectedDateOfArrival;
            _command.DateOfDeparture = expectedDateOfDeparture;
            _command.Description = expectedDescription;

            ApplicationDbContext.Visits.Add(
                new Entities.Visit { 
                    VisitId = visitId,
                    VehicleId = 1,
                    DateOfArrival = DateTime.Now.AddDays(-1),
                    DateOfDeparture = null,
                    Description = ""
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.VisitId = visitId;
            result.VehicleId = expectedVehicleId;
            result.DateOfArrival = expectedDateOfArrival;
            result.DateOfDeparture.ShouldBe(expectedDateOfDeparture);
            result.Description.ShouldBe(expectedDescription);

            var savedValue = ApplicationDbContext
                .Visits
                .FirstOrDefault(e => e.VisitId == visitId);
            
            savedValue.ShouldNotBeNull();
            savedValue.VehicleId.ShouldBe(expectedVehicleId);
            savedValue.DateOfArrival.ShouldBe(expectedDateOfArrival);
            savedValue.DateOfDeparture.ShouldBe(expectedDateOfDeparture);
            savedValue.Description.ShouldBe(expectedDescription);
        }
    }
}
