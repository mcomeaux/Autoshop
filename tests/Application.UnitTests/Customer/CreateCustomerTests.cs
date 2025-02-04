using Autoshop.Application.Customer;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;

namespace Autoshop.Application.UnitTests.Customer
{
    public class CreateCustomerTests : UnitTestBase
    {
        private CreateCustomer.Command _command;
        private CreateCustomer.CommandHandler _handler;
        private Mock<ILogger<CreateCustomer.CommandHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CreateCustomer.CommandHandler>>();

            _command = new CreateCustomer.Command();
            _handler = new CreateCustomer.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        [Test]
        public async Task CreateCustomerCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedCustomerId = 1;
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            var customerRequest = new CustomerRequest{
                Name = expectedName,
                Email = expectedEmail,
                PhoneNumber = expectedPhoneNumber,
                Address = expectedAddress
            };
            _command.CreateRequest = customerRequest;


            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertCustomerCommand.Command>(
                    c => c.Name == expectedName
                    && c.Email == expectedEmail
                    && c.PhoneNumber == expectedPhoneNumber
                    && c.Address == expectedAddress)
                , CancellationToken.None)).Returns(Task.FromResult(
                    new Entities.Customer { 
                        CustomerId = expectedCustomerId,
                        Name = expectedName,
                        Email = expectedEmail,
                        PhoneNumber = expectedPhoneNumber,
                        Address = expectedAddress
                    }));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.CustomerId.ShouldBe(expectedCustomerId);
            response.Name.ShouldBe(expectedName);
            response.Email.ShouldBe(expectedEmail);
            response.PhoneNumber.ShouldBe(expectedPhoneNumber);
            response.Address.ShouldBe(expectedAddress);
        }
    }
}
