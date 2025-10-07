using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;


Console.WriteLine("Разработка клиента");


var connectionString = "";
using IDbConnection connection = new SqlConnection(connectionString);
string query = "INSERT INTO Category(title) OUTPUT INSERTED.CategoryId VALUES (@title)";

