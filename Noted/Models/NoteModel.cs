using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSoft
{
    public class NoteModel
    {
       //create attributes
      public int Id { get; set; }
      public string Body { get; set; }
      public string Header { get; set; }
      public DateTime NewNote { get; set; }
      public DateTime EditDate { get; set; }
      public DateTime Reminder  { get; set; }
      public int EditedByUserId { get; set; }
      public int CreatedByUserId { get; set; }
      public bool State { get; set; } = true;
       

        

       




    }
}
