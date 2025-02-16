using Autoshop.Application.Customer;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Exceptions;

namespace Autoshop.Application.UnitTests.Customer
{
    public class UpdateCustomerTests : UnitTestBase
    {
        private UpdateCustomer.Command _command = null!;
        private UpdateCustomer.CommandHandler _handler = null!;
        private Mock<ILogger<UpdateCustomer.CommandHandler>> _logger = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<UpdateCustomer.CommandHandler>>();

            _command = new UpdateCustomer.Command();
            _handler = new UpdateCustomer.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        [Test]
        public async Task UpdateCustomerCommandHandler_ShouldThrowNotFound_WhenCustomerNotFound()
        {
            // Arrange
            var expectedCustomerId = 1;
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            var customerRequest = new CustomerDto {
                CustomerId = expectedCustomerId,
                Name = expectedName,
                Email = expectedEmail,
                PhoneNumber = expectedPhoneNumber,
                Address = expectedAddress
            };
            _command.UpdateRequest = customerRequest;
            string expectedErrorMessage = $"Customer with Id [{expectedCustomerId}] was not found.";

            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertCustomerCommand.Command>(
                    c => c.Name == expectedName
                    && c.Email == expectedEmail
                    && c.PhoneNumber == expectedPhoneNumber
                    && c.Address == expectedAddress)
                , CancellationToken.None)).Returns(Task.FromResult<Entities.Customer>(null));


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
        public async Task UpdateCustomerCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedCustomerId = 1;
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            var customerRequest = new CustomerDto {
                CustomerId = expectedCustomerId,
                Name = expectedName,
                Email = expectedEmail,
                PhoneNumber = expectedPhoneNumber,
                Address = expectedAddress
            };
            _command.UpdateRequest = customerRequest;


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
