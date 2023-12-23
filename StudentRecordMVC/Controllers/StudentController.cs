using Newtonsoft.Json;
using StudentRecordMVC.DB;
using StudentRecordMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentRecordMVC.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {

        // GET: Student
        [AllowAnonymous]
        public ActionResult signup() { return View(); }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult signup(adminmodel obj)
        {
            HttpClient client = new HttpClient();
            var data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var xj = client.PostAsync("http://localhost:63562/api/signup", content).Result;
            var xxj = xj.Content.ReadAsStringAsync().Result;
            var j = JsonConvert.DeserializeObject<string>(xxj);
            if(j=="This emailid is already registered")
            {
                ViewData["0"] = "This emailid is already registered. ENter another id";
                return View();
            }
            TempData["po"] = "Succesful";
            return RedirectToAction("login");
        }

            [AllowAnonymous]
        public ActionResult ForgPass() { return View(); }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgPass(adminmodel obj) 
        {
            HttpClient client = new HttpClient();
            var data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var xj = client.PostAsync("http://localhost:63562/api/auth", content).Result;
            var xxj = xj.Content.ReadAsStringAsync().Result;
            var j = JsonConvert.DeserializeObject<string>(xxj);
            if(j== "ID is not found")
            {
                ViewData["ii"] = "ID is not found";
                return View(); }
            else if(j== "Incorrect Email ID")
            {
                ViewData["ip"] = "Incorrect Email ID";
                return View();

            }
            TempData["it"] = "Password CHanged";

            return RedirectToAction("login");
        }

        public ActionResult logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("login");
        }
        [AllowAnonymous]
        public ActionResult login()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult login(adminmodel obj)
        {
            
            HttpClient client = new HttpClient();
            var data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var xj = client.PostAsync("http://localhost:63562/api/auth",content).Result;
            var xxj = xj.Content.ReadAsStringAsync().Result;
            var j = JsonConvert.DeserializeObject<string>(xxj);

            if (j == "Invalid E-mailID")
            {
                TempData["xx"] = "Enter Valid Email ID";
                return View();
            }
            else
            {
                if (j == "Login Succesfull")
                {
                    FormsAuthentication.SetAuthCookie(obj.Email, false);
                    Session["email"] = obj.Email;

                    return RedirectToAction("showrecords");
                }
                TempData["x"] = "Incorrect Passsword";
            }
            return View();


        }
        //public ActionResult login(adminmodel obj)
        //{
        //    StudentEntities x = new StudentEntities();
        //    var j = x.admins.Where(m => m.Email == obj.Email).FirstOrDefault();
        //    if (j == null)
        //    {
        //        TempData["xx"] = "Enter Valid Email ID";
        //        return View();
        //    }
        //    if (j.Email == obj.Email && j.Password == obj.Password)
        //    {
        //        FormsAuthentication.SetAuthCookie(j.Email, false);
        //        Session["email"] = j.Email;

        //        return RedirectToAction("showrecords");
        //    }
        //    TempData["x"] = "Incorrect Passsword";
        //    return View();


        //}
        //public ActionResult showrecords()
        //{
        //    StudentEntities x = new StudentEntities();
        //    var s=x.Student_Records.ToList();
        //    List<StudentRecordModel> model=new List<StudentRecordModel>();
        //    foreach (var record in s)
        //    {
        //        model.Add(new StudentRecordModel()
        //        {
        //            Id = record.Id,
        //            Name = record.Name,
        //            DOB=record.DOB,
        //            Age=record.Age,
        //            Class=record.Class,
        //            Email=record.Email,
        //            Result=record.Result,
        //        });
        //    }


        //    return View(model);
        //}
        public ActionResult showrecords()
        {
            HttpClient at = new HttpClient();
            var data=at.GetAsync("http://localhost:63562/api/Check").Result;
            var jsondara=data.Content.ReadAsStringAsync().Result;
            var finaldata=JsonConvert.DeserializeObject<List<StudentRecordModel>>(jsondara);


            return View(finaldata);
        }
        public ActionResult Addrecords()
        {
            StudentRecordModel obj=new StudentRecordModel();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Addrecords(StudentRecordModel obj,string Java,string Python,string Cpp)
        {
            obj.IsCpp=(Cpp=="true")?true:false;
            obj.IsJava = (Java == "true") ? true : false;
            obj.IsPython = (Python == "true") ? true : false;

            HttpClient client = new HttpClient();
            var data=JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data,System.Text.Encoding.UTF8,"application/json");
            client.PostAsync("http://localhost:63562/api/xix", content);

            return RedirectToAction("showrecords");

        }
        //public ActionResult Addrecords(StudentRecordModel obj)
        //{
        //    StudentEntities x = new StudentEntities();
        //    Student_Records xx = new Student_Records();
        //    xx.Id = obj.Id;
        //    xx.Name = obj.Name;
        //    xx.DOB = obj.DOB;
        //    xx.Age = obj.Age;
        //    xx.Class = obj.Class;
        //    xx.Email = obj.Email;
        //    xx.Result = obj.Result;
        //    if (obj.Id == 0)
        //    {
        //        x.Student_Records.Add(xx);
        //        x.SaveChanges();
        //    }
        //    else
        //    {
        //        x.Entry(xx).State = System.Data.Entity.EntityState.Modified;
        //        x.SaveChanges();
        //    }


        //    return RedirectToAction("showrecords");

        //}
        public ActionResult Updaterecords(int Id, string Name, System.DateTime? DOB, int? Age, string Email, int? Class, string Result)
        {
            HttpClient client = new HttpClient();
            var data = client.GetAsync("http://localhost:63562/api/Check/"+Id).Result;
            var jsondara = data.Content.ReadAsStringAsync().Result;
            var finaldata = JsonConvert.DeserializeObject<StudentRecordModel>(jsondara);


            return View("Addrecords", finaldata);
        }
        //public ActionResult Updaterecords(int Id, string Name, System.DateTime? DOB, int? Age, string Email, int? Class, string Result)
        //{
        //    StudentRecordModel md = new StudentRecordModel();
        //    md.Id = Id;
        //    md.Name = Name;
        //    md.DOB = DOB;
        //    md.Age = Age;
        //    md.Class = Class;
        //    md.Email = Email;
        //    md.Result = Result;
        //    return View("Addrecords", md);
        //}
        public ActionResult Deleterecords(int Id)
        {
            HttpClient client = new HttpClient();
            client.DeleteAsync("http://localhost:63562/api/Check/"+Id);
           
            return RedirectToAction("showrecords");
        }
        //public ActionResult Deleterecords(int Id)
        //{
        //    StudentEntities x = new StudentEntities();
        //    var t = x.Student_Records.Where(a => a.Id == Id).First();
        //    x.Student_Records.Remove(t);
        //    x.SaveChanges();
        //    return RedirectToAction("showrecords");
        //}
    }
}