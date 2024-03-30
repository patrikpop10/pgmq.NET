using System.Text.Json;
using PGMQ.NET.Commands.Consts;
using PGMQ.NET.Parametes;
using PGMQ.NET.Queries;
using PGMQ.NET.Queries.Const;
using PGMQ.NET.Queue.Interfaces;

namespace PGMQ.NET.Queue.Writer;
public class QueueWriter : IQueueWriter
{
    private readonly string _queue;
    public QueueWriter(string? connectionString, string queue)
    {
        CommandConsts.ConnectionString = connectionString!;
        _queue = queue; 
        var query = new Query(CommandConsts.InstallExtension);
         query.Execute().Wait();
    }
    public async Task CreateQueue(string q, bool unLogged = false)
    {
        var queryString = unLogged ? QueryConsts.CreateUnLogged : QueryConsts.Create;
        var query = new Query(queryString,new Dictionary<string, object>
            {
              ["q"] = q
            });
        
        await query.Execute(ParameterAdder.Simple);
    }
    public async Task Send<T>(T message, int delay = 0) => await SendResolved(message, delay);
    
    public async Task Send<T>(IEnumerable<T> messages, int delay = 0)  => await SendResolved(messages, delay);
    private async Task SendResolved<T>(T messages, int delay)
    {  
        var json = JsonSerializer.Serialize(messages);
        var jsonb = new JsonBParameter(json);
        var query = new Query(QueryConsts.SendQuery ,new Dictionary<string, object>()
        {
            ["queue"] = _queue,
            ["jsonb"] = jsonb,
            ["delay"] = delay
        });
        await query.Execute(ParameterAdder.WithJsonB); 
    }
}