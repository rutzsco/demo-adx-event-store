using System;
using System.Text;
using System.Text.Json;
using Azure.Identity;
using Kusto.Data;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;

namespace Demo.ADX.Events
{
    public static class NearRealTimeLogger
    {
        private static ICslAdminProvider _adminClient;

        static NearRealTimeLogger()
        {
            var kustoCSB = new KustoConnectionStringBuilder(AdxConfig.QueryUri)
                .WithAadAzureTokenCredentialsAuthentication(new DefaultAzureCredential());

            _adminClient = KustoClientFactory.CreateCslAdminProvider(kustoCSB);
        }

        public static void LogEventInline(string message)
        {
            var logRecord = new
            {
                Timestamp = DateTime.UtcNow,
                Level = "High",
                Message = message
            };
            var json = JsonSerializer.Serialize(logRecord);

            var ingestCommand = $@"
            .ingest inline into table {AdxConfig.TableName}
            with (format=json) 
            <| {json}";

            // Execute the control command
            var result = _adminClient.ExecuteControlCommand(AdxConfig.DatabaseName, ingestCommand.ToString());
        }

        // Simple method to escape CSV values (just an example; real-world logic may be more robust)
        private static string EscapeCsvValue(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"))
            {
                value = value.Replace("\"", "\"\"");      // Escape quotes by doubling
                value = $"\"{value}\"";                  // Wrap in quotes
            }
            return value;
        }
    }
}



