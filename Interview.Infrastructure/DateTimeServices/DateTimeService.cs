using Interview.Application.Interfaces;
using System;

namespace Interview.Infrastructure.DateTimeServices
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetTime() => DateTime.UtcNow;
    }
}
