using BeerRecipeAPI.Auth;
using BeerRecipeAPI.Filters;
using BeerRecipeAPI.Interfaces;
using BeerRecipeAPI.Repository;
using BeerRecipeAPI.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BeerRecipeAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region CORS

            builder.Services.AddCors(cors => cors.AddPolicy("AllowOriginAndMethod", options => options
            .WithOrigins(new[] { "https://localhost:5000" })));

            #endregion

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomLogFilter));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Informe o token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
            });

            #region Dependency Injection

            builder.Services.AddSingleton(typeof(IBeerRepository), typeof(BeerRepository));
            builder.Services.AddSingleton(typeof(IUserRepository), typeof(UserRepository));

            #endregion

            #region JWT Auth
            var firstAuthConfig = new AuthenticationInfo();
            new ConfigureFromConfigurationOptions<AuthenticationInfo>(builder.Configuration.GetSection("FirstAuthenticationInfo")).Configure(firstAuthConfig);
            builder.Services.AddSingleton(firstAuthConfig);

            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfiguration")).Configure(tokenConfiguration);
            builder.Services.AddSingleton(tokenConfiguration);
            var tokenService = new GenerateToken(tokenConfiguration);
            builder.Services.AddScoped(typeof(GenerateToken));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidIssuer = tokenConfiguration.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.Secret))
                };
            });

            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowOriginAndMethod");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #region GenerateUsers

            if (!File.Exists("users.json"))
            {
                var userGenerator = new UserGenerator();
                userGenerator.Generate();
            }

            #endregion

            app.Run();

        }
    }
}