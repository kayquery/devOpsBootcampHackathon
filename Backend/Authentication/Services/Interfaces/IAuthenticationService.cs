using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loan.Authentication.Domain.Models;

namespace Loan.Authentication.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginRequest loginRequest);
        Task<string> RegisterUser(RegisterRequest registerRequest);
    }
}