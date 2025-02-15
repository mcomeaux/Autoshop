using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetInvoicesByCustomerIdQueryTests : UnitTestBase
    {
        private GetInvoicesByCustomerIdQuery.Command _command;
        private GetInvoicesByCustomerIdQuery.CommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _command = new GetInvoicesByCustomerIdQuery.Command
            {
                CustomerId = 0
            };

            _handler = new GetInvoicesByCustomerIdQuery.CommandHandler(ApplicationDbContext);
        }


        [Test]
        public async Task GetInvoicesByCustomerIdQuery_ShouldReturnEmptyList_WhenTheCustomerDoesNotExist()
        {
            // Arrange
            var customerId = 11;
            _command.CustomerId = customerId;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(0);
        }

        [Test]
        public async Task GetInvoicesByCustomerIdQuery_Should_ReturnAListOfInvoices()
        {
            // Arrange
            var customerId = 1;
            _command.CustomerId = customerId;

            var expectedInvoice1 = new Entities.Invoice { 
                    InvoiceId = 1,
                    VisitId = 2,
                    CustomerId = customerId,
                    CreatedDate = DateTime.Now.AddDays(-1),
                    PaidOffDate = null,
                    TotalCost = 100.0,
                    AmountPaid = 10.0,
                    Description = "Test Description1"
                };
            
            var expectedInvoice2 = new Entities.Invoice { 
                    InvoiceId = 2,
                    VisitId = 3,
                    CustomerId = customerId,
                    CreatedDate = DateTime.Now.AddDays(-2),
                    PaidOffDate = null,
                    TotalCost = 200.0,
                    AmountPaid = 20.0,
                    Description = "Test Description2"
                };

            

            ApplicationDbContext.Invoices.Add(expectedInvoice1);
            ApplicationDbContext.Invoices.Add(expectedInvoice2);
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.Count.ShouldBe(2);
            result.Contains(expectedInvoice1).ShouldBe(true);
            result.Contains(expectedInvoice2).ShouldBe(true);
        }
    }
}
