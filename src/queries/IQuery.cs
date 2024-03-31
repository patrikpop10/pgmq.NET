using Npgsql;

namespace PGMQ.NET.Queries;

public interface IQuery
{ 
    Task<NpgsqlDataReader> Execute(Action<NpgsqlCommand, Dictionary<string, object>>? action = null);
    Task<NpgsqlDataReader> Execute(NpgsqlConnection connection ,Action<NpgsqlCommand, Dictionary<string, object>>? action = null);
}