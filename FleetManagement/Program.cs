using FleetManagement.Application.FleetContextSeed;
using FleetManagement.Application.Repositories;
using FleetManagement.Application.Services.DeliveryPoint;
using FleetManagement.Application.Services.DeliveryPoint.DeliveryPointProvider;
using FleetManagement.Application.Services.FleetManagementService;
using FleetManagement.Application.Services.FleetManagementService.Implementation;
using FleetManagement.Domain.ShipmentAggregate;
using FleetManagement.Extensions;
using FleetManagement.Infrastructure;
using FleetManagement.Infrastructure.Repositories.Base;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Web;
using System;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddMediatR(Assembly.Load("FleetManagement.Application"));

builder.Services.AddDbContext<FleetDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        configure.MigrationsAssembly("FleetManagement.Infrastructure");
    });
});

builder.Services.AddScoped<DeliveryPointProvider>();
builder.Services.AddScoped<Branch>();
builder.Services.AddScoped<TransferCenter>();
builder.Services.AddScoped<DistributionCenter>();
builder.Services.AddScoped<FleetContextSeed>();
builder.Services.AddScoped<IFleetManagementService, FleetManagementService>();
builder.Services.AddScoped(typeof(IRepository<Shipment>), typeof(ShipmentRepository));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
                .AddSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    name: "Fleet Management Sql Server Database")
                .AddElasticsearch(
                    builder.Configuration.GetValue<string>("ElasticSearchUrl"),
                    "Fleet Management ElasticSearch Log Pool");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (c, r) =>
    {
        c.Response.ContentType = "application/json";

        var result = JsonConvert.SerializeObject(new
        {
            status = r.Status.ToString(),
            totalDuration = r.TotalDuration.ToString(),
            components = r.Entries.Select(e => new { name = e.Key, status = e.Value.Status.ToString(), duration = e.Value.Duration }),
        });
        await c.Response.WriteAsync(result);
    }
});

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MigrateDatabase(app.Services.GetRequiredService<ILogger<Program>>(), builder.Configuration);

app.Run();
