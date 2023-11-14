using System.Text.Json;

namespace LoadBalancer.DataBase.Connection;

public static class Reader
{
    private static readonly string _path = "DataBase/Connection/db.json";
    public static List<DBInstance>? DBsConnectionStrings { get; set; }


    static Reader()
    {
        InitializeConnectionStringList();
    }


    private static void InitializeConnectionStringList()
    {
        var jsonContent = File.ReadAllText(_path);
        DBsConnectionStrings = JsonSerializer.Deserialize<List<DBInstance>>(jsonContent) ??
                               throw new InvalidOperationException();
    }
}