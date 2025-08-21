using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Domain.Repositories;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Infrastructure.Services;


namespace TaskManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database - PostgreSQL
            services.AddDbContext<TaskManagementDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(TaskManagementDbContext).Assembly.FullName)));


            // Repositories
            services.AddScoped<ITaskRepository, TaskRepository>();

            // Services
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            return services;
        }
    }
}
