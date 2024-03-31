using System.Data;
using Npgsql;
using NpgsqlTypes;
using static Dapper.SqlMapper;

namespace PGMQ.NET.Parametes;

internal class JsonBParameter : ICustomQueryParameter
{
    private readonly string _json;

    internal JsonBParameter(string json)
    {
        _json = json;
    }

    public void AddParameter(IDbCommand command, string name)
    {
        var parameter = (NpgsqlParameter)command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = _json;
        parameter.NpgsqlDbType = NpgsqlDbType.Jsonb;
        command.Parameters.Add(parameter);
    }
}