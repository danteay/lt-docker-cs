namespace app.infra
{
    /// <summary>
    /// Struct that contains the default environment variables for the application.
    /// </summary>
    public struct DefaultEnvs
    {
        public static string Stage => Environment.GetEnvironmentVariable("STAGE") ?? "local";
        public static string ServiceName => "lt-docker-dotnet";
        public static string Port => "5001";

        /// <summary>
        /// Converts the default environment variables to a dictionary for easy loading into the configuration.
        /// </summary>
        /// <returns>Dictionary with the default environment variables</returns>
        public static Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                { "SERVICE_NAME", ServiceName },
                { "PORT", Port },
                { "STAGE", Stage },
            };
        }
    }
}
