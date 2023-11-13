using System.Text.Json;

namespace LoadBalancer.DataBase.Connection;

public class Reader
{
    private readonly string _path = "DataBase/Connection/db.json";
    public List<DBInstance>? DBsConnectionStrings { get; set; }


    public Reader()
    {
        InitializeConnectionStringList();
    }


    private void InitializeConnectionStringList()
    {
        var jsonContent = File.ReadAllText(_path);
        DBsConnectionStrings = JsonSerializer.Deserialize<List<DBInstance>>(jsonContent) ??
                               throw new InvalidOperationException();
    }
}