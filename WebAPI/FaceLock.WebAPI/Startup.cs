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
using FaceLock.Recognition.RecognitionSettings;
using FaceLock.Recognition.Services;
using FaceLock.Recognition.ServicesImplementations.EmguCVImplementation;
using FaceLock.WebAPI.GrpcClientFactory;
using FaceLock.WebAPI.GrpcClientFactory.GrpcClientFactoryImplementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace FaceLock.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            
            Configuration = configuration;
        }

        public Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureAppConfiguration();
            services.AddFeatureManagement();

            string connectionString;
            //Ckeck environment of project
            if (_env.EnvironmentName == "Docker")
            {
                connectionString = 
                    $"Server={Environment.GetEnvironmentVariable("DatabaseServer")}, " +
                    $"{Environment.GetEnvironmentVariable("DatabasePort")}; " +
                    $"Initial Catalog={Environment.GetEnvironmentVariable("DatabaseName")}; " +
                    $"User ID={Environment.GetEnvironmentVariable("DatabaseUser")}; " +
                    $"Password={Environment.GetEnvironmentVariable("DatabasePassword")};" +
                    $"TrustServerCertificate={true};";

                ConfigureServicesDevelopment(services, connectionString);
            }
            if (_env.IsDevelopment())
            {
                connectionString = Configuration.GetConnectionString("DefaultConnection");
                ConfigureServicesDevelopment(services, connectionString);            
            }
            if(_env.IsProduction())
            {
                ConfigureServicesProduction(services);                   
            }

            services.Configure<JwtTokenSettings>(Configuration.GetSection("JwtTokenSettings"));
            services.Configure<EmguCVFaceRecognationSettings>(Configuration.GetSection("EmguCVFaceRecognationSettings"));

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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtTokenSecretKey"]));
            
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
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
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
            services.AddTransient<ISecretKeyGeneratorService, SecureRandomSecretKeyGeneratorStrategy>();
            services.AddTransient<ITokenGeneratorService, JwtTokenGeneratorService>();
            services.AddScoped<ICommandUserService, DataManagement.ServicesImplementations.CommandImplementations.UserService>();
            services.AddScoped<IQueryUserService, DataManagement.ServicesImplementations.QueryImplementations.UserService>();
            services.AddScoped<ICommandPlaceService, DataManagement.ServicesImplementations.CommandImplementations.PlaceService>();
            services.AddScoped<IQueryPlaceService, DataManagement.ServicesImplementations.QueryImplementations.PlaceService>();
            services.AddScoped<ICommandDoorLockService, DataManagement.ServicesImplementations.CommandImplementations.DoorLockService>();
            services.AddScoped<IQueryDoorLockService, DataManagement.ServicesImplementations.QueryImplementations.DoorLockService>();
            services.AddTransient<IDataServiceFactory, DataServiceFactory>();

            services.AddScoped<ITokenStateRepository, InFileTokenStateRepository>();
            services.AddScoped<IBlacklistRepository, InFileBlacklistRepository>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddTransient<IFaceRecognitionService<string>, EmguFaceRecognitionService<string>>();
        }

        private void ConfigureServicesProduction(IServiceCollection services)
        {
            string connection = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            string dbhost = System.Text.RegularExpressions.Regex.Match(connection, @"Data Source=(.+?);").Groups[1].Value;
            string server = dbhost.Split(':')[0].ToString();
            string port = dbhost.Split(':')[1].ToString();
            string dbname = System.Text.RegularExpressions.Regex.Match(connection, @"Database=(.+?);").Groups[1].Value;
            string dbusername = System.Text.RegularExpressions.Regex.Match(connection, @"User Id=(.+?);").Groups[1].Value;
            string dbpassword = System.Text.RegularExpressions.Regex.Match(connection, @"Password=(.+?)$").Groups[1].Value;
            string connectionString = $@"server={server};userid={dbusername};password={dbpassword};database={dbname};port={port};pooling = false; convert zero datetime=True;";

            services.AddDbContext<EF.MySql.FaceLockMySqlDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 1))));

            // Add identity
            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<EF.MySql.FaceLockMySqlDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserRepository, EF.MySql.Repositories.UserRepository.UserRepository>();
            services.AddScoped<IUserFaceRepository, EF.MySql.Repositories.UserRepository.UserFaceRepository>();
            services.AddScoped<IVisitRepository, EF.MySql.Repositories.PlaceRepository.VisitRepository>();
            services.AddScoped<IPlaceRepository, EF.MySql.Repositories.PlaceRepository.PlaceRepository>();
            services.AddScoped<IDoorLockAccessRepository, EF.MySql.Repositories.DoorLockRepository.DoorLockAccessRepository>();
            services.AddScoped<IDoorLockSecurityInfoRepository, EF.MySql.Repositories.DoorLockRepository.DoorLockSecurityInfoRepository>();
            services.AddScoped<IDoorLockHistoryRepository, EF.MySql.Repositories.DoorLockRepository.DoorLockHistoryRepository>();
            services.AddScoped<IDoorLockRepository, EF.MySql.Repositories.DoorLockRepository.DoorLockRepository>();
            services.AddScoped<IUnitOfWork, EF.MySql.Repositories.UnitOfWork>();

            services.AddTransient<IGrpcClientChannelFactory>(_ => new GrpcClientFactory.GrpcClientFactoryImplementations.GrpcClientChannelFactory(Configuration["GrpcServerAddress"]));
        }

        private void ConfigureServicesDevelopment(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<EF.FaceLockDbContext>(options =>
                    options.UseSqlServer(connectionString));

            // Add identity
            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<EF.FaceLockDbContext>()
                .AddDefaultTokenProviders();

            // Application services
            services.AddScoped<IUserRepository, EF.Repositories.UserRepository.UserRepository>();
            services.AddScoped<IUserFaceRepository, EF.Repositories.UserRepository.UserFaceRepository>();
            services.AddScoped<IVisitRepository, EF.Repositories.PlaceRepository.VisitRepository>();
            services.AddScoped<IPlaceRepository, EF.Repositories.PlaceRepository.PlaceRepository>();
            services.AddScoped<IDoorLockAccessRepository, EF.Repositories.DoorLockRepository.DoorLockAccessRepository>();
            services.AddScoped<IDoorLockSecurityInfoRepository, EF.Repositories.DoorLockRepository.DoorLockSecurityInfoRepository>();
            services.AddScoped<IDoorLockHistoryRepository, EF.Repositories.DoorLockRepository.DoorLockHistoryRepository>();
            services.AddScoped<IDoorLockRepository, EF.Repositories.DoorLockRepository.DoorLockRepository>();
            services.AddScoped<IUnitOfWork, EF.Repositories.UnitOfWork>();

            services.AddTransient<IGrpcClientChannelFactory>(_ => new GrpcClientFactory.GrpcClientFactoryImplementations.GrpcClientChannelFactory(Configuration["GrpcServerAddress"]));  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)      
        {
            if(env.EnvironmentName == "Docker")
            {
                InitDockerDatabase.Init(app);
            }
            if (env.IsProduction())
            {                  
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
             
                    var dbContext = services.GetRequiredService<EF.MySql.FaceLockMySqlDbContext>();
                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAzureAppConfiguration();

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
            app.UseCors();                     

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
