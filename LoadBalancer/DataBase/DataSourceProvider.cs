
public class DataSourceProvider : IDataSourceProvider
{

    public DataSource CurrentDataSource { get; set; }
    public DataSourceProvider()
    {
    }
    
    public string GetConnectionString()
    {
        return CurrentDataSource switch
        {
            DataSource.Primary => "Host=localhost;Port=5432;Username=user1;Password=password1;Database=database1",
            DataSource.Secondary => "Host=localhost;Port=5433;Username=user2;Password=password2;Database=database2",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}


public enum DataSource
{
    Primary,
    Secondary
}

public interface IDataSourceProvider
{
    DataSource CurrentDataSource { set; }
    string GetConnectionString();
}