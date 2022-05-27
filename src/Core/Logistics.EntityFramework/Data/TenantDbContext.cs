﻿using Logistics.EntityFramework.Helpers;
using Logistics.Domain.Services;
using Microsoft.EntityFrameworkCore.Design;

namespace Logistics.EntityFramework.Data;

public class TenantDbContext : DbContext
{
    private readonly ITenantService _tenantService;
    private readonly string _connectionString;

    public TenantDbContext(string connectionString)
    {
        _connectionString = connectionString;
        _tenantService = null!;
    }

    public TenantDbContext(
        DbContextOptions options, 
        ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
        _connectionString = string.Empty;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

        if (!options.IsConfigured)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                DbContextHelpers.ConfigureMySql(_connectionString, options);
            }
            else
            {
                DbContextHelpers.ConfigureMySql(_tenantService.GetConnectionString(), options);
            }  
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasMany(m => m.DispatcherCargoes)
                .WithOne(m => m.AssignedDispatcher)
                .HasForeignKey(m => m.AssignedDispatcherId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Truck>(entity =>
        {
            entity.ToTable("trucks");

            entity.HasOne(m => m.Driver)
                .WithOne()
                .HasForeignKey<Truck>(m => m.DriverId);

            entity.HasMany(m => m.Cargoes)
                .WithOne(m => m.AssignedTruck)
                .HasForeignKey(m => m.AssignedTruckId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Cargo>(entity =>
        {
            entity.ToTable("cargoes");
        });
    }
}
