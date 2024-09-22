public class Validation
{
    public static bool StackExists(string Stack)
    {
        //check if stack is in the stack table
        //return false if not
        List<string> stackList  = DBController.QueryStacks(DBController.ConnectDB());
        if (stackList.Contains(Stack.ToUpper().Trim()))
        {
            return true;
            Console.WriteLine("True");
        }
        else
        {
            return false;
            Console.WriteLine("False");
        }
    }
}