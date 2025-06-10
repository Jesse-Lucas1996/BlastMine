namespace BlastMine.Shared;

public class DatabaseSettings: IDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
}