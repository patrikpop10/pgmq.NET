namespace PGMQ.NET.Commands.Consts;

public static class CommandConsts
{
    internal const string InstallExtension = "CREATE EXTENSION IF NOT EXISTS pgmq;";
    internal static string ConnectionString { get; set; } = string.Empty;
}