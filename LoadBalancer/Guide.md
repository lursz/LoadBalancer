Dotnet install Entity Framework Core Tools
```bash
dotnet tool install --g dotnet-ef
```
Dotnet migrate database Model
```bash
dotnet ef migrations add InitialCreate
```
__________________________

Dotnet run migration - create database
```bash
dotnet ef database update
```