using NHibernate;
using NHibernate.Type;

namespace LoadBalancer.Abstracts;

public abstract class MonitorThread
{
    
        public abstract void Run();
    
        public abstract void Check();
}