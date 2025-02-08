using MediatR;
using Microsoft.EntityFrameworkCore;
using Autoshop.Application.Interfaces;
using Autoshop.Application.Entities;

namespace Autoshop.Application.Commands
{
    public class UpsertVisitCommand
    {
        public class Command : IRequest<Entities.Visit>
        {
            public int? VisitId { get; set; }
            public int VehicleId { get; set; }
            public DateTime DateOfArrival { get; set; }
            public DateTime? DateOfDeparture { get; set; }
            public string Description { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Entities.Visit>
        {
            private readonly IApplicationDbContext _dbContext;

            public CommandHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<Entities.Visit> Handle(Command request, CancellationToken cancellationToken)
            {
                Entities.Visit visit;
                if(request.VisitId != null)
                {
                    visit = await _dbContext.Visits.FirstOrDefaultAsync(v => v.VisitId == request.VisitId);
                    if(visit == null){
                        throw(new Exception());//TODO: not found exception
                    }
                    visit.VehicleId = request.VehicleId;
                    visit.DateOfArrival = request.DateOfArrival;
                    visit.DateOfDeparture = request.DateOfDeparture;
                    visit.Description = request.Description;
                }
                else{
                    var createdVisit = await _dbContext.Visits.AddAsync(new Entities.Visit
                    {
                        VehicleId = request.VehicleId,
                        DateOfArrival = request.DateOfArrival,
                        DateOfDeparture = request.DateOfDeparture,
                        Description = request.Description,
                    });
                    visit = createdVisit.Entity;
                }
                

                try
                {
                    var result = await _dbContext.SaveChangesAsync(cancellationToken);

                    return visit;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}