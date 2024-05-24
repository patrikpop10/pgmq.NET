using Microsoft.Extensions.DependencyInjection;
using PGMQ.NET.Queue.Builder;
using PGMQ.NET.Queue.Interfaces;

namespace PGMQ.NET.Extensions;

public static class ServiceExtension
{
    public static void AddQueueWriterSingleton(this IServiceCollection serviceCollection, Action<QueueBuilderWriterOptions> action)
    {
        var queueWriter = QueueBuilder.CreateQueueWriter(action);
        serviceCollection.AddSingleton(queueWriter);
    }

    public static void AddQueueReaderSingleton(this IServiceCollection serviceCollection,
        Action<QueueBuilderReaderOptions> action)
    {
        var queueReader = QueueBuilder.CreateQueueReader(action);
        serviceCollection.AddSingleton(queueReader);
    }
    public static void AddQueueSingleton(this IServiceCollection serviceCollection, 
        Action<QueueBuilderWriterOptions> writerAction, 
        Action<QueueBuilderReaderOptions> readerAction)
    {
        serviceCollection.AddQueueWriterSingleton(writerAction);
        serviceCollection.AddQueueReaderSingleton(readerAction);
    }
    public static void AddQueueScoped(this IServiceCollection serviceCollection, 
        Action<QueueBuilderWriterOptions> writerAction, 
        Action<QueueBuilderReaderOptions> readerAction)
    {
        serviceCollection.AddQueueWriterScoped(writerAction);
        serviceCollection.AddQueueReaderScoped(readerAction);
    }

    public static void AddQueueWriterScoped(this IServiceCollection serviceCollection, 
        Action<QueueBuilderWriterOptions> writerAction)
        => serviceCollection.AddScoped<IQueueWriter>(_ => QueueBuilder.CreateQueueWriter(writerAction));
    
    public static void AddQueueReaderScoped(this IServiceCollection serviceCollection, 
        Action<QueueBuilderReaderOptions> readerAction) 
        => serviceCollection.AddScoped<IQueueReader>(_ => QueueBuilder.CreateQueueReader(readerAction));

    public static void AddQueueTransient(this IServiceCollection serviceCollection,
        Action<QueueBuilderWriterOptions> writerAction, 
        Action<QueueBuilderReaderOptions> readerAction)
    {
        serviceCollection.AddQueueReaderTransient(readerAction);
        serviceCollection.AddQueueWriterTransient(writerAction);
    }
    public static void AddQueueWriterTransient(this IServiceCollection serviceCollection, 
        Action<QueueBuilderWriterOptions> writerAction)
        => serviceCollection.AddTransient<IQueueWriter>(_ => QueueBuilder.CreateQueueWriter(writerAction));
    public static void AddQueueReaderTransient(this IServiceCollection serviceCollection, 
        Action<QueueBuilderReaderOptions> readerAction)
        => serviceCollection.AddTransient<IQueueReader>(_ => QueueBuilder.CreateQueueReader(readerAction));


}