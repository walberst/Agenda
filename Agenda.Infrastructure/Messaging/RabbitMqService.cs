using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Agenda.Domain.Interfaces;

namespace Agenda.Infrastructure.Messaging;

public class RabbitMqService : IMessageBusService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task PublishAsync(string queue, object message)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublish(exchange: "",
                              routingKey: queue,
                              basicProperties: null,
                              body: body);

        Console.WriteLine($"[RabbitMQ] Mensagem publicada na fila '{queue}'");

        return Task.CompletedTask;
    }

    public Task SubscribeAsync(string queue, Func<string, Task> handler)
    {
        _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await handler(message);
        };

        _channel.BasicConsume(queue, autoAck: true, consumer: consumer);

        Console.WriteLine($"[RabbitMQ] Escutando fila '{queue}'...");

        return Task.CompletedTask;
    }
}
