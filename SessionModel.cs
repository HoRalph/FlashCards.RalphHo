using System;
class SessionModel
{
    public int Id {get;set;}
    private string _date = DateTime.Now.ToString("MM/DD/yyyy h:mm:ss");
    public string Date
    {
        get{return _date;}
        set{_date = value;}
    }

    public int StackId {get; set;}
    public StackModel Stack{get;set;}

    public List<FlashCardModel> FlashCards {get;set;}
    public  float Score{get;set;}

}