using NetNotes;

internal class Program
{

    //Note Array, should be passed as ref at most times
    internal static List<Note>? notes;

    private static void Main(string[] args)
    {
        //Set Notes

        if (notes == null)
        {
            var testNote = new Note("Intro Note", "You have no notes, so this is a default note to display in the UI!");
            var testnote2 = new Note("Second Note", "This is a second note to display in the UI!");
            notes = new List<Note>();
            notes.Add(testNote);
            notes.Add(testnote2);
        }

        //Initialize UI
        NoteUI.Initialize(ref notes);
    }
}
