using System;
using System.Collections.Specialized;
using Microsoft.Data.SqlClient;
namespace Program;
class Program
{
    public static void Main (string[] args)
    {
    string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("key1");
    SqlConnection connection = new SqlConnection("Data Source = DESKTOP-UH1K1R2; Initial Catalog = TutorialDB; Integrated Security = true;TrustServerCertificate=true;");
    connection.Open();
    SqlCommand command = new SqlCommand("SELECT * FROM dbo.Customers", connection);
    using (SqlDataReader reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            Console.WriteLine($"{reader[0]} \t {reader[1]} \t {reader[2]} \t ");
        }

    }


    Console.ReadLine();
    }
}


