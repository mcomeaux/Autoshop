using System;

namespace Autoshop.Application.Common.Interfaces
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
