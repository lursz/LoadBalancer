using LoadBalancer.Core;

namespace LoadBalancer.Abstracts;


public interface IUnitOfWork
{
    void register(DbRequest request);
    
    void syncChanges();
}