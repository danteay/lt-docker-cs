using Microsoft.Extensions.Configuration.Yaml;

namespace app.infra
{
    /// <summary>
    /// Singleton class to manage configuration settings
    /// </summary>
    public class Configuration
    {
        private static readonly Lazy<Configuration> Instance = new Lazy<Configuration>(
            () => new Configuration()
        );
        private IConfigurationRoot? _config;

        private Configuration() { }

        public static Configuration GetInstance => Instance.Value;

        /// <summary>
        /// From the loaded configuration, returns a dictionary with all the key-value pairs
        /// </summary>
        /// <returns>Dictionary with all the key-value pairs</returns>
        public Dictionary<string, string> ToDictionary()
        {
            return _config?.AsEnumerable().ToDictionary(x => x.Key, x => x.Value ?? "")
                ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Obtain a configuration value by key
        /// </summary>
        /// <param name="key">Name of the configuration to search</param>
        /// <returns>String value or null of the configuration</returns>
        public string? Get(string key)
        {
            return _config?[key];
        }

        /// <summary>
        /// Initialize the configuration by loading the default environment variables and the YAML files following
        /// the 12-factor app methodology.
        ///
        /// This follows the precedence order:
        ///
        /// - Environment variables
        /// - YAML files
        /// - Default values
        /// </summary>
        public void Initialize()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddInMemoryCollection(DefaultEnvs.ToDictionary()!)
                .AddYamlFile(
                    $"config/{DefaultEnvs.Stage}.yml",
                    optional: false,
                    reloadOnChange: false
                )
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine($"NAME env: {Environment.GetEnvironmentVariable("NAME")}");
        }
    }
}
