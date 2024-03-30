using System.Data;
using Dapper;

namespace PGMQ.NET.Extensions;

public static class DbCommandExtensions
{
    internal static void AddCustomParameter(this IDbCommand dbCommand, string s, SqlMapper.ICustomQueryParameter parameter)
    {
        parameter.AddParameter(dbCommand, s);
    }    
}