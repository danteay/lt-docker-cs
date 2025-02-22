using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace app.infra
{
    public class Logger
    {
        private readonly static Lazy<Logger> Instance = new Lazy<Logger>(
            () => new Logger()
        );
        
        public static Logger GetInstance => Instance.Value;

        private Logger() { }

        public void Initialize()
        {
            var level = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "INFO";
            var secretName = Environment.GetEnvironmentVariable("LOGSTASH_CONFIGS") ?? "";
            
            var minLevel = level switch
            {
                "DEBUG" => LogEventLevel.Debug,
                "ERROR" => LogEventLevel.Error,
                "WARNING" => LogEventLevel.Warning,
                _ => LogEventLevel.Information
            };

            var newLogger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(minLevel)
                .WriteTo.Console(new CompactJsonFormatter());
            
            if (Configuration.GetInstance.Get("STAGE") != "local")
            {
                Console.WriteLine("Running logstash on docker, logging to logstash:5000");
                // Execution inside container
                var secrets = SecretsManager.GetInstance(secretName);
                var logstashHost = secrets.MustGet("HOST");
                newLogger.WriteTo.Http(requestUri: logstashHost, queueLimitBytes: null);
            }
            else
            {
                Console.WriteLine("Running logstash locally, logging to localhost:5003");
                // Execution locally using dotnet run
                newLogger.WriteTo.Http(requestUri: "http://localhost:5003", queueLimitBytes: null);
            }
            
            Log.Logger = newLogger.CreateLogger();
        }
    }
}