namespace NetNotes;


//Note Class - used to manage a single Note
public class Note
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Date { get; set; }
    public string[]? Tags { get; set; }

    public Note(string title, string body, string[]? tags = null)
    {
        Title = title;
        Body = body;
        Date = DateTime.Now;
        Tags = tags;
    }

    public interface IListDataSource : Terminal.Gui.IListDataSource
    {

    }
}
