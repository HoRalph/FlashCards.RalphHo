using Microsoft.Data.SqlClient;

public static class Logic
{
    //Create Flash Card
    public static FlashCardModel CreateFlashCard(string Name, string Definition, int Id = 0, int StackId = 0)
    {
        FlashCardModel FlashCard = new FlashCardModel();
        FlashCard.Name = Name;
        FlashCard.Definition = Definition;
        FlashCard.Id = Id;
        FlashCard.StackId = StackId;
        
        return  FlashCard;
    }
    public static FlashCardModel SaveFlashCard(FlashCardModel model)
    {
        //int stackID = DBController.QueryStackID(DBController.ConnectDB(),model.Name);
        DBController.InsertFlashCard(DBController.ConnectDB(),model.Name,model.Definition,model.StackId);
        return model;
    }



    //Delete Flash Cards
    public static void DeleteFlashCard(int Id)
    {
        
        return;
    }
    //update FlashCard
        public static FlashCardModel UpdateFlashCard(string Name, string Definition, int Id)
        {
            //Get FlashCard from DB

            //Set FlashCard
            FlashCardModel FlashCard = new FlashCardModel();
            FlashCard.Name = Name;
            FlashCard.Definition = Definition;
            FlashCard.Id = Id;

            //Save Flash Card to db
            return  FlashCard;
        }




    //update FlashCard
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
            if (Logic.StackExists(stackName))
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
    
    //Delete FlashCard
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
    //Get all FlashCards

    //Get all Stacks

    

    

    //Check if FlashCard Exists
    public static bool StackExists(string Stack)
    {
        //check if stack is in the stack table
        //return false if not
        List<string> stackList  = DBController.QueryStacks(DBController.ConnectDB());
        if (stackList.Contains(Stack.ToUpper().Trim()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //Check if Stack Exists
    
}