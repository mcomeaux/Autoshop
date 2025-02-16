using Autoshop.Application.Queries;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System;

namespace Autoshop.Application.UnitTests.Queries
{
    public class GetInvoiceByVisitIdQueryTests : UnitTestBase
    {
        private GetInvoiceByVisitIdQuery.Command _command = null!;
        private GetInvoiceByVisitIdQuery.CommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _command = new GetInvoiceByVisitIdQuery.Command
            {
                VisitId = 0
            };

            _handler = new GetInvoiceByVisitIdQuery.CommandHandler(ApplicationDbContext);
        }

        [Test]
        public async Task GetInvoiceByVisitIdQuery_ShouldReturnNull_WhenNoInvoiceExistsForTheVisit()
        {
            // Arrange
            var visitId = 11;
            _command.VisitId = visitId;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
        }

        [Test]
        public async Task GetInvoiceByVisitIdQuery_Should_ReturnAnInvoice()
        {
            // Arrange
            int expectedInvoiceId = 1;
            int expectedVisitId = 1;
            int expectedCustomerId = 1;
            DateTime expectedCreatedDate = DateTime.Now.AddDays(-3);
            DateTime? expectedPaidOffDate = DateTime.Now.AddDays(-2);
            double expectedTotalCost = 100.0;
            double expectedAmountPaid = 100.0;
            string expectedDescription = "Test Description";
            _command.VisitId = expectedVisitId;

            ApplicationDbContext.Invoices.Add(
                new Entities.Invoice { 
                    InvoiceId = expectedInvoiceId,
                    VisitId = expectedVisitId,
                    CustomerId = expectedCustomerId,
                    CreatedDate = expectedCreatedDate,
                    PaidOffDate = expectedPaidOffDate,
                    TotalCost = expectedTotalCost,
                    AmountPaid = expectedAmountPaid,
                    Description = expectedDescription
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.InvoiceId.ShouldBe(expectedInvoiceId);
            result.VisitId.ShouldBe(expectedVisitId);
            result.CustomerId.ShouldBe(expectedCustomerId);
            result.CreatedDate.ShouldBe(expectedCreatedDate);
            result.PaidOffDate.ShouldBe(expectedPaidOffDate);
            result.TotalCost.ShouldBe(expectedTotalCost);
            result.AmountPaid.ShouldBe(expectedAmountPaid);
            result.Description.ShouldBe(expectedDescription);
        }
    }
}
