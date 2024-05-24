using Npgsql;
using PGMQ.NET.Queue.Interfaces;
using PGMQ.NET.Queue.Reader;
using PGMQ.NET.Queue.Writer;

namespace PGMQ.NET.Queue.Builder;
public static class QueueBuilder
{
    public static IQueueWriter CreateQueueWriter(Action<QueueBuilderWriterOptions> optionsAction)
    {
        var queueBuilderOptions = new QueueBuilderWriterOptions();
        optionsAction(queueBuilderOptions);
        var queueWriter = new QueueWriter(queueBuilderOptions.ConnectionString ,queueBuilderOptions.QueueName!);
        if (queueBuilderOptions.CreateQueue)
        {
            Task.Run(async () =>
                await queueWriter.CreateQueue(queueBuilderOptions.QueueName!, queueBuilderOptions.Unlogged));
        }
        return queueWriter;
    }
    public static IQueueReader CreateQueueReader(Action<QueueBuilderReaderOptions> optionsAction)
    {
        var queueBuilderOptions = new QueueBuilderReaderOptions();
        optionsAction(queueBuilderOptions);
        var queueReader = new QueueReader(queueBuilderOptions.ConnectionString!, queueBuilderOptions.QueueName!);
        return queueReader;
    }
}
