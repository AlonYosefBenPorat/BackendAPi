using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiWebApp.Auth;
using ApiWebApp.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using WebApp.DAL.Model;
using DAL.Repositories;
using ApiWebApp.DAL.Model;
using DAL.Data;
using DAL.Models;
using NETCore.MailKit.Infrastructure.Internal;
using NETCore.MailKit.Extensions;
using ApiWebApp.Services;
using ApiWebApp.Services.Interfaces;

namespace ApiWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<WebAppContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebAppContext") ?? throw new InvalidOperationException("Connection string 'WebAppContext' not found."),
                b => b.MigrationsAssembly("ApiWebApp"))); // Specify the migrations assembly

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Bind JWT settings from appsettings.json
            var jwtSettings = new JwtSettings();
            builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            // Add Identity services
            builder.Services.AddIdentity<AppUsers, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<WebAppContext>()
            .AddDefaultTokenProviders();

            // Register TokenService
            builder.Services.AddScoped<TokenService>();

            // Register RoleManager<AppRole> and IRoleStore<AppRole>
            builder.Services.AddScoped<RoleManager<AppRole>>();
            builder.Services.AddScoped<IRoleStore<AppRole>, RoleStore<AppRole, WebAppContext>>();

            // Register the repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRepository<Customer>, Repository<Customer>>();
            builder.Services.AddScoped<IRepository<Server>, Repository<Server>>();
            builder.Services.AddScoped<IRepository<Backup>, Repository<Backup>>();
            builder.Services.AddScoped<IRepository<NetworkDevice>, Repository<NetworkDevice>>();
            builder.Services.AddScoped<IRepository<Firewall>, Repository<Firewall>>();
            builder.Services.AddScoped<IRepository<Asset>, Repository<Asset>>();

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            // Add services to the container.
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use the CORS policy globally
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                if (context.User?.Identity?.IsAuthenticated != true)
                {
                    logger.LogWarning("User is not authenticated.");
                }
                else
                {
                    logger.LogInformation("User is authenticated.");
                }

                await next.Invoke();
            });

            app.MapControllers();

            // Seed roles and other data
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<WebAppContext>();
                    context.Database.Migrate(); // Apply any pending migrations
                    await SeedData.Initialize(services); // Seed the database
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            app.Run();
        }
    }
}
