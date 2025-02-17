namespace app.infra
{
    /// <summary>
    /// Custom exception for secrets management.
    /// </summary>
    /// <param name="message">Message of the exception</param>
    public class SecretsException(string message) : Exception(message);

    /// <summary>
    /// Data structure that represents a secret from the Phase API.
    /// </summary>
    public struct PhaseSecret
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
    }

    /// <summary>
    /// Singleton class to manage secrets from the Phase API. It is responsible for fetching secrets only if the
    /// environment stage is not local.
    /// </summary>
    public class SecretsManager
    {
        private readonly static Lazy<SecretsManager> Instance = new Lazy<SecretsManager>(
            () => new SecretsManager()
        );
        private Dictionary<string, string>? _secrets;

        private SecretsManager() { }

        public static SecretsManager GetInstance => Instance.Value;

        /// <summary>
        /// Initialize the secrets manager by fetching secrets from the Phase API if the environment stage is not local.
        /// </summary>
        public void Initialize()
        {
            var config = Configuration.GetInstance;

            var env = config.Get("STAGE") switch
            {
                "dev" => "development",
                "staging" => "staging",
                "prod" => "production",
                _ => "local",
            };

            if (env == "local")
                return;

            CallPhaseApi(env);
        }

        /// <summary>
        /// Fetch secrets from the Phase API according to the environment stage.
        /// </summary>
        /// <param name="env">Current stage to fetch from Phase API</param>
        private void CallPhaseApi(string env)
        {
            var config = Configuration.GetInstance;

            var appId = config.Get("PHASE_APP_ID") ?? "";
            var apiKey = config.Get("PHASE_API_KEY") ?? "";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = httpClient
                .GetAsync($"https://api.phase.dev/v1/secrets/?app_id={appId}&env={env}")
                .Result;
            var data = response.Content.ReadFromJsonAsync<List<PhaseSecret>>().Result;

            _secrets =
                data == null
                    ? new Dictionary<string, string>()
                    : data.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Search for a secret by key and return its value. If the secret is not found, an exception is thrown.
        /// </summary>
        /// <param name="key">Name of the secret to get irs value</param>
        /// <returns>Value of te given secret</returns>
        /// <exception cref="SecretsException">Secret key not found</exception>
        public string MustGet(string key)
        {
            if (_secrets == null || !_secrets.TryGetValue(key, out var get))
            {
                throw new SecretsException($"Secret {key} not found");
            }

            return get;
        }

        /// <summary>
        /// Get a secret by key. If the secret is not found, return null.
        /// </summary>
        /// <param name="key">Name of the secret to get irs value</param>
        /// <returns>Value of te given secret</returns>
        public string? Get(string key)
        {
            return _secrets?.GetValueOrDefault(key);
        }
    }
}