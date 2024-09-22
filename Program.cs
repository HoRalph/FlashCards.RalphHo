using Microsoft.Data.SqlClient;
namespace Program;
class Program
{
    public static void Main (string[] args)
    {
//Menu
    DBController.CreateTables(DBController.ConnectDB());
    string  input ="";
    input = UserInput.MainMenu();
    string selectedStack ="";
    switch (input)
    {
        case "0":
            return;
        case "1":
            selectedStack = Console.ReadLine();
            //validateStack
            //if stack does not exists ask if you want to create.
            DBController.QueryStacks(DBController.ConnectDB());
            UserInput.StackMenu();        
            break;
        case "2":
            UserInput.FlashCardMenu();
            break;
    }
    Console.ReadLine();
    }
}


