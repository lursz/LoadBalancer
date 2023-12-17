# LoadBalancer

Design Patterns Project

List all packages:

```bash
dotnet list package
```

## Design
<img width="984" alt="image" src="https://github.com/lursz/LoadBalancer/assets/64146291/9e673ba3-bd2e-40ba-a218-01d5da744e42">

## Flow
<img width="1317" alt="image" src="https://github.com/lursz/LoadBalancer/assets/64146291/ebb09d86-8915-4b79-bebc-2df979f096ea">


## Notes:
- We have to get a new connection from the load balancer whenever we want to send request and mark the retrieved session object as `alreadyUsed` or whatever. We don't want to duplicate requests others than select.
- Intercept only insert/update/delete. Call redirect method in load balancer that will send or queue this request in every session object except for the one with alreadyUsed set to true (this is the one that was primary used to build and send a request, so we don't want to do that again).
- Queuing depends if the session is active or not.
- If the session is down, we will have to reconnect it ...some day
- GL & HF
