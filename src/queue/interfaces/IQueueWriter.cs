namespace PGMQ.NET.Queue.Interfaces;

public interface IQueueWriter
{ 
    Task Send<T>(T message, int delay = 0);
    Task Send<T>(IEnumerable<T> messages, int delay = 0);
    Task CreateQueue(string q, bool unLogged = false);
}