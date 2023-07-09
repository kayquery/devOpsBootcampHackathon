using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

public static class ConfigureIdentity
{


    public static void ConsigureIdentityService(this IServiceCollection services, IConfiguration _configuration)
    {
        services
        .AddIdentity<MongoUser, MongoRole>( identity => 
        {
            identity.Password.RequireDigit = false;
            identity.Password.RequireNonAlphanumeric = false;
            identity.Password.RequireUppercase = false;
            identity.Password.RequiredLength = 8;
            identity.Password.RequireLowercase = false;
            
        })
        .AddMongoDbStores<MongoUser, MongoRole, ObjectId>(mongo =>
        {
            mongo.ConnectionString = _configuration.GetValue<string>("MongoDb:ConnectionString");
        })
        .AddDefaultTokenProviders();

    }

}
