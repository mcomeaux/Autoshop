using Autoshop.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;


namespace Autoshop.Application.UnitTests
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


            var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();

            SeedSampleData(context);

            return context;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {
            //context.AddRegExRule("MatchMe RegEx Rule", Guid.NewGuid(),"MatchMe .*");
            //context.AddCheckListRule("Simple CheckList Rule", Guid.NewGuid(), new[] {"value1", "value2", "value3"});
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }



    }
}