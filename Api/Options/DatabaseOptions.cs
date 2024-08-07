namespace Api.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public bool LogToDebugWriteLine { get; set; } = false;
    
    public int CommandTimeout { get; set; }
    
    public bool EnableSensitiveDataLogging { get; set; }
    
    public bool EnableDetailedErrors { get; set; }
}