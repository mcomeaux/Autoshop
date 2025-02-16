using Autoshop.Application.Invoice;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Microsoft.Extensions.Logging;
using System.Threading;
using Autoshop.Application.Commands;
using Autoshop.Application.Common.Models;
using Autoshop.Application.Exceptions;
using Microsoft.VisualBasic;
using System;

namespace Autoshop.Application.UnitTests.Invoice
{
    public class UpdateInvoiceTests : UnitTestBase
    {
        private UpdateInvoice.Command _command = null!;
        private UpdateInvoice.CommandHandler _handler = null!;
        private Mock<ILogger<UpdateInvoice.CommandHandler>> _logger = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<UpdateInvoice.CommandHandler>>();

            _command = new UpdateInvoice.Command();
            _handler = new UpdateInvoice.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }


        [Test]
        public async Task UpdateInvoiceCommandHandler_ShouldThrowNotFound_WhenInvoiceNotFound()
        {
            // Arrange
            var expectedInvoiceId = 1;
            var expectedVisitId = 2;
            var expectedCustomerId = 3;
            var expectedCreatedDate = DateTime.Now.AddDays(-5);
            var expectedPaidOffDate = DateTime.Now.AddDays(-2);
            var expectedTotalCost = 100.0;
            var expectedAmountPaid = 100.0;
            var expectedDescription = "Test Description";
            
            var invoiceRequest = new InvoiceDto{
                InvoiceId = expectedInvoiceId,
                VisitId = expectedVisitId,
                CustomerId = expectedCustomerId,
                CreatedDate = expectedCreatedDate,
                PaidOffDate = expectedPaidOffDate,
                TotalCost = expectedTotalCost,
                AmountPaid = expectedAmountPaid,
                Description = expectedDescription
            };
            _command.UpdateRequest = invoiceRequest;
            string expectedErrorMessage = $"Invoice with Id [{expectedInvoiceId}] was not found.";

            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertInvoiceCommand.Command>(
                    c => c.InvoiceId == expectedInvoiceId
                    && c.VisitId == expectedVisitId
                    && c.CustomerId == expectedCustomerId
                    && c.CreatedDate == expectedCreatedDate
                    && c.PaidOffDate == expectedPaidOffDate
                    && c.TotalCost == expectedTotalCost
                    && c.AmountPaid == expectedAmountPaid
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult<Entities.Invoice>(null));

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
        public async Task UpdateInvoiceCommandHandler_ShouldReturnObject_ForValidData()
        {
            // Arrange
            var expectedInvoiceId = 1;
            var expectedVisitId = 2;
            var expectedCustomerId = 3;
            var expectedCreatedDate = DateTime.Now.AddDays(-5);
            var expectedPaidOffDate = DateTime.Now.AddDays(-2);
            var expectedTotalCost = 100.0;
            var expectedAmountPaid = 100.0;
            var expectedDescription = "Test Description";

            var invoiceRequest = new InvoiceDto{
                InvoiceId = expectedInvoiceId,
                VisitId = expectedVisitId,
                CustomerId = expectedCustomerId,
                CreatedDate = expectedCreatedDate,
                PaidOffDate = expectedPaidOffDate,
                TotalCost = expectedTotalCost,
                AmountPaid = expectedAmountPaid,
                Description = expectedDescription
            };
            _command.UpdateRequest = invoiceRequest;


            MediatorMock.Setup(m => m.Send(
                It.Is<UpsertInvoiceCommand.Command>(
                    c => c.InvoiceId == expectedInvoiceId
                    && c.VisitId == expectedVisitId
                    && c.CustomerId == expectedCustomerId
                    && c.CreatedDate == expectedCreatedDate
                    && c.PaidOffDate == expectedPaidOffDate
                    && c.TotalCost == expectedTotalCost
                    && c.AmountPaid == expectedAmountPaid
                    && c.Description == expectedDescription)
                , CancellationToken.None)).Returns(Task.FromResult(
                    new Entities.Invoice { 
                        InvoiceId = expectedInvoiceId,
                        VisitId = expectedVisitId,
                        CustomerId = expectedCustomerId,
                        CreatedDate = expectedCreatedDate,
                        PaidOffDate = expectedPaidOffDate,
                        TotalCost = expectedTotalCost,
                        AmountPaid = expectedAmountPaid,
                        Description = expectedDescription
                    }));


            // Act
            var response = await _handler.Handle(_command, CancellationToken.None);

            // Assert
            VerifyAll();
            response.InvoiceId.ShouldBe(expectedInvoiceId);
            response.VisitId.ShouldBe(expectedVisitId);
            response.CustomerId.ShouldBe(expectedCustomerId);
            response.CreatedDate.ShouldBe(expectedCreatedDate);
            response.PaidOffDate.ShouldBe(expectedPaidOffDate);
            response.TotalCost.ShouldBe(expectedTotalCost);
            response.AmountPaid.ShouldBe(expectedAmountPaid);
            response.Description.ShouldBe(expectedDescription);
        }
    }
}
