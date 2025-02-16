using Autoshop.Application.Vehicle;
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

namespace Autoshop.Application.UnitTests.Vehicle
{
    public class GetVehiclesByCustomerTests : UnitTestBase
    {
        private GetVehiclesByCustomer.Command _command = null!;
        private GetVehiclesByCustomer.CommandHandler _handler = null!;
        private Mock<ILogger<GetVehiclesByCustomer.CommandHandler>> _logger = null!;
        private static Dictionary<string, Entities.Vehicle> _vehicles = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<GetVehiclesByCustomer.CommandHandler>>();

            _command = new GetVehiclesByCustomer.Command();
            _handler = new GetVehiclesByCustomer.CommandHandler(MediatorMock.Object, _logger.Object);

            _vehicles = new Dictionary<string, Entities.Vehicle>();
            _vehicles.Add("vehicle1", new Entities.Vehicle { 
                    VehicleId = 2,
                    CustomerId = 1,
                    Make = "Honda",
                    Model = "Accord",
                    Year = 2005,
                    Color = "Tan",
                    VIN = "Test VIN1",
                    Description = "Vehicle1"
                });
            _vehicles.Add("vehicle2", new Entities.Vehicle { 
                    VehicleId = 3,
                    CustomerId = 1,
                    Make = "GMC",
                    Model = "Sierra 1500",
                    Year = 2024,
                    Color = "Black",
                    VIN = "Test VIN2",
                    Description = "Vehicle2"
                });
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        
        [Test]
        public async Task GetVehiclesByCustomerCommandHandler_ShouldReturnEmptyList_WhenNoVehiclesAreFound()
        {
            // Arrange
            var customerId = 5;
            _command.CustomerId = customerId;
            

            MediatorMock.Setup(m => m.Send(
                It.Is<GetVehiclesByCustomerIdQuery.Command>(
                    c => c.CustomerId == customerId)
                , CancellationToken.None)).Returns(Task.FromResult(new List<Entities.Vehicle>()));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);
            
            // Assert
            VerifyAll();
            response.Count.ShouldBe(0);

        }

        [Test]
        public async Task GetVehiclesByCustomerCommandHandler_ShouldReturnAListOfVehicleObjects_WhenVehiclesAreFound()
        {
            // Arrange
            var customerId = 1;
            _command.CustomerId = customerId;

            var vehicleList = new List<Entities.Vehicle>();
            vehicleList.Add(_vehicles["vehicle1"]);
            vehicleList.Add(_vehicles["vehicle2"]);

            MediatorMock.Setup(m => m.Send(
                It.Is<GetVehiclesByCustomerIdQuery.Command>(
                    c => c.CustomerId == customerId)
                , CancellationToken.None)).Returns(Task.FromResult(vehicleList));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.Count.ShouldBe(2);
            response.Any(c => 
                    c.VehicleId == _vehicles["vehicle1"].VehicleId
                    && c.CustomerId == _vehicles["vehicle1"].CustomerId
                    && c.Make == _vehicles["vehicle1"].Make
                    && c.Model == _vehicles["vehicle1"].Model
                    && c.Year == _vehicles["vehicle1"].Year
                    && c.Color == _vehicles["vehicle1"].Color
                    && c.VIN == _vehicles["vehicle1"].VIN
                    && c.Description == _vehicles["vehicle1"].Description
                ).ShouldBeTrue();
            response.Any(c => 
                    c.VehicleId == _vehicles["vehicle2"].VehicleId
                    && c.CustomerId == _vehicles["vehicle2"].CustomerId
                    && c.Make == _vehicles["vehicle2"].Make
                    && c.Model == _vehicles["vehicle2"].Model
                    && c.Year == _vehicles["vehicle2"].Year
                    && c.Color == _vehicles["vehicle2"].Color
                    && c.VIN == _vehicles["vehicle2"].VIN
                    && c.Description == _vehicles["vehicle2"].Description
                ).ShouldBeTrue();
        }
    }
}
