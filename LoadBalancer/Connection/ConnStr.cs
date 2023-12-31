using System.Text.Json;

namespace LoadBalancer.Connection;

public static class Reader
{
    private static readonly string _path = "Connection/db.json";
    public static List<DbInstance>? DBsConnectionStrings { get; set; }
    
    static Reader()
    {
        InitializeConnectionStringList();
    }
    
    private static void InitializeConnectionStringList()
    {
        var jsonContent = File.ReadAllText(_path);
        DBsConnectionStrings = JsonSerializer.Deserialize<List<DbInstance>>(jsonContent) ??
                               throw new InvalidOperationException();
    }
}

public class DbInstance
{
    public string Name { get; set; }
    public string ConnectionString { get; set; }
}