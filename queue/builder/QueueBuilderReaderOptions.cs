namespace PGMQ.NET.Queue.Builder;
public class QueueBuilderReaderOptions
{
    public string? ConnectionString { get; set; }
    public string? QueueName { get; set; }
    public bool CreateQueue { get; set; }
    public bool Unlogged { get; set; }
}
