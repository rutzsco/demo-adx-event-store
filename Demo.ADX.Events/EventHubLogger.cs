using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;

namespace Demo.ADX.Events
{
    public class EventHubLogger
    {
        public static async Task SendMessageToEventHubAsync(string message)
        {
            var logRecord = new
            {
                Timestamp = DateTime.UtcNow,
                Level = "High",
                Message = message
            };
            var json = JsonSerializer.Serialize(logRecord);
            await using (var producerClient = new EventHubProducerClient(AdxConfig.EventHubConnectionString, AdxConfig.EventHubName))
            {
                using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

                eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(json)));

                try
                {
                    await producerClient.SendAsync(eventBatch);
                    Console.WriteLine("Log record sent to Event Hub.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending log record: {ex.Message}");
                }
            }
        }
    }
}
