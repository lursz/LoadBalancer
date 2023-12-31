namespace LoadBalancer.Core;

public class DbRequest
{
    public enum Type
    {
        SELECT, INSERT, UPDATE, DELETE
    }
    
    private readonly object dbObject;
    private readonly Type type;
    
    public DbRequest(object dbObject, Type type)
    {
        this.dbObject = dbObject;
        this.type = type;
    }
    
    public object getObject()
    {
        return this.dbObject;
    }
    
    public Type getType()
    {
        return this.type;
    }
}