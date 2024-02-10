# LoadBalancer
## Dev branch
The dev branch contains one significant change - an abillity to operate with all databases down. This being said, dev version is not yet polished and may contain some bugs.

## Description
The main idea behind this project is to create a Load Balancer acting as a broker between the queries and the databases. Its job is to perform CRUD operations and keep all DBs synchronized.  
For SELECT operations LB will use only one databse, chosen by the suitable strategy (e.g. round robin). For INSERT/UPDATE/DELETE operations, LB will send the request to all databases, thus keeping them synchronized. 
Moreover, the load balancer must be able to handle all kinds of unexpected events thrown at him, including but not limited to:
- Failure of one or multiple DBs
- Reconnection of DBs
- Multiple requests at the same time


## Run
In order to run the project, you must have [Docker](https://www.docker.com/products/docker-desktop/) (with Docker Compose) and [.NET 8.0](https://dotnet.microsoft.com/en-us/download) 
installed on your machine.  
First, you need to create the databases. You can provide your own or use Docker to create them. To do the latter, run following command in the `/Docker` directory:
```bash
docker-compose up
```
With the databases up and running, you can now run the Load Balancer. To do so, navigate to the `/LoadBalancer` directory and run the project
```bash
dotnet run
```


## Design
Design of our project is depicted on the following diagram:
![diagram](https://github.com/lursz/LoadBalancer/assets/93160829/b1dc3d0e-7216-41d2-af59-823c2c31b3b3)


Presented below is the initialization flow (to help understand the order of operations):
![Flow](https://github.com/lursz/LoadBalancer/assets/64146291/ebb09d86-8915-4b79-bebc-2df979f096ea)


# Used Design Patterns
During the development of this project we have used the following design patterns:
- [Factory method](https://en.wikipedia.org/wiki/Factory_method_pattern)
![factory3](https://github.com/lursz/LoadBalancer/assets/93160829/3f1314ff-250f-474f-a5ae-be1f5c339dde)

- [State](https://en.wikipedia.org/wiki/State_pattern)
![state0](https://github.com/lursz/LoadBalancer/assets/93160829/34ace22a-7f64-43ac-b0c3-83d76946a499)


- [Strategy](https://en.wikipedia.org/wiki/Strategy_pattern)
![strategy1](https://github.com/lursz/LoadBalancer/assets/93160829/98530907-16b0-4c5a-9f3f-f8788e967220)


- [Unit of work](https://en.wikipedia.org/wiki/Unit_of_work)
![unit_of_work](https://github.com/lursz/LoadBalancer/assets/93160829/9c390684-09f6-4561-b3d6-432930a97170)



## Technology stack
- `C#`
- `.NET 8.0`
- `NHibernate`
- `Docker`
- `PostgreSQL`



