using PGMQ.NET.Queue.Interfaces;

namespace pgmq_consumer;

public class WriterBackgroundService : BackgroundService
{
    private readonly ILogger<WriterBackgroundService> _logger;
    private readonly IQueueWriter _writer;
    private readonly List<string> _names = ["jogo", "ricardo", "tula", "john", "eddie", "danny", "reach"];
    private readonly Random _random = new();

    
    public WriterBackgroundService(ILogger<WriterBackgroundService> logger, IQueueWriter writer)
    {
        _logger = logger;
        _writer = writer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting write for writer {writer}", _writer);
        while (true)
        {
            var index = _random.Next(0, _names.Count);
            await _writer.Send(new XX{Name = _names[index], DateTime = DateTime.UtcNow});
            await Task.Delay(1_500, stoppingToken);
        }
        
    }
    
    

 
}
public class XX
{
    public DateTime DateTime { get; init; }
    public string? Name { get; init; }

}