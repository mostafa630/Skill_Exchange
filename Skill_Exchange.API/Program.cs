using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Skill_Exchange.API;
using Skill_Exchange.API.Hubs;
using Skill_Exchange.Application.FluentValidation.Auth;
using Skill_Exchange.Application.Interfaces;
using Skill_Exchange.Application.Services;
using Skill_Exchange.Application.Services.GlobalQuery.Handlers;
using Skill_Exchange.Domain.Entities;
using Skill_Exchange.Domain.Interfaces;
using Skill_Exchange.Infrastructure;
using Skill_Exchange.Infrastructure.AuthenticationServices;
using Skill_Exchange.Infrastructure.Configurations;
using Skill_Exchange.Infrastructure.Peresistence;
using Skill_Exchange.Infrastructure.Repositories;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// ---------------------- Controllers ----------------------
builder.Services.AddControllers();

// ---------------------- Swagger ----------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Include XML documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    // ===== Add JWT Bearer support =====
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token. Example: 'Bearer eyJhbGci...'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});



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

//---------------------------- serialzie Enums to its String values -----------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


//-----------------------------Identity-------------------

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
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

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
// ---------------------- SignalR ----------------------
builder.Services.AddSignalR();
// ---------------------- Message ----------------------
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtOptions.Issuer,
        ValidAudience = JwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(JwtOptions.SigningKey))
    };
});

// ==================== Add CORS ====================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});


builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>();
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

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseWebSockets();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");
app.Run();


