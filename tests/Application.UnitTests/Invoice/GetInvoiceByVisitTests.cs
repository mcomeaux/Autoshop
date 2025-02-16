using Autoshop.Application.Invoice;
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

namespace Autoshop.Application.UnitTests.Invoice
{
    public class GetInvoiceByVisitTests : UnitTestBase
    {
        private GetInvoiceByVisit.Command _command = null!;
        private GetInvoiceByVisit.CommandHandler _handler = null!;
        private Mock<ILogger<GetInvoiceByVisit.CommandHandler>> _logger = null!;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<GetInvoiceByVisit.CommandHandler>>();

            _command = new GetInvoiceByVisit.Command();
            _handler = new GetInvoiceByVisit.CommandHandler(MediatorMock.Object, _logger.Object);
        }

        private void VerifyAll()
        {
            MediatorMock.VerifyAll();
            _logger.VerifyAll();
        }

        
        [Test]
        public async Task GetInvoiceByVisitCommandHandler_ShouldThrowNotFound_WhenVisitNotFound()
        {
            // Arrange
            var expectedVisitId = 0;
            string expectedErrorMessage = $"Invoice with VisitId [{expectedVisitId}] was not found.";
            _command.VisitId = expectedVisitId;

            MediatorMock.Setup(m => m.Send(
                It.Is<GetInvoiceByVisitIdQuery.Command>(
                    c => c.VisitId == expectedVisitId)
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
        public async Task GetInvoiceByVisitCommandHandler_ShouldReturnObject_WhenInvoiceFound()
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
            _command.VisitId = expectedVisitId;


            MediatorMock.Setup(m => m.Send(
                It.Is<GetInvoiceByVisitIdQuery.Command>(
                    c => c.VisitId == expectedVisitId)
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
