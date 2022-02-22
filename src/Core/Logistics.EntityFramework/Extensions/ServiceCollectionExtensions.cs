﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Logistics.Domain.Repositories;
using Logistics.EntityFramework.Data;
using Logistics.EntityFramework.Repositories;
using Logistics.EntityFramework.Helpers;

namespace Logistics.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Local");

        services.AddDbContext<DatabaseContext>(
            o => DbContextHelpers.ConfigureMySql(connectionString, o));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}