using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loan.Authentication.Domain.Exceptions
{
    public class MappedException : Exception
    {
        public MappedException(string? message) : base(message)
        {
        }
    }
}