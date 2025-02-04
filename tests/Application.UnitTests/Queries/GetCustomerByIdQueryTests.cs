using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetCustomerByIdQueryTests : UnitTestBase
    {
        private GetCustomerByIdQuery.Command _command;
        private GetCustomerByIdQuery.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new GetCustomerByIdQuery.Command
            {
                CustomerId = 0
            };

            _handler = new GetCustomerByIdQuery.CommandHandler(ApplicationDbContext);
        }

        [Test]
        public async Task GetCustomerByIdQuery_ShouldReturnNull_WhenTheQuestionDoesNotExist()
        {
            // Arrange
            var customerId = 11;
            _command.CustomerId = customerId;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public async Task GetCustomerByIdQuery_Should_ReturnACustomer()
        {
            // Arrange
            var customerId = 11;
            var expectedName = "testing testerson";
            var expectedEmail = "test@gmail.com";
            var expectedPhoneNumber = "1231231234";
            var expectedAddress = "111 test st.";
            _command.CustomerId = customerId;

            ApplicationDbContext.Customers.Add(
                new Entities.Customer { 
                    CustomerId = customerId,
                    Name = expectedName,
                    Email = expectedEmail,
                    PhoneNumber = expectedPhoneNumber,
                    Address = expectedAddress
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.CustomerId.ShouldBe(customerId);
            result.Name.ShouldBe(expectedName);
            result.Email.ShouldBe(expectedEmail);
            result.PhoneNumber.ShouldBe(expectedPhoneNumber);
            result.Address.ShouldBe(expectedAddress);
        }
    }
}
