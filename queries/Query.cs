using System.Data;
using Npgsql;
using PGMQ.NET.Commands.Consts;

namespace PGMQ.NET.Queries;

internal sealed class Query(string querystring, Dictionary<string, object> parameters) : AbstractQuery, IQuery
{
    internal Query(string querystring) : this(querystring, new Dictionary<string, object>())
    {
    }

    protected override string Querystring { get; set; } = querystring;
    protected override Dictionary<string, object> Parameters { get; set; } = parameters;

    public async Task<NpgsqlDataReader> Execute(Action<NpgsqlCommand, Dictionary<string, object>>? action = null)
    {
        await using var connection = new NpgsqlConnection(CommandConsts.ConnectionString);
        await connection.OpenAsync();
        var command = new NpgsqlCommand(Querystring, connection);
        action?.Invoke(command, Parameters);
        var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        return reader;
    }
    public async Task<NpgsqlDataReader> Execute(NpgsqlConnection connection,Action<NpgsqlCommand, Dictionary<string, object>>? action = null)
    {
        var command = new NpgsqlCommand(Querystring, connection);
        action?.Invoke(command, Parameters);
        var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        return reader;
    }
}