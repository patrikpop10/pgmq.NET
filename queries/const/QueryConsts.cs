namespace PGMQ.NET.Queries.Const;

public static class QueryConsts
{
    internal const string SendQuery = "SELECT * FROM pgmq.send(@queue, @jsonb, @delay);";
    internal const string PopQuery = "SELECT * FROM pgmq.pop(@queue);";
    internal const string ReadQuery = "SELECT * FROM pgmq.read(@queue, @vt , 1);";
    internal const string CreateUnLogged = "SELECT * FROM pgmq.create_unlogged(@q);";
    internal const string Create = "SELECT * FROM pgmq.create(@q);"; 
    internal const string CheckTable = "SELECT tablename FROM pg_tables WHERE tablename = 'a_' || @table;";
    internal const string ArchiveQuery = "SELECT * FROM pgmq.archive(@queue, @id);";
}

