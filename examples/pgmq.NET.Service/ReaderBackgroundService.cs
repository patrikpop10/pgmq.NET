using pgmq_consumer;
using PGMQ.NET.Queue.Interfaces;

namespace pgmq_web;

public class ReaderBackgroundService : BackgroundService 
{
    private readonly ILogger<ReaderBackgroundService> _logger;
    private readonly IQueueReader _reader;

    public ReaderBackgroundService(ILogger<ReaderBackgroundService> logger, IQueueReader reader)
    {
        _logger = logger;
        _reader = reader;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting reader");
        
        await foreach (var i in _reader.ContinuousPop<string>(stoppingToken))
        {
            string? x = i;
            _logger.LogInformation("{x}", x);
            //_logger.LogInformation("{y}", x?.Name);
            _logger.LogInformation("{z} \n", i.MessageId);
            await Task.Delay(1000, stoppingToken);
            
        }
    }
    
}