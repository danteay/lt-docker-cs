using app.domains.products;
using app.domains.users;
using Microsoft.EntityFrameworkCore;

namespace app.infra
{
    /// <summary>
    /// Class that represents the Postgres database context. It is responsible for managing the database
    /// connections and the entities that will be stored in the database.
    /// </summary>
    public class PostgresDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        private readonly string _secretName;

        public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        {
            _secretName =
                Configuration.GetInstance.Get("POSTGRES_SECRET_NAME")
                ?? throw new SecretsException(
                    "POSTGRES_SECRET_NAME environment variable is required"
                );

            SecretsManager.GetInstance(_secretName).Initialize();
        }

        /// <summary>
        /// Register domain classes as tables in the database.
        /// </summary>
        /// <param name="modelBuilder">Entity framework model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Product>().ToTable("products");
        }

        /// <summary>
        /// Configure the database connection string based on the environment variables.
        /// </summary>
        /// <param name="optionsBuilder">Context options for the database connection</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Default connection string for local development
            var connectionString = "Host=localhost;Database=postgres;Username=root;Password=root";

            var secrets = SecretsManager.GetInstance(_secretName);

            if (Configuration.GetInstance.Get("STAGE") != "local")
            {
                var user = secrets.MustGet("POSTGRES_USER");
                var password = secrets.MustGet("POSTGRES_PASSWORD");
                var host = secrets.MustGet("POSTGRES_HOST");
                var database = secrets.MustGet("POSTGRES_DB");
                var port = secrets.MustGet("POSTGRES_PORT");

                connectionString =
                    $"Host={host};Database={database};Username={user};Password={password}";
                
                if (!string.IsNullOrEmpty(port)) connectionString += $";Port={port}";
            }

            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
