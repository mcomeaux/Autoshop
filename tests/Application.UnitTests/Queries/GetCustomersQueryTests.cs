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
                PhoneNumber = "",
                Page = null,
                PageSize = null
            };

            _handler = new GetCustomersQuery.CommandHandler(ApplicationDbContext);
            _customers = new Dictionary<string, Entities.Customer>();
        }

        public void AddCustomersToDB(int customerCount)
        {
            for(int i = 1;i<=customerCount;i++)
            {
                _customers.Add("customer" + i.ToString(), new Entities.Customer { 
                    CustomerId = i,
                    Name = "Tester " + i.ToString(),
                    Email = "tester" + i.ToString() + "@gmail.com",
                    PhoneNumber = "111111" + (1000 + i).ToString(),
                    Address = (100 + i).ToString() + " testing st"
                });
                ApplicationDbContext.Customers.Add(_customers["customer" + i]);
            }

            ApplicationDbContext.SaveChanges();
        }

        [TestCase("", "", 3, true, true, true)]
        [TestCase("Test", "", 3, true, true, true)]
        [TestCase("1", "", 1, true, false, false)]
        [TestCase("", "1111111002", 1, false, true, false)]
        [TestCase("Test", "1111111002", 1, false, true, false)]
        [TestCase("1", "1111111002", 0, false, false, false)]
        [TestCase("4", "", 0, false, false, false)]
        public async Task GetCustomersQuery_Should_ReturnAListOfCustomers(string name, string phoneNumber, int count, bool containsCustomer1, bool containsCustomer2, bool containsCustomer3)
        {
            // Arrange
            AddCustomersToDB(3);
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

        //TODO: Add test for Paging
        [Test]
        public async Task GetCustomersQuery_ShouldReturnAListOf25Customers_WhenNoPagingIsPresent()
        {
            // Arrange
            AddCustomersToDB(30);
            _command.Name = "";
            _command.PhoneNumber = "";

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(25);
        }

        [Test]
        public async Task GetCustomersQuery_ShouldReturnCustomersByPage_WhenPagingIsPresent()
        {
            // Arrange
            AddCustomersToDB(4);
            _command.Name = "";
            _command.PhoneNumber = "";
            _command.Page = 2;
            _command.PageSize = 2;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.Contains(_customers["customer3"]).ShouldBe(true);
            result.Contains(_customers["customer4"]).ShouldBe(true);
        }

    }
}
