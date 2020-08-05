using NoteSoft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Noted.Controllers
{
    public class UserController : Controller
    {
        public ActionResult GetUser()
        {

            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0)
            {
                List<UserModel> result = new List<UserModel>();

                DataSet ds = new DataSet();
                Dictionary<string, object> params_values = new Dictionary<string, object>();

                SqlDataAccess da = new SqlDataAccess();
                ds = da.ExecuteQueryParameterized("SELECT * FROM [User]", params_values);
                UserModel user1 = new UserModel();
                //loads user data from DB
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    user1.Name = dr["Name"].ToString();
                    user1.LastName = dr["LastName"].ToString();
                    user1.Password = dr["Password"].ToString();
                    user1.Mail = dr["Mail"].ToString();
                    user1.Cellphone = Convert.ToInt32(dr["Cellphone"]);
                    result.Add(user1);
                } //end for
                return View(user1);
            }

            else
            {
                return RedirectToAction("Index", "Login", new { message = "What are you trying to do??" });

            }//end if else

            
        }


        public ActionResult NewUser()
        {
                       
             return View();


        }

        public ActionResult SaveUser(FormCollection form)
        {
            SqlDataAccess da = new SqlDataAccess();

            Dictionary<string, object> params_values = new Dictionary<string, object>();

            
            
            //create new user from data collected from form
            UserModel user1 = new UserModel();

            user1.Name = form["NameTxt"];
            user1.LastName = form["LastNameTxt"];
            user1.Mail = form["EmailTxt"];
            user1.Password = form["PasswordTxt"];
            user1.Cellphone = Convert.ToInt32(form["PhoneTxt"]);

            params_values.Add("@Name", user1.Name);
            params_values.Add("@LastName", user1.LastName);
            params_values.Add("@Password", user1.Password);
            params_values.Add("@Mail", user1.Mail);
            params_values.Add("@Cellphone", user1.Cellphone);
            //send query and check result
            bool success = da.ExecuteSentenceParameterized("INSERT INTO [User](Name, LastName, Password, Mail, Cellphone) VALUES(@Name, @LastName, @Password, @Mail, @Cellphone)", params_values);

            

          

            return RedirectToAction("Index" , "Login");


            
        }

      
    }

}