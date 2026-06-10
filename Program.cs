using EventBookingAPI.Data;
using EventBookingAPI.Repositories;
using EventBookingAPI.Repositories.Interfaces;
using EventBookingAPI.Services;
using EventBookingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//LOGGING CONFIGURATION
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// DATABASE CONNECTION

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));


// CONTROLLERS

builder.Services.AddControllers();


// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// JWT AUTHENTICATION

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
        };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = 401;

            context.Response.ContentType =
                "application/json";

            return context.Response.WriteAsync(
                "{\"message\":\"Unauthorized Access\"}");
        }
    };
});


// AUTHORIZATION

builder.Services.AddAuthorization();


// REPOSITORIES

builder.Services.AddScoped<
    IAuthRepository,
    AuthRepository>();

builder.Services.AddScoped<
    IEventRepository,
    EventRepository>();

builder.Services.AddScoped<
    IBookingRepository,
    BookingRepository>();


// SERVICES

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IEventService,
    EventService>();

builder.Services.AddScoped<
    IBookingService,
    BookingService>();


var app = builder.Build();


// CORS

app.UseCors("AllowReactApp");


// AUTHENTICATION

app.UseAuthentication();


// AUTHORIZATION

app.UseAuthorization();


// MAP CONTROLLERS

app.MapControllers();


// RUN APPLICATION

app.Run();