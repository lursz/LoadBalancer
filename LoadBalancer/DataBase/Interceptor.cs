using System.Collections;
using LoadBalancer.Connection;
using NHibernate;
using NHibernate.Event;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace LoadBalancer.DataBase;

public class Interceptor : EmptyInterceptor
{
    private readonly List<DbInstance> _dBsConnStr = Reader.DBsConnectionStrings;

    
    public override SqlString OnPrepareStatement(SqlString sql)
    {
        
        using var session = NHibernate.Context.CurrentSessionContext.HasBind(NHibernateHelper.SessionFactory)
            ? NHibernateHelper.SessionFactory.GetCurrentSession()
            : NHibernateHelper.SessionFactory.OpenSession();
    
        Console.WriteLine("session.Connection.ConnectionString: " + session.Connection.ConnectionString);
        session.Connection.ConnectionString = _dBsConnStr[1].ConnectionString;
        
        var sqlString = sql.ToString();
        sqlString = sqlString.Replace(_dBsConnStr[0].ConnectionString, _dBsConnStr[1].ConnectionString);
        return new SqlString(sqlString);
        
    }
    
}



