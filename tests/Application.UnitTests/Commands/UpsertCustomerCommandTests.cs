using Autoshop.Application.Commands;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace Autoshop.Application.UnitTests.Commands
{
    public class UpsertCustomerCommandTests : UnitTestBase
    {
        private UpsertCustomerCommand.Command _command;
        private UpsertCustomerCommand.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new UpsertCustomerCommand.Command
            {
                CustomerId = null,
                Name = "",
                Email = "",
                PhoneNumber = "",
                Address = ""
            };

            _handler = new UpsertCustomerCommand.CommandHandler(ApplicationDbContext);
        }

        //TODO: Add test for CustomerId Not found

        [Test]
        public async Task UpsertCustomerCommand_ShouldAddNewCustomer_WithValidData()
        {
            // Arrange
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            _command.Name = expectedName;
            _command.Email = expectedEmail;
            _command.PhoneNumber = expectedPhoneNumber;
            _command.Address = expectedAddress;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Name.ShouldBe(expectedName);
            result.Email.ShouldBe(expectedEmail);
            result.PhoneNumber.ShouldBe(expectedPhoneNumber);
            result.Address.ShouldBe(expectedAddress);

            var savedValue = ApplicationDbContext
                .Customers
                .FirstOrDefault(e => e.CustomerId == result.CustomerId);
            
            savedValue.ShouldNotBeNull();
            savedValue.Name.ShouldBe(expectedName);
            savedValue.Email.ShouldBe(expectedEmail);
            savedValue.PhoneNumber.ShouldBe(expectedPhoneNumber);
            savedValue.Address.ShouldBe(expectedAddress);
        }

        [Test]
        public async Task UpsertCustomerCommand_ShouldUpdateAnExistingCustomer_WithValidData()
        {
            // Arrange
            var expectedCustomerId = 1;
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            _command.CustomerId = expectedCustomerId;
            _command.Name = expectedName;
            _command.Email = expectedEmail;
            _command.PhoneNumber = expectedPhoneNumber;
            _command.Address = expectedAddress;


            ApplicationDbContext.Customers.Add(
                new Entities.Customer { 
                    Name = "",
                    Email = "",
                    PhoneNumber = "",
                    Address = ""
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.CustomerId.ShouldBe(expectedCustomerId);
            result.Name.ShouldBe(expectedName);
            result.Email.ShouldBe(expectedEmail);
            result.PhoneNumber.ShouldBe(expectedPhoneNumber);
            result.Address.ShouldBe(expectedAddress);

            var savedValue = ApplicationDbContext
                .Customers
                .FirstOrDefault(e => e.CustomerId == result.CustomerId);

            savedValue.ShouldNotBeNull();
            savedValue.CustomerId.ShouldBe(expectedCustomerId);
            savedValue.Name.ShouldBe(expectedName);
            savedValue.Email.ShouldBe(expectedEmail);
            savedValue.PhoneNumber.ShouldBe(expectedPhoneNumber);
            savedValue.Address.ShouldBe(expectedAddress);
        }
    }
}
