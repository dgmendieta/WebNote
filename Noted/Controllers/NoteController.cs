using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NoteSoft;


namespace Noted.Controllers
{
    public class NoteController : Controller
    {
        //  Note
        public ActionResult AddNote(FormCollection form)
        {
            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0)
            {

                NoteModel note1 = new NoteModel();
                note1.Header = form["HeaderTxt"];
                note1.Body = form["BodyTxt"];
                note1.NewNote = DateTime.Now;
                note1.Reminder = Convert.ToDateTime(form["ReminderTxt"]);
                note1.CreatedByUserId = Convert.ToInt32(Session["UserId"]);
                note1.State = true;


                SqlDataAccess da = new SqlDataAccess();

                Dictionary<string, object> params_values = new Dictionary<string, object>();
                //stores data in DB
                params_values.Add("@Header", note1.Header);
                params_values.Add("@Body", note1.Body);
                params_values.Add("@CreatedByUserId", note1.CreatedByUserId);
                params_values.Add("@Date", note1.NewNote);
                params_values.Add("@Reminder", note1.Reminder);
                params_values.Add("@Active", note1.State);
                bool success = da.ExecuteSentenceParameterized("INSERT INTO Note(Header, Body, CreatedByUserId, NewNote, Reminder,Active) VALUES(@Header, @Body, @CreatedByUserId, @Date, @Reminder,@Active)", params_values);


                if (success)
                {
                    return RedirectToAction("AddNoteForm", "Note", new { message = "Note successfully created" });
                }
                else
                {
                    return RedirectToAction("AddNoteForm", "Note", new { message = "Something went wrong" });
                }//end if else

            }
            else
            {
                return RedirectToAction("Index", "Login", new { message = "What are you trying to do??" });

            }// end if else

           
        }

       
        public ActionResult AddNoteForm(string message)
        {

            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0)
            {

                ViewBag.Message = message;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { message = "What are you trying to do??" });

            }//end if else
            
        }


        public ActionResult GetNote()
        {
            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0)
            {

                List<NoteModel> result = new List<NoteModel>();

                DataSet ds = new DataSet();
                Dictionary<string, object> params_values = new Dictionary<string, object>();

                SqlDataAccess da = new SqlDataAccess();
                ds = da.ExecuteQueryParameterized("SELECT * FROM Note", params_values);

                //loads data from DB
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NoteModel note1 = new NoteModel();
                    note1.Header = dr["Header"].ToString();
                    note1.Body = dr["Body"].ToString();
                    note1.CreatedByUserId = int.Parse(dr["CreatedByUserId"].ToString());
                    note1.NewNote = DateTime.Parse(dr["NewNote"].ToString());
                    note1.Reminder = DateTime.Parse(dr["Reminder"].ToString());

                    result.Add(note1);
                }// end for

                return View(result);

            }
            else
            {
                return RedirectToAction("Index", "Login", new { message = "What are you trying to do??" });

            }//end if else
            


        }

        public ActionResult GetNoteByUserId()
        {
            //receive id to call and edit note
            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0)
            {
                int id = Convert.ToInt32(Session["UserId"]);

                List<NoteModel> result = new List<NoteModel>();

                DataSet ds = new DataSet();
                Dictionary<string, object> params_values = new Dictionary<string, object>();

                SqlDataAccess da = new SqlDataAccess();
                ds = da.ExecuteQueryParameterized("SELECT * FROM Note,[User] WHERE CreatedByUserId=IdUsr AND IdUsr= " + id, params_values);

                //load data from DB
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NoteModel note1 = new NoteModel();
                    note1.Id = Convert.ToInt32(dr["Id"]);
                    note1.Header = dr["Header"].ToString();
                    note1.Body = dr["Body"].ToString();
                    note1.CreatedByUserId = int.Parse(dr["CreatedByUserId"].ToString());
                    note1.NewNote = DateTime.Parse(dr["NewNote"].ToString());
                    note1.Reminder = DateTime.Parse(dr["Reminder"].ToString());

                    result.Add(note1);

                }//end for
                return View(result);
            } else
            {
                return RedirectToAction("Index", "Login", new { message = "What are you trying to do??" });
            }
            
            

        }

        public ActionResult EditNote(FormCollection form)
        {
            

            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0)
            {
                NoteModel note1 = new NoteModel();
                note1.Header = form["HeaderTxt"];
                note1.Body = form["BodyTxt"];
                note1.EditDate = DateTime.Now;
                note1.Reminder = Convert.ToDateTime(form["ReminderTxt"]);
                note1.EditedByUserId = Convert.ToInt32(Session["UserId"]);



                SqlDataAccess da = new SqlDataAccess();

                Dictionary<string, object> params_values = new Dictionary<string, object>();
                //stores data in DB
                params_values.Add("@Header", note1.Header);
                params_values.Add("@Body", note1.Body);
                params_values.Add("@EditedByUserId", note1.EditedByUserId);
                params_values.Add("@EditDate", note1.EditDate);
                params_values.Add("@Reminder", note1.Reminder);

                bool success = da.ExecuteSentenceParameterized("UPDATE Note SET Header=@Header, Body=@Body, EditedByUserId=@EditedByUserId, EditDate=@EditDate, Reminder=@Reminder WHERE Id=" + note1.Id, params_values);


                if (success)
                {
                    return RedirectToAction("GetNoteByUserId", "Note", new { message = "Note successfully updated" });
                }
                else
                {
                    return RedirectToAction("GetNoteByUserId", "Note", new { message = "Something went wrong" });
                }//end if else

            }
            else
            {
                return RedirectToAction("Index", "Login", new { message = "What are you trying to do??" });

            }//end if else

            
        }

       

    }
}
