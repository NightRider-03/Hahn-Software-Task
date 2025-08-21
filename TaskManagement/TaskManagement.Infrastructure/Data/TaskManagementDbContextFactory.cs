using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.Data
{
    public class TaskManagementDbContextFactory : IDesignTimeDbContextFactory<TaskManagementDbContext>
    {
        public TaskManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskManagementDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TaskManagement.API"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            var dummyDispatcher = new DummyDomainEventDispatcher();

            return new TaskManagementDbContext(optionsBuilder.Options, dummyDispatcher);
        }
    }

    public class DummyDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(TaskManagement.Domain.Common.IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
