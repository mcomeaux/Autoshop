using Autoshop.Application.Commands;
using System.Threading.Tasks;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Autoshop.Application.UnitTests.Commands
{
    public class UpsertInvoiceCommandTests : UnitTestBase
    {
        private UpsertInvoiceCommand.Command _command = null!;
        private UpsertInvoiceCommand.CommandHandler _handler = null!;

        [SetUp]
        public void Setup()
        {
            _command = new UpsertInvoiceCommand.Command
            {
                VisitId = 1,
                CustomerId = 1,
                CreatedDate = DateTime.Now.AddDays(-3),
                PaidOffDate = null,
                TotalCost = 100.0,
                AmountPaid = 0.0,
                Description = ""
            };

            _handler = new UpsertInvoiceCommand.CommandHandler(ApplicationDbContext);
        }

        //TODO: Add test for InvoiceId Not found
        //TODO: Add test for VisitId Not found
        //TODO: Add test for CustomerId Not found


        [Test]
        public async Task UpsertInvoiceCommand_ShouldAddNewInvoice_WithValidData()
        {
            // Arrange
            int expectedVisitId = 1;
            int expectedCustomerId = 1;
            DateTime expectedCreatedDate = DateTime.Now.AddDays(-3);
            DateTime? expectedPaidOffDate = null;
            double expectedTotalCost = 100.0;
            double expectedAmountPaid = 0.0;
            string expectedDescription = "Test Description";
            _command.VisitId = expectedVisitId;
            _command.CustomerId = expectedCustomerId;
            _command.CreatedDate = expectedCreatedDate;
            _command.PaidOffDate = expectedPaidOffDate;
            _command.TotalCost = expectedTotalCost;
            _command.AmountPaid = expectedAmountPaid;
            _command.Description = expectedDescription;

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.VisitId.ShouldBe(expectedVisitId);
            result.CustomerId.ShouldBe(expectedCustomerId);
            result.CreatedDate.ShouldBe(expectedCreatedDate);
            result.PaidOffDate.ShouldBe(expectedPaidOffDate);
            result.TotalCost.ShouldBe(expectedTotalCost);
            result.AmountPaid.ShouldBe(expectedAmountPaid);
            result.Description.ShouldBe(expectedDescription);

            var savedValue = ApplicationDbContext
                .Invoices
                .FirstOrDefault(e => e.InvoiceId == result.InvoiceId);
            
            savedValue.ShouldNotBeNull();
            savedValue.VisitId.ShouldBe(expectedVisitId);
            savedValue.CustomerId.ShouldBe(expectedCustomerId);
            savedValue.CreatedDate.ShouldBe(expectedCreatedDate);
            savedValue.PaidOffDate.ShouldBe(expectedPaidOffDate);
            savedValue.TotalCost.ShouldBe(expectedTotalCost);
            savedValue.AmountPaid.ShouldBe(expectedAmountPaid);
            savedValue.Description.ShouldBe(expectedDescription);
        }

        [Test]
        public async Task UpsertInvoiceCommand_ShouldUpdateAnExistingInvoice_WithValidData()
        {
            // Arrange
            int invoiceId = 1;
            int expectedVisitId = 1;
            int expectedCustomerId = 1;
            DateTime expectedCreatedDate = DateTime.Now.AddDays(-3);
            DateTime? expectedPaidOffDate = DateTime.Now.AddDays(-2);
            double expectedTotalCost = 100.0;
            double expectedAmountPaid = 100.0;
            string expectedDescription = "Test Description";
            _command.InvoiceId = invoiceId;
            _command.VisitId = expectedVisitId;
            _command.CustomerId = expectedCustomerId;
            _command.CreatedDate = expectedCreatedDate;
            _command.PaidOffDate = expectedPaidOffDate;
            _command.TotalCost = expectedTotalCost;
            _command.AmountPaid = expectedAmountPaid;
            _command.Description = expectedDescription;

            ApplicationDbContext.Invoices.Add(
                new Entities.Invoice { 
                    InvoiceId = invoiceId,
                    VisitId = 2,
                    CustomerId = 2,
                    CreatedDate = DateTime.Now.AddDays(-2),
                    PaidOffDate = null,
                    TotalCost = 151.0,
                    AmountPaid = 10.0,
                    Description = ""
                });
            ApplicationDbContext.SaveChanges();

            // Act
            var result = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            result.VisitId.ShouldBe(expectedVisitId);
            result.CustomerId.ShouldBe(expectedCustomerId);
            result.CreatedDate.ShouldBe(expectedCreatedDate);
            result.PaidOffDate.ShouldBe(expectedPaidOffDate);
            result.TotalCost.ShouldBe(expectedTotalCost);
            result.AmountPaid.ShouldBe(expectedAmountPaid);
            result.Description.ShouldBe(expectedDescription);

            var savedValue = ApplicationDbContext
                .Invoices
                .FirstOrDefault(e => e.InvoiceId == invoiceId);
            
            savedValue.ShouldNotBeNull();
            savedValue.VisitId.ShouldBe(expectedVisitId);
            savedValue.CustomerId.ShouldBe(expectedCustomerId);
            savedValue.CreatedDate.ShouldBe(expectedCreatedDate);
            savedValue.PaidOffDate.ShouldBe(expectedPaidOffDate);
            savedValue.TotalCost.ShouldBe(expectedTotalCost);
            savedValue.AmountPaid.ShouldBe(expectedAmountPaid);
            savedValue.Description.ShouldBe(expectedDescription);
        }
    }
}
