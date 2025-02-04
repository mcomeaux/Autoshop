using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using Azure.Core;
using System.Collections.Generic;
using System;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetCustomersQueryTests : UnitTestBase
    {
        private GetCustomersQuery.Command _command;
        private GetCustomersQuery.CommandHandler _handler;

        private static Dictionary<string, Entities.Customer> _customers;

        [SetUp]
        public void Setup()
        {
            _command = new GetCustomersQuery.Command
            {
                Name = "",
                PhoneNumber = ""
            };

            _handler = new GetCustomersQuery.CommandHandler(ApplicationDbContext);
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
            _customers.Add("customer3", new Entities.Customer { 
                    CustomerId = 3,
                    Name = "Tester Three",
                    Email = "tester3@gmail.com",
                    PhoneNumber = "3333333333",
                    Address = "333 testing st"
                });

            ApplicationDbContext.Customers.Add(_customers["customer1"]);
            ApplicationDbContext.Customers.Add(_customers["customer2"]);
            ApplicationDbContext.Customers.Add(_customers["customer3"]);
            ApplicationDbContext.SaveChanges();
        }

        [TestCase("", "", 3, true, true, true)]
        [TestCase("Test", "", 3, true, true, true)]
        [TestCase("One", "", 1, true, false, false)]
        [TestCase("", "2222222222", 1, false, true, false)]
        [TestCase("Test", "2222222222", 1, false, true, false)]
        [TestCase("One", "2222222222", 0, false, false, false)]
        [TestCase("Four", "", 0, false, false, false)]
        public async Task GetCustomersQuery_Should_ReturnAListOfCustomers(string name, string phoneNumber, int count, bool containsCustomer1, bool containsCustomer2, bool containsCustomer3)
        {
            // Arrange
            _command.Name = name;
            _command.PhoneNumber = phoneNumber;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(count);
            result.Contains(_customers["customer1"]).ShouldBe(containsCustomer1);
            result.Contains(_customers["customer2"]).ShouldBe(containsCustomer2);
            result.Contains(_customers["customer3"]).ShouldBe(containsCustomer3);
        }

    }
}
