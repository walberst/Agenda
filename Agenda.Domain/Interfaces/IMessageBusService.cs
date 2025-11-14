namespace Agenda.Domain.Interfaces;

public interface IMessageBusService
{
    Task PublishAsync(string queue, object message);
    Task SubscribeAsync(string queue, Func<string, Task> handler);
}
