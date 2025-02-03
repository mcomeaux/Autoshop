using Autoshop.Application.Customer;
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

namespace Autoshop.Application.UnitTests.Customer
{
    public class GetCustomerTests : UnitTestBase
    {
        private GetCustomer.Command _command;
        private GetCustomer.CommandHandler _handler;
        private Mock<ILogger<GetCustomer.CommandHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<GetCustomer.CommandHandler>>();

            _command = new GetCustomer.Command();
            _handler = new GetCustomer.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        
        [Test]
        public async Task GetCustomerCommandHandler_ShouldThrowNotFound_WhenCustomerNotFound()
        {
            // Arrange
            var expectedCustomerId = 0;
            string expectedErrorMessage = $"Customer with Id [{expectedCustomerId}] was not found.";
            _command.CustomerId = expectedCustomerId;

            MediatorMock.Setup(m => m.Send(
                It.Is<GetCustomerByIdQuery.Command>(
                    c => c.CustomerId == expectedCustomerId)
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

            //TODO: Check for notfound exception
        }

        [Test]
        public async Task GetCustomerCommandHandler_ShouldReturnObject_WhenQuestionFound()
        {
            // Arrange
            var expectedCustomerId = 0;
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            _command.CustomerId = expectedCustomerId;


            MediatorMock.Setup(m => m.Send(
                It.Is<GetCustomerByIdQuery.Command>(
                    c => c.CustomerId == expectedCustomerId)
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
