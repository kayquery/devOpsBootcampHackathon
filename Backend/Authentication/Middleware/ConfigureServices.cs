using Loan.Authentication.Services;
using Loan.Authentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

}
