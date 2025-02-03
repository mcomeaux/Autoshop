
using Autoshop.Infrastructure;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Autoshop.Application.UnitTests
{
    public class UnitTestBase
    {
        public ApplicationDbContext ApplicationDbContext { get; private set; }

        public Mock<IMediator> MediatorMock { get; private set; }


        [SetUp]
        public void Setup()
        {
            ApplicationDbContext = ApplicationDbContextFactory.Create();
            MediatorMock = new Mock<IMediator>();
        }

        [TearDown]
        public void Teardown()
        {
            ApplicationDbContextFactory.Destroy(ApplicationDbContext);
        }
    }
}