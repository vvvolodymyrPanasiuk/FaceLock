using FaceLock.Authentication;
using FaceLock.Authentication.Repositories;
using FaceLock.Authentication.RepositoriesImplementations.BlacklistRepositoryImplementations;
using FaceLock.Authentication.RepositoriesImplementations.TokenStateRepositoryImplementations;
using FaceLock.Authentication.Services;
using FaceLock.Authentication.ServicesImplementations;
using FaceLock.DataManagement.Services;
using FaceLock.DataManagement.Services.Commands;
using FaceLock.DataManagement.Services.Queries;
using FaceLock.DataManagement.ServicesImplementations;
using FaceLock.DataManagement.ServicesImplementations.TokenGeneratorImplementation;
using FaceLock.Domain.Entities.UserAggregate;
using FaceLock.Domain.Repositories;
using FaceLock.Domain.Repositories.DoorLockRepository;
using FaceLock.Domain.Repositories.PlaceRepository;
using FaceLock.Domain.Repositories.UserRepository;
using FaceLock.EF;
using FaceLock.EF.Repositories;
using FaceLock.EF.Repositories.DoorLockRepository;
using FaceLock.EF.Repositories.PlaceRepository;
using FaceLock.EF.Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.IO;
using System.Reflection;
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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
                    ValidateLifetime = true,
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

            // Register the Swagger services
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "FaceLockWebAPI",
                    Version = "v1",
                    Description = "More information will follow later"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);

                config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // Add API controllers
            services.AddControllers();           

            // Application services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserFaceRepository, UserFaceRepository>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IDoorLockAccessRepository, DoorLockAccessRepository>();
            services.AddScoped<IDoorLockAccessTokenRepository, DoorLockAccessTokenRepository>();
            services.AddScoped<IDoorLockHistoryRepository, DoorLockHistoryRepository>();
            services.AddScoped<IDoorLockRepository, DoorLockRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ITokenGeneratorService, SecureRandomTokenGeneratorStrategy>();
            services.AddScoped<ICommandUserService, DataManagement.ServicesImplementations.CommandImplementations.UserService>();
            services.AddScoped<IQueryUserService, DataManagement.ServicesImplementations.QueryImplementations.UserService>();
            services.AddScoped<ICommandPlaceService, DataManagement.ServicesImplementations.CommandImplementations.PlaceService>();
            services.AddScoped<IQueryPlaceService, DataManagement.ServicesImplementations.QueryImplementations.PlaceService>();
            services.AddScoped<ICommandDoorLockService, DataManagement.ServicesImplementations.CommandImplementations.DoorLockService>();
            services.AddScoped<IQueryDoorLockService, DataManagement.ServicesImplementations.QueryImplementations.DoorLockService>();
            services.AddTransient<IDataServiceFactory, DataServiceFactory>();

            services.AddScoped<ITokenStateRepository, InDatabaseTokenStateRepository>();
            services.AddScoped<IBlacklistRepository, InDatabaseBlacklistRepository>();
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
            // Register the Swagger generator and the Swagger UI middlewares
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.RoutePrefix = "swagger";
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "FaceLockWebAPI");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

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
