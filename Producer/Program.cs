using Azure.Messaging.ServiceBus;
using Azure.Identity;

namespace Producer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServiceBusClient client;
            ServiceBusSender sender;
            const string? connectionString = "";
            const string? queueName = "";
            const int numMessage = 3;

            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);

            using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();

            for (int i = 0; i <= numMessage; i++)
            {
                string message = $"Mensagem: {i+1} da fila.";
                if (!batch.TryAddMessage(new ServiceBusMessage(message))) 
                {
                    Console.WriteLine("Um erro aconteceu.");
                    throw new Exception("Erro ao inserir a mensagem");
                }
            }

            try
            {
                Console.WriteLine("Pressione uma tecla para enviar as mensagens.");
                Console.ReadKey();
                await sender.SendMessagesAsync(batch);
                Console.WriteLine("Pressione uma tecla para parar.");
                Console.ReadKey();
            }
            finally
            {
                Console.WriteLine($"{numMessage} foram enviadas para a fila");
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}