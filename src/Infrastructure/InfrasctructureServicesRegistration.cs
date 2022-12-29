using System.Text;
using Application.Contracts.Gateways;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Models;
using Application.Models.Identity;
using Infrastructure.Gateways.Mail;
using Infrastructure.Identity;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public static class InfrasctructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));


            services.AddDbContext<LeaveManagementDbContext>(opt =>
                opt.UseSqlite(
                    configuration.GetConnectionString("LeaveManagementConnectionString"),
                    b => b.MigrationsAssembly(typeof(LeaveManagementDbContext).Assembly.FullName)
                )
            );
            services.AddDbContext<LeaveManagementIdentityDbContext>(opt =>
                opt.UseSqlite(
                    configuration.GetConnectionString("LeaveManagementIdentityConnectionString"),
                    b => b.MigrationsAssembly(typeof(LeaveManagementIdentityDbContext).Assembly.FullName)
                )
            );

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
            services.AddTransient<IAuthService, AuthService>();

             services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<LeaveManagementIdentityDbContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                    };
                });

            return services;
        }
    }
}