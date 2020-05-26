using SqliteLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqliteLib
{
   [Serializable]
    public class ListOfNotes
    {
        public Note[] notes {get; set;}
        
        public ListOfNotes()
        {
            notes = new Note[10];
        } 
    }
}
