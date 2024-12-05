using System.Collections;
using System.Data.Common;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

class UserInput
{
    
    public static void MainMenu()
    {
       bool validInput = false; 
        while (true)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine( "0 Exit");
            Console.WriteLine( "1 Manage Stacks");
            Console.WriteLine( "2 Manage FlashCards");
            Console.WriteLine( "3 Study");
            Console.WriteLine( "4 View Study session data");
            Console.WriteLine("---------------------------");          
            validInput = true;
            string result = Console.ReadLine();
            string selectedStack ="";
            switch (result)
          {
            case "0":
                return;
            case "1":
                DBController.ViewStacks(DBController.GetStacks(DBController.ConnectDB()));
                while(true)
                {
                    Console.WriteLine("Enter the stack name to select");
                    selectedStack = Console.ReadLine();
                    if (Logic.StackExists(selectedStack))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Stack. PLease reenter.");
                    }
                }
                UserInput.StackMenu(selectedStack);  
                break;
            
            case "2":
                UserInput.FlashCardMenu();
                break;
            case "3"://study session
                Console.Clear();
                DBController.ViewStacks(DBController.GetStacks(DBController.ConnectDB()));
                Console.WriteLine();
                Console.WriteLine("Enter a Stack to study.");
                string stack = "";
                while(true)
                {
                    stack = Console.ReadLine();
                    if (Logic.StackExists(stack))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Stack. Please retry.");
                    }
                }
                

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
        Console.WriteLine($"------ Current Stack: {Stack} ------");
        Console.WriteLine("0 to return to main menu");
        Console.WriteLine("X to change current stack");
        Console.WriteLine("V to view all Flashcards in stack");
        Console.WriteLine("A to  view X amount of cards in stack");
        Console.WriteLine("C to Create a Flashcard in current stack");
        Console.WriteLine("E to Edit a Flashcard");
        Console.WriteLine("D to Delete a Flashcard");
        Console.WriteLine("Z to Delete this Stack");
        
        string result = Console.ReadLine().ToUpper().Trim();
        SqlConnection connection  = DBController.ConnectDB();
        switch (result)
        {
            case "0":
                MainMenu();
                break;
            case "X":
                DBController.ViewStacks(DBController.GetStacks(connection));
                while(true)
                {
                    Console.WriteLine("Please enter the new stack name to select.");
                    string stack = Console.ReadLine();
                    if (Logic.StackExists(stack))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid stack please try again");
                    }
                }
                StackMenu(Stack);
                break;
            case "V":
                DBController.ViewFlashcardsInStack(connection, Stack);
                break;
            case "A":
                Console.WriteLine("Please enter the amount of cards you want to view.");
                int cardNumber = 0;
                connection = DBController.ConnectDB();
                while(true)
                {
                    if(int.TryParse(Console.ReadLine(), out cardNumber))
                    {
                        if (cardNumber > DBController.countFlashCards(connection, Stack))
                        {
                            Console.WriteLine("Number entered is greater than the number of flashcards in this Stack. Please retry.");
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid number. Please retry.");
                    }
                }
                DBController.XFlashcardsInStack(connection, Stack, cardNumber);
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
                definition = Console.ReadLine();
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
            
            case "Z":
                Console.WriteLine($"{ Stack } stack is now deleted");
                Console.WriteLine("All Flashcards in this stack are deleted");
                Console.WriteLine("Please Confirm (Y/N)");
                result = Console.ReadLine().ToUpper().Trim();

                if (result == "Y")
                {
                    DBController.DeleteStack(connection, Stack);
                    Console.WriteLine("Stack its Flash cards deleted.");
                }
                else
                {
                    Console.WriteLine("Aborted Delete Stack.");
                }

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
                Console.Clear();
                DBController.ViewTable(DBController.ConnectDB(), "FlashCards");
                Console.WriteLine();
                DBController.ViewTable(DBController.ConnectDB(), "Stacks");
                Console.ReadLine();
                break;
            case "C":
                Console.Clear();
                //ask for Name, Definition, Stack
                string?flashcardName;
                string?definition;
                string?stackName;
                int stackID = 0;
                Console.WriteLine("Enter the name of the Flashcard");
                flashcardName = Console.ReadLine();
                Console.WriteLine("Enter the definition of the Flashcard");
                definition = Console.ReadLine();
                Console.WriteLine("Enter the stack name this Flashcard  belongs to.");
                stackName = Console.ReadLine();
                //validate inputs
                
                //Check if Stack Exists If not save the stack
                if (!Logic.StackExists(stackName))
                {
                    //if not exists save stack return stack ID
                    Console.WriteLine("Stack does not exists. Create Stack? (Y/N)");

                    if (Console.ReadLine().ToUpper().Trim() == "Y")
                    {
                        DBController.InsertStack(DBController.ConnectDB(),stackName);
                        stackID = DBController.QueryStackID(DBController.ConnectDB(),stackName);
                    }

                    else 
                        return;
                }
                
                else
                {
                    //if exists get stack ID
                    stackID = DBController.QueryStackID(DBController.ConnectDB(),stackName);
                }

                //Create Flash card
                FlashCardModel flashCard = Logic.CreateFlashCard(flashcardName, definition,StackId:stackID);
                //save to flash card table
                Logic.SaveFlashCard(flashCard);
                break;

            case "E":
                Logic.UpdateFlashCard();
                break;
            case "D":
                Logic.DeleteFlashCard();
                break;
            default:
                break;
        }
    }
    public  static void StudyMenu()
    {
        
    }

}