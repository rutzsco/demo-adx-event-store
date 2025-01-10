namespace Demo.ADX.Events
{
    public static class AdxConfig
    {
        public static string IngestUri { get; private set; }
        public static string QueryUri { get; private set; }
        public static string DatabaseName { get; private set; }
        public static string TableName { get; private set; }
        public static string EventHubConnectionString { get; private set; }
        public static string EventHubName { get; private set; }

        // Utility method to load from configuration
        public static void LoadFromConfiguration(IConfiguration config)
        {
            IngestUri = config["IngestUri"];
            QueryUri = config["QueryUri"];
            DatabaseName = config["DatabaseName"];
            TableName = config["TableName"];
            EventHubConnectionString = config["EventHubConnectionString"];
            EventHubName = config["EventHubName"];
        }
    }

}