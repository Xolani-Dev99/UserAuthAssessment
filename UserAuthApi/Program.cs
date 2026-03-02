using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using UserAuthApi.Data;
using UserAuthApi.Interfaces;
using UserAuthApi.Models;
using UserAuthApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ================= SERILOG =================
// Centralized logging: console + file
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ================= DATABASE =================
// PostgreSQL (original, commented out for quick SQLite testing)
 builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// SQLite setup for quick local testing
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlite("Data Source=UserAuthTest.db"));

// ================= DEPENDENCY INJECTION =================
// Services follow SOLID principles: loose coupling, easily testable
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// ================= CORS =================
// Allow your React frontend to call the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// ================= JWT AUTH =================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// ================= AUTHORIZATION =================
builder.Services.AddAuthorization();

// ================= CONTROLLERS =================
builder.Services.AddControllers();

var app = builder.Build();

// ================= MIDDLEWARE ORDER =================
app.UseSerilogRequestLogging(); // optional, logs HTTP requests
//app.UseHttpsRedirection(); // redirect HTTP -> HTTPS
app.UseCors("AllowReactApp"); // enable CORS for frontend
app.UseAuthentication();     // JWT auth
app.UseAuthorization();      // role/policy checks (if needed)

app.MapControllers();

app.Run();