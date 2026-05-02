using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace ShopApp.Services;
using ShopApp.Models;
public class DatabaseService
{
    // Строка подключения. 
    // Если SQL Server в Docker на этой же машине, используй localhost.
    // TrustServerCertificate=True нужен, если у тебя нет SSL сертификата на сервере.
    private readonly string _connectionString = 
        "Server=localhost;Database=Shop;User Id=SA;Password=YourStrong%Passw0rd;TrustServerCertificate=True;";

    public List<User> GetAllUsers()
    {
        var users = new List<User>();

        try 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Username, Email, FullName, Role, IsActive FROM Users";
                
                using (var command = new SqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            // Проверка на null из базы (Email и FullName могут быть пустыми)
                            Email = reader.IsDBNull(2) ? null : reader.GetString(2),
                            FullName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Role = reader.GetString(4),
                            IsActive = reader.GetBoolean(5)
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // На этапе разработки полезно видеть ошибки в консоли терминала
            Console.WriteLine($"Database Error: {ex.Message}");
        }

        return users;
    }
}