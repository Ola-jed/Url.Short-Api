using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Url.Short_Api.Data;
using Url.Short_Api.Data.Profiles;

namespace Url.Short_Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCors(this IServiceCollection serviceCollection, string policyName)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddPolicy(policyName,
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Url Shortener API",
                Version = "v1",
                Description = "Application to shorten urls",
                Contact = new OpenApiContact
                {
                    Name = "Olabissi Gbangboche",
                    Email = "olabijed@gmail.com",
                    Url = new Uri("https://github.com/Ola-jed")
                }
            });
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Authorization header using the Bearer scheme. (\"Authorization: Bearer {token}\")",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { "Bearer" } }
            };
            c.AddSecurityRequirement(securityRequirement);
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }

    public static void AddAuth(this IServiceCollection serviceCollection, string key)
    {
        serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }

    public static void AddMapper(this IServiceCollection serviceCollection)
    {
        var mapperConfig = new MapperConfiguration(m =>
        {
            m.AddProfile<UrlShortenProfile>();
            m.AddProfile<UrlTypeProfile>();
        });
        serviceCollection.AddSingleton(mapperConfig.CreateMapper());
    }

    public static void AddPgsql(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
            Password = configuration["Password"],
            Username = configuration["UserId"],
            Host = configuration["Host"]
        };
        serviceCollection.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(builder.ConnectionString));
    }
}