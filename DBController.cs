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
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using Spectre;
using Spectre.Console;
using Spectre.Console.Cli;

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
    public static void UpdateFlashCard(SqlConnection connection, int Id, string Name, string Definition, int StackID)
    {
        string sqlString = @"UPDATE FlashCards SET Name = @Name, Definition = @Definition, StackID = @StackID
                            WHERE ID = @Id;";
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
        connection.Close();
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
        connection.Close();
    }
    public static void DeleteFlashCardStacks(SqlConnection connection, int id)
    {
        string  sqlString = @"DEELETE FROM dbo.FlashCards
                            WHERE StackId = @Id;";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Id", id);
            command.ExecuteNonQuery();
            connection.Close();
            return;
        }
    }
    public static void DeleteStack(SqlConnection connection, string Name)
    {
        //Get stack ID
        int stackId = QueryStackID(connection,Name);
        //Delete all flashcards with the stackID
        DeleteFlashCardStacks(connection,stackId);
        //Delete the stack from stack table
        DeleteStack(connection,Name);
    }    
    public static int InsertFlashCard(SqlConnection connection, string Name, string Definition, int StackID)
    {
        string  sqlString = @"INSERT INTO FlashCards (Name, Definition, StackId) 
                            VALUES (@Name, @Definition, @StackId); SELECT SCOPE_IDENTITY() AS INT";
        connection.Open();
        SqlCommand command = new SqlCommand(sqlString, connection);
        using (command)
        {
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Definition", Definition);
            command.Parameters.AddWithValue("StackId", StackID);
            
            int output = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return output;
        }
        
    }
    public static int InsertStack(SqlConnection connection, string Name)
    {
        String sqlString = @"INSERT INTO Stacks (Name)
                            VALUES (@Name); SELECT SCOPE_IDENTITY() AS INT";
        connection.Open();
        SqlCommand command  =  new SqlCommand(sqlString,  connection);
        using (command)
        {
            command.Parameters.AddWithValue("Name",Name);
            int output = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            return output;
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
                int output = (int)reader[0];
                connection.Close();
                return output;
            }
        }
        connection.Close();
        return 0;
    }
    public static void ViewStacks(List<string> StackList)
    {
        Console.Clear();
        Table table = new Table();
        table.AddColumn("Stack");
        foreach (string stack in StackList)
        {
            table.AddRow(stack);
        }
        AnsiConsole.Write(table);
    }
    public static void ViewFlashcardsInStack(SqlConnection Connection, string Stack)
    {
        string sqlString = @"SELECT FlashCards.ID, FlashCards.Name, FlashCards.Definition, Stacks.Name
                            FROM FlashCards
                             LEFT JOIN Stacks ON FlashCards.StackID = Stacks.ID 
                             WHERE Stacks.Name = @Stack;";
        Connection.Open();
        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Definition");
        table.AddColumn("Stack Name");
        using(SqlCommand command = new SqlCommand(sqlString, Connection))
        {
            command.Parameters.AddWithValue("Stack", Stack);
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                table.AddRow([reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString()]);
            }
        }
        Connection.Close();
        AnsiConsole.Write(table);
    }
    public static void XFlashcardsInStack(SqlConnection connection, string Stack, int cardNumber)
    {
        Random rand = new Random();
        int i =0;
        string idList = "";
        int flashcardCount = countFlashCards(connection,Stack);
        for (i = 1; i<=cardNumber;i++)
        {
            int j = rand.Next(1,flashcardCount+1);
            idList = idList +  j.ToString();
            if (i < cardNumber )
            {
                idList +=", ";
            }
        }
        idList = $"({idList})"; 
        string sqlString = @$"SELECT FlashCards.ID, FlashCards.Name, FlashCards.Definition FROM Flashcards LEFT JOIN Stacks ON FlashCards.StackID = Stacks.ID WHERE Stacks.Name = @Stack AND Flashcards.ID IN {idList};";
        SqlCommand command = new SqlCommand(sqlString,connection);
        connection.Open();
        command.Parameters.AddWithValue("Stack", Stack);
        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Name");
        table.AddColumn("Definition");
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                table.AddRow([reader[0].ToString(), reader[1].ToString(), reader[2].ToString()]);
            }
        }
        connection.Close();
        AnsiConsole.Write(table);
    }
    public static int countFlashCards(SqlConnection connection, string Stack)
    {
        string sqlString = @"SELECT Count(FlashCards.ID) FROM Flashcards LEFT JOIN STACKS ON FlashCards.StackID = Stacks.ID WHERE Stacks.Name = @Stack;";
        SqlCommand command = new SqlCommand(sqlString, connection);
        connection.Open();
        command.Parameters.AddWithValue("Stack", Stack);
        using(SqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                try
                {
                    int count = int.Parse(reader[0].ToString());
                    connection.Close();
                    return count;
                }
                catch
                {
                    connection.Close();
                    return 0;
                }
            }
            connection.Close();
            return 0;
        }
        
    }
}