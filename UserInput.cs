using System.Data.Common;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

class UserInput
{
    
    public static string MainMenu()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine( "0 Exit");
        Console.WriteLine( "1 Manage Stacks");
        Console.WriteLine( "2 Manage FlashCards");
        Console.WriteLine( "3 Study");
        Console.WriteLine( "4 View Study session data");
        Console.WriteLine("---------------------------");
       bool validInput = false; 
        while (!validInput)
        {
          validInput = true;
          string result = Console.ReadLine();
          switch (result)
          {
            case "0":
                return "0";
            case "1":
                return "1";
            case "2":
                return "2";
            case "3":
                return "3";
            case "4":
                return "4";
            default:
                validInput = false;
                Console.WriteLine("Invalid input. Please retry.");
                break;
          }
        }
        return "0";
    }
    public static  void StackMenu()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("0 to return to main menu");
        Console.WriteLine("X to change current stack");
        Console.WriteLine("V to view all Flashcards in stack");
        Console.WriteLine("A to  view X amount of cards in stack");
        Console.WriteLine("C to Create a Flashcard in current stack");
        Console.WriteLine("E to Edit a Flashcard");
        Console.WriteLine("D to Delete a Flashcard");
        Console.WriteLine("---------------------------");
    }
    public  static void FlashCardMenu()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("V to view all Flashcard");
        Console.WriteLine("C to Create a Flashcard");
        Console.WriteLine("E to Edit a Flashcard");
        Console.WriteLine("D to Delete a Flashcard");
        Console.WriteLine("---------------------------");

        string result = Console.ReadLine().ToUpper().Trim();
        switch (result)
        {
            case "V":
                DBController.ViewTable(DBController.ConnectDB(), "FlashCards");
                Console.WriteLine();
                DBController.ViewTable(DBController.ConnectDB(), "Stacks");
                break;
            case "C":
                CreateFlashCard();
                break;
            case "E":
                UpdateFlashCard();
                break;
            case "D":
                DeleteFlashCard();
                break;
            default:
                break;
        }
    }
    public  static void StudyMenu()
    {
        
    }
    public static string[] CreateFlashCard()
    {
        string?flashcardName;
        string?definition;
        string?stackName;
        Console.WriteLine("Enter the name of the Flashcard");
        flashcardName = Console.ReadLine();
        Console.WriteLine("Enter the definition of the Flashcard");
        definition = Console.ReadLine();
        Console.WriteLine("Enter the stack name this Flashcard  belongs to.");
        stackName = Console.ReadLine();
        while(!Validation.StackExists(stackName))
        {
            Console.WriteLine("Stack does not exists. Would you like to create this stack?(Y/N)");
            string result = Console.ReadLine().ToUpper().Trim();
            switch (result)
            {
                case "Y":
                    DBController.InsertStack(DBController.ConnectDB(),stackName);
                    break;
                default:
                    Console.WriteLine("Enter the stack name this Flashcard  belongs to.");
                    stackName = Console.ReadLine();
                    break;
            }
        }
        int stackID = DBController.QueryStackID(DBController.ConnectDB(),stackName);
        DBController.InsertFlashCard(DBController.ConnectDB(),flashcardName,definition,stackID);
        return [flashcardName, definition, stackName];
    }
    public static void UpdateFlashCard()
    {
        //view all flash cards

        Console.WriteLine("Enter the ID of the Flashcard you want to edit");
        int flashcardIDInt = 0;
        while (true)
        {
            string flashcardID = Console.ReadLine();
            bool validID = int.TryParse(flashcardID, out flashcardIDInt);
            if (validID)
            {
                break;
            }
        }
        Console.WriteLine("Enter the new Flashcard name");
        string name = Console.ReadLine();
        Console.WriteLine("Enter the new Flashcard definition");
        string definition = Console.ReadLine();
        Console.WriteLine("Enter the new stack");
        string stackName = "";
        SqlConnection connection = DBController.ConnectDB();
        while(true)
        {
            stackName = Console.ReadLine();
            if (Validation.StackExists(stackName))
            {
                break;
            }
            else
            {
                Console.WriteLine("This stack does not exist. Do you want to create this?");
                {
                    if (Console.ReadLine().ToUpper().Trim()=="Y")
                    {
                        DBController.InsertStack(connection, stackName);
                    }
                    else
                        Console.WriteLine("Please enter a valid stack.");
                }
            }
        }
        int stackID = DBController.QueryStackID(connection,stackName);
        DBController.UpdateFlashCard(connection,flashcardIDInt,name,definition, stackID);
    }
    public static void DeleteFlashCard()
    {
        //  view all flash cards
        int flashcardID = 0;
        while (true)
        {
            Console.WriteLine("Enter the ID of the flashcard to delete");
            bool validInt = int.TryParse(Console.ReadLine(), out flashcardID);
            if (validInt)
                {
                    break;
                }
        }
        DBController.DeleteFlashCard(DBController.ConnectDB(), flashcardID);
    }
}