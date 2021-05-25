﻿using Lipar.Infrastructure.Tools.Utilities.Services;
using System;

namespace Lipar.Infrastructure.Tools.Utilities.Implementation
{
    public class UtcDateTime : IDateTime
    {
        public DateTime DateTime => DateTime.UtcNow;
    }
}