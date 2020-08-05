using NoteSoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noted.Controllers
{
    public class LoginController : Controller
    {

        // First view when open. Shows the Login form.
        public ActionResult Index(string message)
        {
            //When session it's already open, redirects to list of notes.
            if (Session["UserId"] == null || Convert.ToInt32(Session["UserId"]) == 0)
            {
                ViewBag.Message = message;

                return View();
            } else
            {
                int id = Convert.ToInt32(Session["UserId"]);
                return RedirectToAction("GetNoteByUserId", "Note", new { id });
            }//end if else
            
        }//end Index

        //Receive data from the form to login and make it happens.
        public ActionResult Logged(FormCollection form)
        {

            //Receive and store data from form.
            string user = form["UserTxt"];
            string pass = form["PasswordTxt"];
            
            
            
            DataSet ds = new DataSet();
            Dictionary<string, object> params_values = new Dictionary<string, object>();

            SqlDataAccess da = new SqlDataAccess();
            params_values.Add("@Mail", user.ToString());
            params_values.Add("@Password", pass.ToString());
            ds = da.ExecuteQueryParameterized("SELECT * FROM [User] WHERE Mail=@Mail AND Password=@Password", params_values);

            UserModel user1 = new UserModel();
            
            //Get data from DB
             foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    user1.Name = dr["Name"].ToString();
                    user1.LastName = dr["LastName"].ToString();
                    user1.Id = int.Parse(dr["IdUsr"].ToString());
                    user1.Mail = dr["Mail"].ToString();



                }// end for

            
             //Check correct Login
            if (user1.Id != 0)
            {
                Session["UserId"] = user1.Id;
                Session["UserName"] = user1.Name;
                return RedirectToAction("GetNoteByUserId", "Note", new { user1.Id });
                
            }
            else {
                return RedirectToAction("Index", "Login", new { Message = "Incorrect User or password"});
            }//endif
                                   
        }//end Logged

       

    }//end Class
}//end Namespace