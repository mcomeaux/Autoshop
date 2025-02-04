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
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Net.Http;
using Autoshop.Application.Common.Models;
using System.Linq;

namespace Autoshop.Application.UnitTests.Customer
{
    public class GetCustomersTests : UnitTestBase
    {
        private GetCustomers.Command _command;
        private GetCustomers.CommandHandler _handler;
        private Mock<ILogger<GetCustomers.CommandHandler>> _logger;
        private static Dictionary<string, Entities.Customer> _customers;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<GetCustomers.CommandHandler>>();

            _command = new GetCustomers.Command();
            _handler = new GetCustomers.CommandHandler(MediatorMock.Object, _logger.Object);

            _customers = new Dictionary<string, Entities.Customer>();
            _customers.Add("customer1", new Entities.Customer { 
                    CustomerId = 1,
                    Name = "Tester One",
                    Email = "tester1@gmail.com",
                    PhoneNumber = "1111111111",
                    Address = "111 testing st"
                });
            _customers.Add("customer2", new Entities.Customer { 
                    CustomerId = 2,
                    Name = "Tester Two",
                    Email = "tester2@gmail.com",
                    PhoneNumber = "2222222222",
                    Address = "222 testing st"
                });
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        
        [Test]
        public async Task GetCustomersCommandHandler_ShouldReturnEmptyList_WhenCustomersAreFound()
        {
            // Arrange
            var name = "";
            var phoneNumber = "";
            _command.Name = name;
            _command.PhoneNumber = phoneNumber;

            MediatorMock.Setup(m => m.Send(
                It.Is<GetCustomersQuery.Command>(
                    c => c.Name == name
                    && c.PhoneNumber == phoneNumber)
                , CancellationToken.None)).Returns(Task.FromResult(new List<Entities.Customer>()));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);
            
            // Assert
            VerifyAll();
            response.Count.ShouldBe(0);
        }

        [Test]
        public async Task GetCustomersCommandHandler_ShouldAListOdCustomerResponseObjects_WhenCustomersAreFound()
        {
            // Arrange
            var expectedName = "";
            var expectedPhoneNumber = "";
            _command.Name = expectedName;
            _command.PhoneNumber = expectedPhoneNumber;

            var customerList = new List<Entities.Customer>();
            customerList.Add(_customers["customer1"]);
            customerList.Add(_customers["customer2"]);

            MediatorMock.Setup(m => m.Send(
                It.Is<GetCustomersQuery.Command>(
                    c => c.Name == expectedName
                    && c.PhoneNumber == expectedPhoneNumber)
                , CancellationToken.None)).Returns(Task.FromResult(customerList));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.Count.ShouldBe(2);
            response.Any(c => 
                c.CustomerId == _customers["customer1"].CustomerId
                && c.Name == _customers["customer1"].Name
                && c.Email == _customers["customer1"].Email
                && c.PhoneNumber == _customers["customer1"].PhoneNumber
                && c.Address == _customers["customer1"].Address
                ).ShouldBeTrue();
            response.Any(c => 
                c.CustomerId == _customers["customer2"].CustomerId
                && c.Name == _customers["customer2"].Name
                && c.Email == _customers["customer2"].Email
                && c.PhoneNumber == _customers["customer2"].PhoneNumber
                && c.Address == _customers["customer2"].Address
                ).ShouldBeTrue();
        }
    }
}
