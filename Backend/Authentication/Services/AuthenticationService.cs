using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Loan.Authentication.Domain.Models;
using Loan.Authentication.Domain.Exceptions;
using Loan.Authentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using AspNetCore.Identity.Mongo.Model;

namespace Loan.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<MongoUser> _userManager;
        private readonly SignInManager<MongoUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            UserManager<MongoUser> userManager,
            SignInManager<MongoUser> signInManager,
            IConfiguration configuration)
            => (_userManager, _signInManager, _configuration) = (userManager, signInManager, configuration);


        public async Task<string> Login(LoginRequest loginRequest)
        {
            MongoUser user = await _userManager.FindByEmailAsync(loginRequest.Email) ?? throw new MappedException("Usuário não encontrado");

            SignInResult result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new MappedException("Senha inválida");
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(MongoUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public async Task<string> RegisterUser(RegisterRequest RegisterRequest)
        {
            MongoUser existingUser = await _userManager.FindByEmailAsync(RegisterRequest.Email);
            if (existingUser != null)
            {
                throw new MappedException("Usuário já existe");
            }

            MongoUser newUser = new()
            {
                UserName = RegisterRequest.Username,
                Email = RegisterRequest.Email
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, RegisterRequest.Password);
            if (!result.Succeeded)
            {
                throw new MappedException(result.Errors.FirstOrDefault()?.Description);
            }

            return newUser.Id.ToString();
        }
    }
}