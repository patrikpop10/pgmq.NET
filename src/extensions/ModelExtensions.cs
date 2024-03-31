using System.Text.Json;
using Npgsql;
using PGMQ.NET.Models;

namespace PGMQ.NET.Extensions;

internal static class ModelExtensions
{
    private const int MessageIdIndex = 0;
    private const int ReadCountIndex = 1;
    private const int EnqueueAtIndex = 2;
    private const int VtIndex = 3;
    private const int MessageIndex = 4;
    internal static PGMQModel<T?> MapToPGMQModel<T>(this NpgsqlDataReader? result)
    => new()
    {
        MessageId = result!.GetInt64(MessageIdIndex),
        ReadCount = result.GetInt32(ReadCountIndex),
        EnqueueAt = result.GetDateTime(EnqueueAtIndex),
        Vt = result.GetDateTime(VtIndex),
        Message = JsonSerializer.Deserialize<T>(result.GetFieldValue<string>(MessageIndex))
    };
    internal static PGMQModel<T> MapToPGMQModelString<T>(this NpgsqlDataReader? result)
    => new()
    {
        MessageId = result!.GetInt64(MessageIdIndex),
        ReadCount = result.GetInt32(ReadCountIndex),
        EnqueueAt = result.GetDateTime(EnqueueAtIndex),
        Vt = result.GetDateTime(VtIndex),
        Message = result.GetFieldValue<T>(MessageIndex)
    };

}
