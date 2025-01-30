using Autoshop.Application.Common.Interfaces;
using System;

namespace Autoshop.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
