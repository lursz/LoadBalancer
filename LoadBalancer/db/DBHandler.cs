using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace LoadBalancer.db
{
    public class DBHandler
    {
        private string connectionString;

        public DBHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void CreateTable()
        {
            using NpgsqlConnection? connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using NpgsqlCommand? command = new NpgsqlCommand("CREATE TABLE users (id serial PRIMARY KEY, username VARCHAR (50) UNIQUE NOT NULL, password VARCHAR (50) NOT NULL, email VARCHAR (355) UNIQUE NOT NULL)", connection);
            command.ExecuteNonQuery();
        }

        
        public void gowno()
        {
        using NpgsqlConnection? connection = new NpgsqlConnection(connectionString: "Host=localhost;Port=5432;Username=user1;Password=password1;Database=database1");
        connection.Open();
        using NpgsqlCommand? command = new NpgsqlCommand("CREATE TABLE users3 (id serial PRIMARY KEY, username VARCHAR (50) UNIQUE NOT NULL, password VARCHAR (50) NOT NULL, email VARCHAR (355) UNIQUE NOT NULL)", connection);
        command.ExecuteNonQuery();
        }
    }
}