using Azure.Messaging.ServiceBus;

namespace Consumer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServiceBusClient client;
            ServiceBusProcessor processor;
            const string? connectionString = "";
            const string? queueName = "";

            client = new ServiceBusClient(connectionString);
            processor = client.CreateProcessor(queueName);

            try
            {
                processor.ProcessMessageAsync += MessageHandler;
                processor.ProcessErrorAsync += ErrorHandler;

                await processor.StartProcessingAsync();
                Console.WriteLine("Ouvindo...");
                Console.WriteLine("Pressione uma tecla para parar.");
                Console.ReadKey();
                await processor.StopProcessingAsync();

            }
            finally
            {
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }

        }
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}