# LoadBalancer
1. [Description](#Description)
2. [Design](#Design)
3. [Flow](#Flow)
4. [Stack](#Stack)

## Description
The main idea behind this project is to create a load balancer that will act as a broker between the queries and the databases. It's job is to perform CRUD operations and keep all DBs synchronized.  
Moreover the load balancer must be able to handle all kinds of unexpected events thrown at him, including but not limited to:
- Failure of one or multiple DBs
- Reconnection of DBs


## Design
![Design](https://github.com/lursz/LoadBalancer/assets/64146291/9e673ba3-bd2e-40ba-a218-01d5da744e42)

## Flow
![Flow](https://github.com/lursz/LoadBalancer/assets/64146291/ebb09d86-8915-4b79-bebc-2df979f096ea)


## Notes
- We have to get a new connection from the load balancer whenever we want to send request and mark the retrieved session object as `alreadyUsed` or whatever. We don't want to duplicate requests others than select.
- Intercept only insert/update/delete. Call redirect method in load balancer that will send or queue this request in every session object except for the one with alreadyUsed set to true (this is the one that was primary used to build and send a request, so we don't want to do that again).
- Queuing depends if the session is active or not.
- If the session is down, we will have to reconnect it ...some day
- GL & HF


# Design Patterns
During the development of this project we have used the following design patterns:
- [Bridge](https://en.wikipedia.org/wiki/Bridge_pattern)
- [Factory](https://en.wikipedia.org/wiki/Factory_method_pattern)
- [Singleton](https://en.wikipedia.org/wiki/Singleton_pattern)
- [Observer](https://en.wikipedia.org/wiki/Observer_pattern)
- [State](https://en.wikipedia.org/wiki/State_pattern)
- [Proxy](https://en.wikipedia.org/wiki/Proxy_pattern)
- [Builder](https://en.wikipedia.org/wiki/Builder_pattern)
- [Adapter](https://en.wikipedia.org/wiki/Adapter_pattern)
- [Facade](https://en.wikipedia.org/wiki/Facade_pattern)

- ![image](https://github.com/lursz/LoadBalancer/assets/64146291/c84d9563-e690-4a1a-8700-85557ff55fc4)

- ![image](https://github.com/lursz/LoadBalancer/assets/64146291/77da963b-51e9-40bf-9175-fa3dbd5d6547)



## Stack
In our project we have used:
- `C#`
- `.NET 8.0`
- `NHibernate`
- `NHibernate.Core`
- `Nhibernate.Linq`
- `Docker`
- `PostgreSQL`
