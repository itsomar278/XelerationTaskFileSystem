using System;
using Microsoft.EntityFrameworkCore;
using XelerationTask.Core.Interfaces;
using XelerationTask.Infastructure.Persistence;
using XelerationTask.Infastructure.Persistence.IUnitOfWorks;
using XelerationTask.Infastructure.Persistence.Repositories;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FileSystemDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddTransient<IFileRepository , FileRepository>();

            builder.Services.AddTransient<IFolderRepository , FolderRepository>();

            builder.Services.AddTransient<IUnitOfWork , UnitOfWork>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
