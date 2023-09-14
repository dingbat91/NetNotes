
using NetNotes;
using Terminal.Gui;

public static class NoteUI
{
    static readonly Terminal.Gui.Toplevel Top = Application.Top;
    internal static Note[]? notes = null;
    public static void Initialize(Note[]? notes = null)
    {
        //Initialize GUI
        Application.Init();

        //Set Notes
        if (notes != null)
        {
            NoteUI.notes = notes;
        }
        else
        {
            var testNote = new Note("Intro Note", "You have no notes, so this is a default note to display in the UI!");
            NoteUI.notes = new Note[] { testNote };
        }

        //Main Menu Code
        var mainMenu = new MenuBar(new MenuBarItem[]{
            new MenuBarItem("_File",new MenuItem[]{
            new MenuItem("_New","Creates new note", null),
            new MenuItem("_Quit","Quits the Application",() => {if (Quit ()) Top.Running = false;})
            })
        });

        //Add Main Menu to Top Level UI
        Top.Add(mainMenu);

        //Initialize Note Browser - Default First View
        NoteBrowser.init(NoteUI.notes);

        //Execute GUI
        Application.Run();

        //Shutdown if you've gotten here somehow! (Should've exited by menu call)
        Application.Shutdown();
    }

    // Exit Prompt
    static bool Quit()
    {
        var n = MessageBox.Query(50, 7, "Quit NetNote", "Are you sure you want to quit this app?", "Yes", "No");
        return n == 0;
    }
}

//Note Browser UI
internal static class NoteBrowser
{
    static readonly Terminal.Gui.Toplevel Top = Application.Top;
    static internal Terminal.Gui.Window NBrowser = new Window("Note Browser")
    {
        X = 0,
        Y = 1,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    static internal Terminal.Gui.Window NList = new Window("Notes")
    {
        X = 0,
        Y = 0,
        Width = Dim.Percent(10),
        Height = Dim.Fill()
    };
    static Terminal.Gui.Window NView = new Window("Note View")
    {
        X = Pos.Right(NList),
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    internal static void init(Note[]? notes = null)
    {
        //Note List
        NBrowser.Add(NList);
        // Note View
        NBrowser.Add(NView);
        Top.Add(NBrowser);

    }

    internal static void update(Note[] notes)
    {
        //Don't do anything if No notes
        if (notes == null)
        {
            return;
        }

        //Clear Note List
        var list = new ListView(notes)
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
    }
}
