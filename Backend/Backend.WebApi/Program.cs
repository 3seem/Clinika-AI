using System.Text;
using Backend.Application;
using Backend.Application.Common.Interfaces;
using Backend.Infrastructure;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

namespace Backend.WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Clean Architecture Dependencies
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);

        // 2. JWT Authentication Setup
        var jwtSecret = builder.Configuration["JwtSettings:Secret"]
            ?? throw new InvalidOperationException("JwtSettings:Secret is not configured.");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // 3. Role-Based Access Control (5 Ecosystem Roles)
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequirePatientRole", policy => policy.RequireRole("patient"));
            options.AddPolicy("RequireDoctorRole", policy => policy.RequireRole("doctor"));
            options.AddPolicy("RequireLabRole", policy => policy.RequireRole("lab_staff"));
            options.AddPolicy("RequirePharmacyRole", policy => policy.RequireRole("pharmacy_staff"));
            options.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("super_admin"));
        });

        // 4. CORS Policy for Frontend Portals (React Web & Flutter)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllPortals", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // 5. Automatic Database Seeding (Master Specialties & Initial Admin)
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
            await DatabaseSeeder.SeedAsync(dbContext, passwordHasher);
        }

        // 6. HTTP Pipeline Configuration
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Hospitalia AI — API Documentation")
                       .WithTheme(ScalarTheme.Moon);
            });
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAllPortals");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}
