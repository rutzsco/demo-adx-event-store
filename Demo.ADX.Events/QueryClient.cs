using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Kusto.Data;
using System.Text;
using Azure.Identity;

namespace Demo.ADX.Events
{
    public static class QueryClient
    {
        private static ICslQueryProvider _queryProvider;

        static QueryClient()
        {
            var kustoConnectionStringBuilder = new KustoConnectionStringBuilder(AdxConfig.QueryUri)
                .WithAadAzureTokenCredentialsAuthentication(new DefaultAzureCredential());

            _queryProvider = KustoClientFactory.CreateCslQueryProvider(kustoConnectionStringBuilder);
        }

        public static void QueryLastLogs(int takeCount = 10)
        {
            string query = $"{AdxConfig.TableName} | order by Timestamp desc | take {takeCount}";
            var reader = _queryProvider.ExecuteQuery(AdxConfig.DatabaseName, query, null);

            Console.WriteLine("\n=== Last Log Entries ===");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Timestamp"]}, {reader["Level"]}, {reader["Message"]}");
            }
            reader.Close();
        }

        public static void QueryCountByLevel()
        {
            string query = $"{AdxConfig.TableName} | summarize Count=count() by Level";
            var reader = _queryProvider.ExecuteQuery(AdxConfig.DatabaseName, query, null);

            Console.WriteLine("\n=== Log Count by Level ===");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Level"]}: {reader["Count"]}");
            }
            reader.Close();
        }

        public static string SearchLogs(string keyword)
        {
            string query = $"{AdxConfig.TableName} | where Message contains \"{keyword}\"";
            var sb = new StringBuilder();
            var reader = _queryProvider.ExecuteQuery(AdxConfig.DatabaseName, query, null);

            Console.WriteLine($"\n=== Logs containing '{keyword}' ===");
            while (reader.Read())
            {
                sb.AppendLine($"{reader["Timestamp"]}, {reader["Level"]}, {reader["Message"]}");
            }
            reader.Close();
            return sb.ToString();
        }
    }

}
