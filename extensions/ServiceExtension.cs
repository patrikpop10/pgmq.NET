using Microsoft.Extensions.DependencyInjection;
using PGMQ.NET.Queue.Builder;

namespace PGMQ.NET.Extensions;

public static class ServiceExtension
{
    public static async Task AddQueueWriter(this IServiceCollection serviceCollection, Action<QueueBuilderWriterOptions> action)
    {
        var queueWriter =  await QueueBuilder.CreateQueueWriter(action);
        serviceCollection.AddSingleton(queueWriter);
    }

    public static void AddQueueReader(this IServiceCollection serviceCollection,
        Action<QueueBuilderReaderOptions> action)
    {
        var queueReader = QueueBuilder.CreateQueueReader(action);
        serviceCollection.AddSingleton(queueReader);
    }
}