using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loan.Authentication.Domain.Exceptions;
using Loan.Authentication.Domain.Models;
using Loan.Authentication.Services.Interfaces;
using Loan.Authentication.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Loan.Authentication.Controllers
{
    public class AuthController : BaseController
    {

        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
            => _authenticationService = authenticationService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                string token = await _authenticationService.Login(loginRequest);
                return StatusCode(201, token);
            }
            catch (MappedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch 
            {
                return BadRequest("Houve algum erro no Login");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                string userId = await _authenticationService.RegisterUser(registerRequest);
                return StatusCode(201, userId);
            }
            catch (MappedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Houve algum erro no cadastro");
            }
        }
    }
}