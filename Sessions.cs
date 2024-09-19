using System;
class Sessions
{
    private string _date = DateTime.Now.ToString("MM/DD/yyyy h:mm:ss");
    public string Date
    {
        get{return _date;}
        set{_date = value;}
    }

}