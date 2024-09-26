using System.Collections;
using System.Data.Common;
using Microsoft.Data.SqlClient;

class UserInput
{
    
    public static void MainMenu()
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
          string selectedStack ="";
          switch (result)
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
            case "3":
                break;
            case "4":
                break;
            default:
                validInput = false;
                Console.WriteLine("Invalid input. Please retry.");
                break;
          }
        }
        return;
    }
    public static  void StackMenu(string Stack)
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine("0 to return to main menu");
        Console.WriteLine("X to change current stack");
        Console.WriteLine("V to view all Flashcards in stack");
        Console.WriteLine("A to  view X amount of cards in stack");
        Console.WriteLine("C to Create a Flashcard in current stack");
        Console.WriteLine("E to Edit a Flashcard");
        Console.WriteLine("D to Delete a Flashcard");
        
        string result = Console.ReadLine().ToUpper().Trim();
        SqlConnection connection  = DBController.ConnectDB();
        switch (result)
        {
            case "0":
                MainMenu();
                break;
            case "X":
                Console.WriteLine("Please enter the new stack name");
                StackMenu(Console.ReadLine());
                break;
            case "V":
                DBController.ViewFlashcardsInStack(connection, Stack);
                break;
            case "A":
                
                break;
            case "C":
                string?flashcardName;
                string?definition;
                Console.WriteLine("Enter the name of the Flashcard");
                flashcardName = Console.ReadLine();
                Console.WriteLine("Enter the definition of the Flashcard");
                definition = Console.ReadLine();
                int stackID = DBController.QueryStackID(connection,Stack);
                DBController.InsertFlashCard(DBController.ConnectDB(),flashcardName,definition,stackID);
                break;
            case "E":
                DBController.ViewFlashcardsInStack(connection, Stack);
                Console.WriteLine("Enter the ID of the flashcard to edit.");
                int ID = 0;
                while(true)
                {
                    string inputID = Console.ReadLine();
                    if (int.TryParse(inputID, out ID))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format. Please renter.");
                    }
                }
                Console.WriteLine("Enter the new Flashcard name");
                string name = Console.ReadLine();
                Console.WriteLine("Enter the new Flashcard definition");
                string definition = Console.ReadLine();
                DBController.UpdateFlashCard(connection, ID, name, definition,DBController.QueryStackID(connection,Stack));
                break;
            case "D":
                DBController.ViewFlashcardsInStack(connection, Stack);
                Console.WriteLine("Enter the ID of the flashcard to delete.");
                while(true)
                {
                    string inputID = Console.ReadLine();
                    if (int.TryParse(inputID, out ID))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID format. Please renter.");
                    }
                }
                DBController.DeleteFlashCard(connection, ID);
                break;
            default:
                break;
        }
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
        DBController.ViewTable(DBController.ConnectDB(), "FlashCards");
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
        DBController.ViewTable(DBController.ConnectDB(), "FlashCards");
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