using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.Data
{
    public class TaskManagementDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public TaskManagementDbContext(
            DbContextOptions<TaskManagementDbContext> options,
            IDomainEventDispatcher domainEventDispatcher) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public DbSet<TaskItem> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagementDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (property.Metadata.ClrType == typeof(DateTime?) && property.CurrentValue is DateTime dateTime)
                        {
                            property.CurrentValue = dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                        }
                        else if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue is DateTime dateTimeNotNull)
                        {
                            property.CurrentValue = dateTimeNotNull.Kind == DateTimeKind.Utc ? dateTimeNotNull : DateTime.SpecifyKind(dateTimeNotNull, DateTimeKind.Utc);
                        }
                    }
                }
            }

            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entitiesWithEvents
                .SelectMany(e => e.DomainEvents)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var domainEvent in domainEvents)
            {
                await _domainEventDispatcher.DispatchAsync(domainEvent, cancellationToken);
            }

            foreach (var entity in entitiesWithEvents)
            {
                entity.ClearDomainEvents();
            }

            return result;
        }
    }
}
