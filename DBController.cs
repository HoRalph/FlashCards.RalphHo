/*string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("key1");
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

*/

/*
crate table
sql string

*/
using System.Data;
using Microsoft.Data.SqlClient;
class DBController
{
    public static SqlConnection ConnectDB()
    {
        string?connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");
        SqlConnection connection  =  new SqlConnection(connectionString);
        connection.ConnectionString  = connectionString;
        return  connection;
    }
    public static void CreateTables(SqlConnection connection)
    {
    String  createFlashCardsTable =   @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                    WHERE TABLE_SCHEMA = 'dbo'
                                    AND TABLE_NAME = 'FlashCards')
                                    BEGIN
                                    CREATE TABLE FlashCards (
                                    ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                    Name varchar(255),
                                    StackId int)
                                    END;
                                    ";
    string createStacksTable = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_SCHEMA = 'dbo'
                                AND TABLE_NAME = 'Stacks')
                                CREATE TABLE Stacks (
                                ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                Name varchar(255));";
    string createSessionsTable = @"IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                WHERE TABLE_SCHEMA = 'dbo'
                                AND TABLE_NAME = 'Sessions'))
                                CREATE TABLE Sessions (
                                ID Int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                DateTime varchar(255),
                                Stack varchar(255),
                                Score DECIMAL(2,2 ));";
    using (connection)
    {
    connection.Open();
    SqlCommand command = new SqlCommand(createFlashCardsTable,  connection);
    command.ExecuteNonQuery();
    command.CommandText = createStacksTable;
    command.ExecuteNonQuery();
    command.CommandText = createSessionsTable;
    command.ExecuteNonQuery();
    connection.Close();
    }
    }
    public static string ViewTable(string table)
    {
        return "A";
    }
    public static void UpdateRecord()
    {

    }
    public static void DeleteRecord()
    {

    }
    public static void InsertRecord(string table, )
    {
        switch (table)
        {
            case "FlashCards":
            string?insertSqlString = "a";
                break;
            case "Stacks":
                break;
            case "Sessions":
                break;
            default:
                break;
        }
    }
}