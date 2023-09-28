using System.CodeDom;
using System.Data.Common;
using LiteDB;

namespace NetNotes
{


    // Class for managing liteDB database for Long Term Note Storage
    static class NoteDatabase
    {
        static LiteDatabase db = new LiteDatabase("notes.db");
        static ILiteCollection<Note>? notes;

        //Initialize Database
        public static void init()
        {

            if (notes == null)
            {
                notes = db.GetCollection<Note>("notes");
            }

        }

        //CRUD Operations----------------------------------------------
        public static void Add(Note note)
        {

            notes?.Insert(note);
        }

        public static void Remove(Note note)
        {

            notes?.Delete(note.NoteID);
        }

        public static void Update(Note note)
        {

            notes?.Update(note);
        }

        public static Note? Get(string id)
        {

            return notes?.FindById(id);
        }

        public static Note[]? GetAll()
        {
            return notes?.FindAll().ToArray();
        }
        //----------------------------------------------------------------

        //Clear Database
        public static void Clear()
        {
            notes?.DeleteAll();
        }

    }
}
