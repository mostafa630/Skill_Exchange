using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Skill_Exchange.Infrastructure.Peresistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDbConnection");
    var db_name = builder.Configuration["MongoDbSettings:DatabaseName"];
    return new MongoDbContext(mongoConnectionString, db_name);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
