using Lipar.Core.Contract.Services;
using System;

namespace Lipar.Infrastructure.Tools.Utilities.Implementation
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
