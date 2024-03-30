using PGMQ.NET.Models;

namespace PGMQ.NET.Queue.Interfaces;

public interface IQueueReader
{
    Task<PGMQModel<T?>> Read<T>(int vt = 0);
    Task<PGMQModel<T?>> Pop<T>();
    Task Archive(long id);
    Task<PGMQModel<T?>> ReadAndArchive<T>();
    IAsyncEnumerable<PGMQModel<T?>> ContinuousReadAndArchive<T>(CancellationToken cancellationToken);
    IAsyncEnumerable<PGMQModel<T?>> ContinuousPop<T>(CancellationToken cancellationToken);
}