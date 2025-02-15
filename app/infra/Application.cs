using app.domains.products.repository;
using app.domains.products.service;
using Microsoft.OpenApi.Models;
using System.Reflection;
using app.domains.users.service;
using app.domains.users.repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace app.infra
{
    /// <summary>
    /// Class that represents the application itself. It is responsible for setting up the application
    /// and it's dependencies, such as configurations, services, and middlewares.
    ///
    /// The application follows the 12-factor app methodology by loading configurations from environment,
    /// and it's detached from the Properties/launchSettings.json file giving full control over the configuration
    /// to the code.
    /// </summary>
    public class Application
    {
        private readonly WebApplicationBuilder _builder;

        private Application(string[] args)
        {
            // Basic WebApplicationBuilder (minimal defaults)
            _builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                // We'll set the EnvironmentName later, after we read configs
                ContentRootPath = Directory.GetCurrentDirectory(),
                Args = args
            });
        }

        /// <summary>
        /// Loads default environment variables and configurations from YAML files along with system environment
        /// variables. This configuration follows the 12-factor app methodology by maintaining configurations
        /// precedence in the following order:
        ///
        /// - Environment variables
        /// - YAML files
        /// - Default values
        /// </summary>
        private void LoadConfigurations()
        {
            Configuration.GetInstance.Initialize();

            _builder.Configuration.Sources.Clear();
            _builder.Configuration.AddInMemoryCollection(Configuration.GetInstance.ToDictionary()!);
        }

        /// <summary>
        /// Configure the WebHost with the corresponding port from the loaded configuration.
        /// </summary>
        private void ConfigureWebHost()
        {
            // Now that config is loaded, read the port from config
            var appPort = _builder.Configuration["PORT"];
            Console.WriteLine($"Server will run on port {appPort}");

            // UseUrls after config is loaded
            _builder.WebHost.UseUrls($"http://0.0.0.0:{appPort}");
        }

        /// <summary>
        /// Attach services and middlewares to the WebHost.
        /// </summary>
        private void ConfigureServices()
        {
            _builder.Services.AddControllers();
            _builder.Services.AddEndpointsApiExplorer();
        }

        /// <summary>
        /// Configure swagger if the application is not running in production.
        /// </summary>
        private void ConfigureSwagger()
        {
            if (_builder.Configuration["STAGE"] == "prod") return;

            _builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Load the DbContexts for the application.
        /// </summary>
        private void ConfigureDbContext()
        {
            _builder.Services.AddDbContext<PostgresDbContext>();
        }

        /// <summary>
        /// Register services and repositories to be injected in the application.
        /// </summary>
        private void LoadInjections()
        {
            _builder.Services.AddScoped<IUserRepository, UserRepository>();
            _builder.Services.AddScoped<IUserService, UserService>();
            _builder.Services.AddScoped<IProductRepository, ProductRepository>();
            _builder.Services.AddScoped<IProductService, ProductService>();
        }

        /// <summary>
        /// Build the application and run it.
        /// </summary>
        private void Run()
        {
            var app = _builder.Build();

            // Configure the HTTP request pipeline.
            if (_builder.Configuration["STAGE"] != "prod")
            {
                Console.WriteLine("Enabling Swagger UI...");
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Entry point for the application. It builds the application and runs it.
        /// </summary>
        /// <param name="args">Program arguments</param>
        public static void Build(string[] args)
        {
            var appBuilder = new Application(args);

            // 1. Load config first
            appBuilder.LoadConfigurations();

            // 2. Now decide how the WebHost is configured (ports, etc.)
            appBuilder.ConfigureWebHost();

            // 3. Add rest of the services/middleware
            appBuilder.ConfigureServices();
            appBuilder.ConfigureSwagger();
            appBuilder.ConfigureDbContext();
            appBuilder.LoadInjections();

            // 4. Finally run
            appBuilder.Run();
        }
    }
}
