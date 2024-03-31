using Dapper;
using Npgsql;
using PGMQ.NET.Extensions;

namespace PGMQ.NET.Parametes;

internal static class ParameterAdder
{
    internal static readonly Action<NpgsqlCommand, Dictionary<string, object>> WithJsonB = (command, parameters) =>
    {
        foreach (var parameter in parameters)
        {
            if (parameter.Value is SqlMapper.ICustomQueryParameter queryParameter)
            {
                command.AddCustomParameter(parameter.Key, queryParameter);
                continue;
            }
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
        }
    };
    internal static readonly Action<NpgsqlCommand, Dictionary<string, object>> Simple = (command, parameters) => 
        command.Parameters.AddRange(parameters.Select(x => new NpgsqlParameter(x.Key, x.Value)).ToArray());
    
}