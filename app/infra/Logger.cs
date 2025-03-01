using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace app.infra
{
    public class Logger
    {
        private static readonly Lazy<Logger> Instance = new Lazy<Logger>(() => new Logger());

        public static Logger GetInstance => Instance.Value;

        private Logger() { }

        public void Initialize()
        {
            var level = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "INFO";
            var secretName = Environment.GetEnvironmentVariable("LOGSTASH_CONFIGS") ?? "";
            var stage = Configuration.GetInstance.Get("STAGE");

            var minLevel = level switch
            {
                "DEBUG" => LogEventLevel.Debug,
                "ERROR" => LogEventLevel.Error,
                "WARNING" => LogEventLevel.Warning,
                _ => LogEventLevel.Information,
            };

            var formatter = new ElasticsearchJsonFormatter(
                renderMessageTemplate: false,
                inlineFields: true
            );

            var newLogger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("environment", stage)
                .Enrich.WithProperty("serviceName", Configuration.GetInstance.Get("SERVICE_NAME"))
                .MinimumLevel.Is(minLevel)
                .WriteTo.Console(formatter);

            if (Configuration.GetInstance.Get("STAGE") != "local")
            {
                // Execution inside container
                var secrets = SecretsManager.GetInstance(secretName);
                secrets.Initialize();

                var elasticHost = secrets.MustGet("ELASTIC_HOST");
                var elasticUser = secrets.MustGet("ELASTIC_USER");
                var elasticPassword = secrets.MustGet("ELASTIC_PASSWORD");

                newLogger.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticHost))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = $"logs-{DateTime.UtcNow:yyyy.MM.dd}",
                        ModifyConnectionSettings = x =>
                            x.BasicAuthentication(elasticUser, elasticPassword),
                    }
                );
            }

            Log.Logger = newLogger.CreateLogger();
        }
    }
}
