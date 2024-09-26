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
            Console.WriteLine("Enter the stack name");
            selectedStack = Console.ReadLine();
            DBController.QueryStacks(DBController.ConnectDB());
            UserInput.StackMenu(selectedStack);        
            break;
        case "2":
            UserInput.FlashCardMenu();
            break;
    }
    Console.ReadLine();
    }
}


