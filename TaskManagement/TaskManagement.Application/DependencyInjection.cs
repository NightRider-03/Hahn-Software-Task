using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor; 
using FluentValidation;
using TaskManagement.Application.Common;
namespace TaskManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.Scan(scan => scan
                .FromAssemblies(assembly) 
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
