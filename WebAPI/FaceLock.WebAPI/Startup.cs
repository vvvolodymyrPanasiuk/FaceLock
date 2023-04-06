using FaceLock.Authentication;
using FaceLock.Authentication.Repositories;
using FaceLock.Authentication.RepositoriesImplementations;
using FaceLock.Authentication.Services;
using FaceLock.Authentication.ServicesImplementations;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.EF;
using FaceLock.EF.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Text;


namespace FaceLock.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FaceLockDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("name=ConnectionStrings:DefaultConnection")));

            services.Configure<JwtTokenSettings>(Configuration.GetSection("JwtTokenSettings"));

            // Add logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Console()
                     .WriteTo.File("./logs/log.txt", rollingInterval: RollingInterval.Day)
                     .CreateLogger());
            });

            // Add identity
            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<FaceLockDbContext>()
                .AddDefaultTokenProviders();
                //.AddTokenProvider("MyApp", typeof(DataProtectorTokenProvider<User>));
            
            // Configure identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            // Add JWT authentication           
            var jwtTokenSettings = Configuration.GetSection("JwtTokenSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.GetValue<string>("SecretKey")));

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add authorization by roles
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole("admin"));
                options.AddPolicy("user", policy => policy.RequireRole("user"));
            });          

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            services.AddMemoryCache(options =>
            {
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(60);
                options.SizeLimit = 256;
            });
            // Add API controllers
            services.AddControllers();
            // Register the Swagger services
            services.AddSwaggerDocument();

            // Application services
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserFaceRepository, UserFaceRepository>();
            services.AddTransient<IVisitRepository, VisitRepository>();
            services.AddTransient<IPlaceRepository, PlaceRepository>();
            services.AddTransient<IBlacklistRepository, InDatabaseBlacklistRepository>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            // Add CORS handling
            app.UseCors(options =>
                options.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            //app.UseCors("AllowAll");
            //app.UseCors("AllowAll");
            
            // Add authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
