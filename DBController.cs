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
using Microsoft.Data.SqlClient;
using Spectre;
using Spectre.Console;

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
        String  createFlashCardsTable = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES
                                        WHERE TABLE_SCHEMA = 'dbo'
                                        AND TABLE_NAME = 'FlashCards')
                                        BEGIN
                                        CREATE TABLE FlashCards (
                                        ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
                                        Name varchar(255),
                                        Definition varchar(255),
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
    public static void ViewTable(SqlConnection connection,string tableName)
    {
        string sqlString = @$"SELECT * FROM {tableName};";
        SqlCommand command = new SqlCommand(sqlString, connection);
        
        connection.Open();
        var table = new Table();
        List<string[]> flashcardRow = new List<string[]>();
        string[][] stackRow = new string[3][];
        using(SqlDataReader reader = command.ExecuteReader())
        {
            switch (tableName)
            {
                case "FlashCards":
                    table.AddColumn("ID");
                    table.AddColumn("Name");
                    table.AddColumn("StackId");
                    table.AddColumn("Definition");
                    int rowCount = 0;
                    while (reader.Read())
                    {
                        flashcardRow.Add([reader[0].ToString(), reader[1].ToString(), reader[2].ToString()  , reader[3].ToString() ]);
                        table.AddRow([reader[0].ToString(), reader[1].ToString(), reader[2].ToString()  , reader[3].ToString() ]);
                    }
                    break;
                case "Stacks":
                    table.AddColumn("ID");
                    table.AddColumn("Name");
                    rowCount = 0;
                    while (reader.Read())
                    {
                        table.AddRow([reader[0].ToString(),reader[1].ToString()]);
                    }
                    break;
                default:
                    break;
            }
        }
        connection.Close();
        AnsiConsole.Write(table);        
    }
    public static void UpdateFlashCard(SqlConnection connection, int Id, string Name, String Definition, int StackID)
    {
        string sqlString = @"UPDATE FlashCards (Name, Definition, StackID)
                            VALUES (@Name, @Definition, @StackID) WHERE ID = @Id;";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Id", Id);
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Definition", Definition);
            command.Parameters.AddWithValue("StackID", StackID);
            command.ExecuteNonQuery();
        }
    }
    public static void UpdateStack()
    {

    }
    public static void DeleteFlashCard(SqlConnection connection, int ID)
    {
        string sqlString = @"DELETE FROM Flashcards WHERE ID = @ID;";
        connection.Open();
        using (SqlCommand command = new SqlCommand(sqlString,connection) )
        {
            command.Parameters.AddWithValue("ID", ID);
            command.ExecuteNonQuery();
        }
    }
    public static void DeleteStack()
    {

    }    
    public static void InsertFlashCard(SqlConnection connection, string Name, string Definition, int StackID)
    {
        string  sqlString = @"INSERT INTO FlashCards (Name, Definition, StackId) 
                            VALUES (@Name, @Definition, @StackId)";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Definition", Definition);
            command.Parameters.AddWithValue("StackId", StackID);
            command.ExecuteNonQuery();
        }
    }
    public static void InsertStack(SqlConnection connection, string Name)
    {
        String sqlString = @"INSERT INTO Stacks (Name)
                            VALUES (@Name);";
        connection.Open();
        SqlCommand command  =  new SqlCommand(sqlString,  connection);
        using (command)
        {
            command.Parameters.AddWithValue("Name",Name);
            command.ExecuteNonQuery();
        }
    }
    public static List<String> QueryStacks(SqlConnection Connection)
    {
        List<String> stacks = [];
        string stackString = @"SELECT Name FROM Stacks";
        Connection.Open();
        SqlCommand command = new SqlCommand(stackString, Connection);
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                stacks.Add(reader[0].ToString().ToUpper().Trim());
            }
        }
        Connection.Close();
        return stacks;
    }
    public static int QueryStackID(SqlConnection connection, string Name)
    {
        string sqlString = @"SELECT ID FROM Stacks WHERE Name = @Name;";
        SqlCommand command = new SqlCommand(sqlString, connection);
        command.Parameters.AddWithValue("Name", Name);
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader() )
        {
            while (reader.Read())
            {
                return (int)reader[0];
            }
        }
        return 0;
    }
}