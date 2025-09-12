using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Skill_Exchange.API;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Application.Services.GlobalQuery;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure;
using Skill_Exchange.Infrastructure.Configurations;
using Skill_Exchange.Infrastructure.Peresistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------------------- EF Core ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ---------------------- MongoDB ----------------------
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>();
    return new MongoDbContext(settings);
});

// ---------------------- UnitOfWork ----------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------------------- AutoMapper ----------------------
builder.Services.AddAutoMapper(typeof(Skill_Exchange.Application.Mapping.UserProfile).Assembly);

// ---------------------- MediatR ----------------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllHandler<,>).Assembly);
});

builder.Services.AddMediatorHandlers();
// ---------------------- Build App ----------------------
var app = builder.Build();

// ---------------------- Seed MongoDB ----------------------
using (var scope = app.Services.CreateScope())
{
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    var seeder = new MongoSeeder(mongoContext);
    seeder.SeedMessages();
}

// ---------------------- Middleware ----------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
