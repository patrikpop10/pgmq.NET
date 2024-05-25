using System.Runtime.CompilerServices;
using Npgsql;
using PGMQ.NET.Commands.Consts;
using PGMQ.NET.Extensions;
using PGMQ.NET.Models;
using PGMQ.NET.Parametes;
using PGMQ.NET.Queries;
using PGMQ.NET.Queries.Const;
using PGMQ.NET.Queue.Interfaces;

namespace PGMQ.NET.Queue.Reader;

public class QueueReader : IQueueReader
{
    private readonly string _queue;

    internal QueueReader(string connectionString, string queue)
    {
        _queue = queue;
        CommandConsts.ConnectionString  = connectionString;
    }
    public async Task<PGMQModel<T?>> Read<T>(int vt = 0)
    {
        var readQuery = new Query(QueryConsts.ReadQuery, new Dictionary<string, object>()
        {
         ["queue"] = _queue,
         ["vt"] = vt
        });
        return await Reader<T>(readQuery);
    }
    private static async Task<PGMQModel<T?>> Reader<T>(IQuery query)
    {
        await using NpgsqlConnection connection = new(CommandConsts.ConnectionString);
        await connection.OpenAsync();
        var result = await query.Execute(connection,ParameterAdder.Simple);
        while (result.Read())
        {
            return typeof(T) ==  typeof(string) ? result.MapToPGMQModelString<T?>() : result.MapToPGMQModel<T?>();
        }
        return default!;
    }
    public async Task<PGMQModel<T?>> Pop<T>()
    {
        var popQuery = new Query(QueryConsts.PopQuery,new Dictionary<string, object>()
        {
            ["queue"] =_queue
        });
        return await Reader<T>(popQuery);
    }

    public async IAsyncEnumerable<PGMQModel<T?>> ContinuousPop<T>([EnumeratorCancellation] CancellationToken cancellationToken)
    {
         while (true) {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            var x = await Pop<T>();
            if (x is null)
            {
                continue;
            }
            yield return x; 
         }
    }
    public async Task Archive(long id)
    {
        var archiveQuery = new Query(QueryConsts.ArchiveQuery, new Dictionary<string, object>()
        {
            ["queue"] = _queue,
            ["id"] = id
        });
        await archiveQuery.Execute(ParameterAdder.Simple);
    }
    public async Task<PGMQModel<T?>> ReadAndArchive<T>()
    {
        var x = await Read<T>();
        if(x is null)
        {
            return null!;
        }
        await Archive((long)x.MessageId);
        return x;
    }
    public async IAsyncEnumerable<PGMQModel<T?>> ContinuousReadAndArchive<T>([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        do
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            var x = await ReadAndArchive<T>();
            if (x is null)
            {
                continue;
            }
            yield return x;
        }
        while (true);
    }


}
