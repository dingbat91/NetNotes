
using System.Diagnostics;
using Microsoft.VisualBasic;

using Terminal.Gui;

/*
!! TO-DO LIST !!
    TODO: note DB/File Loading
    TODO: Tags
*/
namespace NetNotes
{
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
            NoteBrowser.Init(ref notes);

            //Execute GUI
            Application.Run();


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

        static internal Terminal.Gui.Button saveButton = new Button()
        {
            Text = "Save",
            X = Pos.Left(NView) + 1,
            Y = Pos.Bottom(NView) - 1,
            ColorScheme = Colors.Menu
        };

        //Initialize Note Browser
        internal static void Init(ref List<Note> extnotes)
        {
            //Note List
            NBrowser.Add(NList);
            // Note View
            NBrowser.Add(NView);
            NBrowser.Add(saveButton);
            NView.Add(NText);
            Top.Add(NBrowser);

            // add in notes
            if (extnotes != null)
            {
                Update(extnotes);
            }
            else
            {
                // No Notes
                NList.Add(list);
            }

            //setup event handlers
            InitEHandlers();
        }

        //Register Event Handlers
        private static void InitEHandlers()
        {
            //save Note text event handler.
            saveButton.Clicked += () =>
            {
                if (NText.Text != null)
                {
                    Save(ref NoteUI.notes, NText.Text.ToString());
                }
                else
                {
                    throw new ArgumentNullException(nameof(NText));
                }
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

        // Event Handler for saving notes
        private static void Save(ref List<Note> notes, string newtext)
        {
            NText.ReadOnly = true;
            var note = NoteUI.notes[list.SelectedItem];
            note.Body = newtext;
            NoteUI.notes[list.SelectedItem] = note;
            NoteDatabase.Update(note);
            NText.ReadOnly = false;
        }

        //Event Handler for switching notes
        private static void SwitchNote(ListViewItemEventArgs e)
        {
            var note = NoteUI.notes[e.Item];
            //Disable Editing during change
            NText.ReadOnly = true;
            //Clear old history
            NText.ClearHistoryChanges();
            NText.Text = note.Body;
            //Re-enable Editing
            NText.ReadOnly = false;

        }
    }
}