using Npgsql;

namespace PGMQ.NET.Commands;

public class Command : ICommand
{
    private readonly string _command;
    private readonly Dictionary<string, object> _parameters;
    public Command(string command, Dictionary<string, object> parameters)
    {
        _command = command;
        _parameters = parameters;
    }
    public async Task<int> Execute(NpgsqlConnection connection, Action<NpgsqlCommand, Dictionary<string, object>>? action = null)
    {
        await using var command = new NpgsqlCommand(_command, connection);
        action?.Invoke(command, _parameters);
        return await command.ExecuteNonQueryAsync();
    }
}