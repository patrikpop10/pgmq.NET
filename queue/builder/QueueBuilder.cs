using Npgsql;
using PGMQ.NET.Queue.Interfaces;
using PGMQ.NET.Queue.Reader;
using PGMQ.NET.Queue.Writer;

namespace PGMQ.NET.Queue.Builder;
public static class QueueBuilder
{
    public static async Task<IQueueWriter> CreateQueueWriter(Action<QueueBuilderWriterOptions> optionsAction)
    {
        var queueBuilderOptions = new QueueBuilderWriterOptions();
        optionsAction(queueBuilderOptions);
        var queue = new QueueWriter(queueBuilderOptions.ConnectionString ,queueBuilderOptions.QueueName!);
        if (queueBuilderOptions.CreateQueue)
        {
           await queue.CreateQueue(queueBuilderOptions.QueueName!, queueBuilderOptions.Unlogged);
        }
        return queue;
    }
    public static IQueueReader CreateQueueReader(Action<QueueBuilderReaderOptions> optionsAction)
    {
        var queueBuilderOptions = new QueueBuilderReaderOptions();
        optionsAction(queueBuilderOptions);
        var queue = new QueueReader(queueBuilderOptions.ConnectionString!, queueBuilderOptions.QueueName!);
        return queue;
    }
}
