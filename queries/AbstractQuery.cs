namespace PGMQ.NET.Queries;

public abstract class AbstractQuery
{
    protected abstract string Querystring { get; set; }
    protected abstract Dictionary<string, object> Parameters { get; set; }
}