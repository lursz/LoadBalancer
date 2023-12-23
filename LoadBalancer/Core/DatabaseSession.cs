namespace LoadBalancer.Core;

public class DatabaseSession : Abstracts.ManageableSession
{
    public DatabaseSession()
    {
        Console.WriteLine("DatabaseSession created");
    }

    public override object execute()
    {
        Console.WriteLine("DatabaseSession execute");
        return null;
    }

    public override void fix()
    {
        Console.WriteLine("DatabaseSession fix");
    }

    public override bool isUsed()
    {
        Console.WriteLine("DatabaseSession isUsed");
        return false;
    }
}