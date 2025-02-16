using Autoshop.Application.Visit;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Autoshop.Application.Queries;
using Autoshop.Application.Exceptions;
using System.Threading;
using System;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Net.Http;
using Autoshop.Application.Common.Models;
using System.Linq;

namespace Autoshop.Application.UnitTests.Visit
{
    public class GetActiveVisitsTests : UnitTestBase
    {
        private GetActiveVisits.Command _command = null!;
        private GetActiveVisits.CommandHandler _handler = null!;
        private Mock<ILogger<GetActiveVisits.CommandHandler>> _logger = null!;
        private static Dictionary<string, Entities.Visit> _visits = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<GetActiveVisits.CommandHandler>>();

            _command = new GetActiveVisits.Command();
            _handler = new GetActiveVisits.CommandHandler(MediatorMock.Object, _logger.Object);

            _visits = new Dictionary<string, Entities.Visit>();
            _visits.Add("visit1", new Entities.Visit { 
                    VisitId = 2,
                    VehicleId = 1,
                    DateOfArrival = DateTime.Now.AddDays(-1),
                    DateOfDeparture = null,
                    Description = "Visit1"
                });
            _visits.Add("visit2", new Entities.Visit { 
                    VisitId = 3,
                    VehicleId = 1,
                    DateOfArrival = DateTime.Now.AddDays(-2),
                    DateOfDeparture = null,
                    Description = "Visit2"
                });
            _visits.Add("visit3", new Entities.Visit { 
                    VisitId = 4,
                    VehicleId = 2,
                    DateOfArrival = DateTime.Now.AddDays(-2),
                    DateOfDeparture = DateTime.Now.AddDays(-1),
                    Description = "Visit3"
                });
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        
        [Test]
        public async Task GetActiveVisitsCommandHandler_ShouldReturnEmptyList_WhenNoVisitsAreFound()
        {
            // Arrange

            MediatorMock.Setup(m => m.Send(
                It.Is<GetActiveVisitsQuery.Command>(
                    c => true), CancellationToken.None
                )).Returns(Task.FromResult(new List<Entities.Visit>()));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);
            
            // Assert
            VerifyAll();
            response.Count.ShouldBe(0);

        }

        [Test]
        public async Task GetActiveVisitsCommandHandler_ShoulReturndAListOfVisitObjects_WhenActiveVisitsAreFound()
        {
            // Arrange

            var visitList = new List<Entities.Visit>();
            visitList.Add(_visits["visit1"]);
            visitList.Add(_visits["visit2"]);

            MediatorMock.Setup(m => m.Send(
                It.Is<GetActiveVisitsQuery.Command>(
                    c => true)
                , CancellationToken.None)).Returns(Task.FromResult(visitList));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.Count.ShouldBe(2);
            response.Any(c => 
                    c.VisitId == _visits["visit1"].VisitId
                    && c.VehicleId == _visits["visit1"].VehicleId
                    && c.DateOfArrival == _visits["visit1"].DateOfArrival
                    && c.DateOfDeparture == _visits["visit1"].DateOfDeparture
                    && c.Description == _visits["visit1"].Description
                ).ShouldBeTrue();
            response.Any(c => 
                    c.VisitId == _visits["visit2"].VisitId
                    && c.VehicleId == _visits["visit2"].VehicleId
                    && c.DateOfArrival == _visits["visit2"].DateOfArrival
                    && c.DateOfDeparture == _visits["visit2"].DateOfDeparture
                    && c.Description == _visits["visit2"].Description
                ).ShouldBeTrue();
        }
    }
}
