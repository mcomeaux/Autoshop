using Autoshop.Application.Commands;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Autoshop.Application.UnitTests.Commands
{
    public class UpsertVehicleCommandTests : UnitTestBase
    {
        private UpsertVehicleCommand.Command _command;
        private UpsertVehicleCommand.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new UpsertVehicleCommand.Command
            {
                CustomerId = 1,
                Make = "",
                Model = "",
                Year = 1990,
                Color = "",
                VIN = "",
                Description = ""
            };

            _handler = new UpsertVehicleCommand.CommandHandler(ApplicationDbContext);
        }

        //TODO: Add test for CustomerId Not found
        //TODO: Add test for VehicleId Not found


        [Test]
        public async Task UpsertVehicleCommand_ShouldAddNewVehicle_WithValidData()
        {
            // Arrange
            var expectedCustomerId = 1;
            var expectedMake = "Honda";
            var expectedModel = "CRX";
            var expectedYear = 1989;
            var expectedColor = "White";
            var expectedVIN = "Test VIN";
            var expectedDescription = "Test Description";
            _command.CustomerId = expectedCustomerId;
            _command.Make = expectedMake;
            _command.Model = expectedModel;
            _command.Year = expectedYear;
            _command.Color = expectedColor;
            _command.VIN = expectedVIN;
            _command.Description = expectedDescription;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.CustomerId = expectedCustomerId;
            result.Make.ShouldBe(expectedMake);
            result.Model.ShouldBe(expectedModel);
            result.Year.ShouldBe(expectedYear);
            result.Color.ShouldBe(expectedColor);
            result.VIN.ShouldBe(expectedVIN);
            result.Description.ShouldBe(expectedDescription);

            var savedValue = ApplicationDbContext
                .Vehicles
                .FirstOrDefault(e => e.VehicleId == result.VehicleId);
            
            savedValue.ShouldNotBeNull();
            savedValue.CustomerId.ShouldBe(expectedCustomerId);
            savedValue.Make.ShouldBe(expectedMake);
            savedValue.Model.ShouldBe(expectedModel);
            savedValue.Year.ShouldBe(expectedYear);
            savedValue.Color.ShouldBe(expectedColor);
            savedValue.VIN.ShouldBe(expectedVIN);
            savedValue.Description.ShouldBe(expectedDescription);
        }

        [Test]
        public async Task UpsertVehicleCommand_ShouldUpdateAnExistingVehicle_WithValidData()
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
            _command.CustomerId = expectedCustomerId;
            _command.Make = expectedMake;
            _command.Model = expectedModel;
            _command.Year = expectedYear;
            _command.Color = expectedColor;
            _command.VIN = expectedVIN;
            _command.Description = expectedDescription;

            ApplicationDbContext.Vehicles.Add(
                new Entities.Vehicle { 
                    VehicleId = vehicleId,
                    CustomerId = 12,
                    Make = "",
                    Model = "",
                    Year = 1900,
                    Color = "",
                    VIN = "",
                    Description = ""
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.VehicleId = vehicleId;
            result.CustomerId = expectedCustomerId;
            result.Make.ShouldBe(expectedMake);
            result.Model.ShouldBe(expectedModel);
            result.Year.ShouldBe(expectedYear);
            result.Color.ShouldBe(expectedColor);
            result.VIN.ShouldBe(expectedVIN);
            result.Description.ShouldBe(expectedDescription);

            var savedValue = ApplicationDbContext
                .Vehicles
                .FirstOrDefault(e => e.VehicleId == vehicleId);
            
            savedValue.ShouldNotBeNull();
            savedValue.CustomerId.ShouldBe(expectedCustomerId);
            savedValue.Make.ShouldBe(expectedMake);
            savedValue.Model.ShouldBe(expectedModel);
            savedValue.Year.ShouldBe(expectedYear);
            savedValue.Color.ShouldBe(expectedColor);
            savedValue.VIN.ShouldBe(expectedVIN);
            savedValue.Description.ShouldBe(expectedDescription);
        }
    }
}
