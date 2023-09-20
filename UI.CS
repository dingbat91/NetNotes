
using System.Diagnostics;
using Microsoft.VisualBasic;
using NetNotes;
using Terminal.Gui;

/*
!! TO-DO LIST !!
 TODO: Load in notes.
 TODO: Allow swapping between Notes
*/


public static class NoteUI
{
    static readonly Terminal.Gui.Toplevel Top = Application.Top;
    internal static List<Note> notes = new List<Note>();
    public static void Initialize(List<Note> inputNotes)
    {
        if (inputNotes == null)
        {
            throw new ArgumentNullException(nameof(inputNotes));
        }

        notes = inputNotes;
        //Main Menu Code
        Terminal.Gui.MenuBar mainMenu = new MenuBar(new MenuBarItem[]{
            new MenuBarItem("_File",new MenuItem[]{
            new MenuItem("_New","Creates new note", null),
            new MenuItem("_Quit","Quits the Application",() => {if (Quit ()) Top.Running = false;})
            })
        });

        //Add Main Menu to Top Level UI
        Top.Add(mainMenu);

        //Initialize Note Browser - Default First View
        NoteBrowser.Init(notes);

        //Execute GUI
        Application.Run();

        //Shutdown if you've gotten here somehow! (Should've exited by menu call)
        Application.RequestStop();
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
    //View parameters
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
        Width = Dim.Percent(25),
        Height = Dim.Fill()
    };
    static internal Terminal.Gui.Window NView = new Window("Note View")
    {
        X = Pos.Right(NList),
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };
    static internal Terminal.Gui.TextView NText = new Terminal.Gui.TextView()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill(),
        ReadOnly = false
    };
    static internal Terminal.Gui.ListView list = new ListView()
    {
        X = 0,
        Y = 0,
        Width = Dim.Fill(),
        Height = Dim.Fill()
    };

    //Initialize Note Browser
    internal static void Init(List<Note> notes)
    {
        //Note List
        NBrowser.Add(NList);
        // Note View
        NBrowser.Add(NView);
        NView.Add(NText);
        Top.Add(NBrowser);

        // add in notes
        if (notes != null)
        {
            Update(notes);
        }
        else
        {
            // No Notes
            NList.Add(list);
        }

        //setup event handlers
        InitEHandlers();
    }


    private static void InitEHandlers()
    {
        //save Note text event handler
        NText.TextChanged += () =>
        {
            Save();
        };

        // switch note event handler
        list.SelectedItemChanged += (e) => SwitchNote(e);
    }

    //Update Note List
    internal static void Update(List<Note> notes)
    {
        //Don't do anything if No notes
        if (notes == null)
        {
            return;
        }

        //Reset Note List
        list.RemoveAll();
        var noteTitles = notes.Select(note => note.Title).ToList();
        list.SetSource(noteTitles);

        //reset Note List
        NList.Add(list);

        //set text view to first note
        NText.Text = notes[0].Body;
        list.SelectedItem = 0;

    }

    // save edited note
    private static void Save()
    {
        // get note
        var note = NoteUI.notes[list.SelectedItem];
        // update note
        note.Body = NText.Text.ToString();
        // update note list
        NoteUI.notes[list.SelectedItem] = note;
    }

    private static void SwitchNote(ListViewItemEventArgs e)
    {
        var note = NoteUI.notes[e.Item];
        NText.Text = note.Body;
    }
}
