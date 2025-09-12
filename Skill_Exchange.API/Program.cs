using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Skill_Exchange.Infrastructure.Configurations;
using Skill_Exchange.Infrastructure.Peresistence;
using Microsoft.Extensions.DependencyInjection;
using Skill_Exchange.Application.Services.User.Queries.Handlers;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Register the MongoDbSettings class
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register the MongoDbContext as a singleton
builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>();
    return new MongoDbContext(settings);
});

// Inject UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllUsersHandler).Assembly));
var app = builder.Build();

// Seed the MongoDB database
using (var scope = app.Services.CreateScope())
{
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    var seeder = new MongoSeeder(mongoContext);
    seeder.SeedMessages();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
