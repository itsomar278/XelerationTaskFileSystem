using System;
using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Interfaces;
using XelerationTask.Infastructure.Persistence;
using XelerationTask.Infastructure.Persistence.UnitOfWorks;
using XelerationTask.Infastructure.Persistence.Repositories;
using XelerationTask.Application.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using XelerationTask.API.Filters;
using XelerationTask.Infastructure.Security;
using XelerationTask.Application.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace XelerationTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer eyJhbGciOi...etc"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
            });



            builder.Services.AddDbContext<FileSystemDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddTransient<IFileRepository, FileRepository>();

            builder.Services.AddTransient<IFolderRepository, FolderRepository>();

            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

            builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();

            builder.Services.AddTransient<IAuthService, AuthService>();

            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IFolderService, FolderService>();

            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<FileSystemExceptionFilter>();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<FileSystemExceptionFilter>();
            });

            builder.Services.AddAuthentication("Bearer")
                     .AddJwtBearer("Bearer", options =>
                     {
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,
                             ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                             ValidAudience = builder.Configuration["JwtSettings:Audience"],
                             ClockSkew = TimeSpan.Zero,
                             IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])
                 )

                         };
                     });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin() 
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);
                    result.ContentTypes.Add("application/json");
                    return result;
                };
            });



            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}