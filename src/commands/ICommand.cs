using Npgsql;

namespace PGMQ.NET.Commands;

public interface ICommand
{
    Task<int> Execute(NpgsqlConnection connection, Action<NpgsqlCommand, Dictionary<string, object>>? action = null);
}