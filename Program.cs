using Microsoft.Data.SqlClient;
namespace Program;
class Program
{
    public static void Main (string[] args)
    {
//Menu
    UserInput.MainMenu();
    DBController.CreateTables(DBController.ConnectDB());
    Console.ReadLine();


/*
add flash cards
create  a stack and add flahcards to stack
    each flashcard needs to be assigned to a stack

study session area:
    pick a stack
    sessions  need to be stored, data, score
    if a stack is deleted the session is deleted.
    need function to view all sesions

Tables:
1) flashcards
    1)ID
    2)word
    3)definition
    4)stack

2) stacks
    1) ID
    2) stack name

3) sessions
    1) ID
    2) Date
    3) stack
    4) score
*id stack is deleted the Flashcards are deleted
**if stack is deleted the session is deleted


*/
    }
}


