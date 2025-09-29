using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Skill_Exchange.API;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure;
using Skill_Exchange.Infrastructure.Configurations;
using Skill_Exchange.Infrastructure.Peresistence;
using Skill_Exchange.Infrastructure.AuthenticationServices;
using Skill_Exchange.Application.Interfaces;
using FluentValidation.AspNetCore;
using Skill_Exchange.Application.FluentValidation.Auth;
using Microsoft.AspNetCore.Identity;
using Skill_Exchange.Domain.Entities;



var builder = WebApplication.CreateBuilder(args);

// ---------------------- Controllers ----------------------
builder.Services.AddControllers();

// ---------------------- Swagger ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//----------------------- JWT Authentication -----------
var JwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
builder.Services.AddSingleton(JwtOptions);
builder.Services.AddScoped<IJwtService, JwtService>();
//----------------------- SMTP Settings ----------------
var SmtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddSingleton(SmtpSettings);
//--------------------- Email Services ----------------
builder.Services.AddScoped<IEmailService, EmailService>();
//--------------------- Authentication Services --------
builder.Services.AddScoped<IAuthService, AuthService>();
// ---------------------- EF Core ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ---------------------- MongoDB ------------------------
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);

builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>();
    return new MongoDbContext(settings);
});

// ---------------------- UnitOfWork ---------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------------------- AutoMapper ---------------------
builder.Services.AddAutoMapper(typeof(Skill_Exchange.Application.Mapping.UserProfile).Assembly);

// ---------------------- MediatR -------------------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllHandler<,>).Assembly);
});

builder.Services.AddMediatorHandlers();

// ==================== Add CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// ==================== Add Controllers & FluentValidation ====================
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
        fv.DisableDataAnnotationsValidation = true; // optional: disable default attributes
    });

// ---------------------- Build App ----------------------
var app = builder.Build();

// ---------------------- Seed MongoDB -------------------
using (var scope = app.Services.CreateScope())
{
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    var seeder = new MongoSeeder(mongoContext);
    seeder.SeedMessages();
}

// ---------------------- Middleware ---------------------
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Skill Exchange API V1");
        options.RoutePrefix = string.Empty; // Swagger UI at root
    });
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
