using kindergarden.Models;
using kindergarden.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace kindergarden.Controllers
{
    public class LoginController : Controller
    {
        readonly KinderModelContext db = new KinderModelContext();

        // GET: Login
        public ActionResult Login(bool? result, bool? logged)
        {
            if (result == false)
            {
                ViewBag.Message = "Bitte überprüfen Sie Ihre Anmeldeinformationen.";
            }
            if (logged == true)
            {
                ViewBag.Message = "Sie sind bereits angemeldet!";
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            var model = db.person.Include("Addresses").Where(p => p.Email == email && p.Pass == pass && p.IsActive == true).FirstOrDefault();
            if (model != null)
            {
                //false ise login olunmus demektir.
                bool result = CheckUserLogedIn(model.Id);
                if (result == true)
                {
                    if (model.Addresses.Count() == 0)
                    {
                        return RedirectToAction("PersonDetail", new { personId = model.Id });
                    }
                    if (model.Pass == "1234")
                    {
                        return RedirectToAction("SetPassword", "Login", new { email = model.Email });
                    }
                    if (model.IsAdmin == true)
                    {
                        Session["name"] = model.Name;
                        Session["lastName"] = model.LastName;
                        Session["role"] = "admin";
                        Session["userId"] = model.Id;
                        Session["schoolId"] = model.SchoolId;
                        CreateLoggingRecord(model, "Leitung");
                        //CreateActiveUser(model);
                        return RedirectToAction("AdminPage", "Home");
                    }
                    //master kullanicisi demektir
                    else if (model.IsMaster == true)
                    {
                        Session["name"] = model.Name;
                        Session["lastName"] = model.LastName;
                        Session["role"] = "master";
                        Session["userId"] = model.Id;
                        return RedirectToAction("MasterPage", "Home");
                    }
                    //veli olan kullanici demektir
                    else if (model.IsTeacher == true)
                    {
                        Session["name"] = model.Name;
                        Session["lastName"] = model.LastName;
                        Session["role"] = "teacher";
                        Session["userId"] = model.Id;
                        Session["schoolId"] = model.SchoolId;
                        CreateLoggingRecord(model, "Erzieher");
                        //CreateActiveUser(model);
                        return RedirectToAction("TeacherPage", "Home");
                    }
                    else
                    {
                        Session["name"] = model.Name;
                        Session["lastName"] = model.LastName;
                        Session["role"] = "parent";
                        Session["userId"] = model.Id;
                        Session["schoolId"] = model.SchoolId;
                        CreateLoggingRecord(model, "Eltern");
                        //CreateActiveUser(model);
                        return RedirectToAction("ParentPage", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", new { logged = true });
                }
            }
            return RedirectToAction("Login", new { result = false });
        }

        [NonAction]
        private bool CheckUserLogedIn(int id)
        {
            bool result = false;
            var model = db.ActiveUser.Where(p => p.UserId == id).FirstOrDefault();

            if (model == null)
                result = true;
            else
                result = false;
            return result;
        }

        [NonAction]
        private void CreateActiveUser(Person model)
        {
            ActiveUser auser = new ActiveUser();
            auser.LastName = model.LastName;
            auser.Name = model.Name;
            auser.UserId = model.Id;
            auser.SchoolId = model.School.SchoolId;
            db.ActiveUser.Add(auser);
            db.SaveChanges();
        }

        [NonAction]
        private void CreateLoggingRecord(Person person, string role)
        {
            LogRecord record = new LogRecord()
            {
                Login = person.Name + " " + person.LastName,
                LoginTime = DateTime.Now,
                SchoolName = person.School.Name,
                Role = role
            };
            db.LogRecords.Add(record);
            db.SaveChanges();
        }

        public ActionResult SetPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                ViewBag.Email = email;
            }
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SetPassword(string email, string password)
        {
            var model = db.person.Where(p => p.Email == email).FirstOrDefault();
            model.Pass = password;
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        //Tüm session nesnelerini kapatiyorum.
        public ActionResult Logout()
        {
            //int userId = Convert.ToInt32(Session["userId"]);
            Session.Clear();
            Session.Abandon();
            //if (userId !=0)
            //{
            //    RemoveActiveUser(userId);
            //}
            return RedirectToAction("Login", "Login");
        }

        [NonAction]
        private void RemoveActiveUser(int? userId)
        {
            var model = db.ActiveUser.Where(p => p.UserId == userId).FirstOrDefault();
            if (model != null)
            {
                db.ActiveUser.Remove(model);
                db.SaveChanges();
            }

        }

        public ActionResult Thanks(string person)
        {
            ViewBag.Person = person;
            return View();
        }

        public ActionResult SchoolRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SchoolRegister(AdminRegisterVM model, string register)
        {
            if (register == "true")
            {
                model.School.IsPayWillSchool = true;
            }
            else
            {
                model.School.IsPayWillSchool = false;
            }
            if (ModelState.IsValid)
            {
                int SchoolId = CrateSchool(model.School);
                CreatePerson(model.Person, SchoolId);
                string name = model.Person.Name + " " + model.Person.LastName;
                SendEmailToMaster(model);
                return RedirectToAction("Thanks", new { person = name });
            }
            return View();
        }

        [NonAction]
        public void SendEmailToMaster(AdminRegisterVM data)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("formrgmbh@gmail.com");
            ePosta.To.Add("bykingpin@gmail.com");
            ePosta.To.Add("heuseyin1986@gmail.com");
            ePosta.Subject = "Kita Yeni okul kaydoldu";
            ePosta.Body = @"okul Adi: " + data.School.Name + "<br> okulmu ödeyecek: " + data.School.IsPayWillSchool + "<br> okul telefonu: <a href='tel:" + data.School.Phone + " 'target='_blank'>" + data.School.Phone + "</a> <br><hr> admin adi: " + data.Person.Name + "<br> admin soyadi: " + data.Person.LastName + "<br> Admin telefonu <a href='tel:" + data.Person.Gsm + " 'target='_blank'>" + data.Person.Gsm + "</a>";
            ePosta.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            NetworkCredential networkCredential = new NetworkCredential("formrgmbh@gmail.com", "Formr123?");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            try { smtp.Send(ePosta); }
            catch (Exception) { }
        }

        
        public void SendEmailToUser(string email)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("info@kita365.de");
            ePosta.To.Add("bykingpin@gmail.com");
            ePosta.To.Add("heuseyin1986@gmail.com");
            ePosta.Subject = "Willkommen bei Kita365";
            ePosta.Body = @"<h3>Ihr Account wurde Freigeschaltet, wir wünschen Ihnen viel Spaß bei der Benutzung.</h3>";
            ePosta.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            NetworkCredential networkCredential = new NetworkCredential("info@kita365.de", "Kita123?");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Host = "smtp.ionos.de";
            smtp.EnableSsl = true;
            object userState = ePosta;
            try { smtp.Send(ePosta); }
            catch (Exception e) {
                string hata = e.Message;
            }
        }



        [NonAction]
        private void CreatePerson(Person person, int schoolId)
        {
            person.IsAdmin = true;
            person.SchoolId = schoolId;
            person.IsMaster = false;
            person.IsActive = false;
            db.person.Add(person);
            db.SaveChanges();
        }

        [NonAction]
        private int CrateSchool(School school)
        {
            school.CreatedDate = DateTime.Now;
            school.schoolGuid = Guid.NewGuid().ToString();
            db.School.Add(school);
            db.SaveChanges();
            int schoolId = school.SchoolId;
            return schoolId;
        }

        public ActionResult TeacherRegister()
        {
            return View();
        }

        public ActionResult AdminRegister()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AdminRegister(Person model, string GUID)
        {
            int schoolId = CheckSchoolGUID(GUID);
            if (schoolId != 0)
            {
                model.CreatedDate = DateTime.Now;
                model.IsActive = false;
                model.IsAdmin = true;
                model.SchoolId = schoolId;
                db.person.Add(model);
                db.SaveChanges();
                SendEmailToUser(model.Email);
                return RedirectToAction("Thanks", new { person = model.Name + " " + model.LastName });
            }
            ModelState.AddModelError("GUID", "Bitte prüfen Aktivierungsschlüssel");
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult TeacherRegister(Person model, string GUID)
        {
            int schoolId = CheckSchoolGUID(GUID);
            if (schoolId != 0)
            {
                model.CreatedDate = DateTime.Now;
                model.IsActive = false;
                model.IsTeacher = true;
                model.SchoolId = schoolId;
                db.person.Add(model);
                db.SaveChanges();
                SendEmailToUser(model.Email);
                return RedirectToAction("Thanks", new { person = model.Name + " " + model.LastName });
            }
            ModelState.AddModelError("GUID", "Bitte prüfen Aktivierungsschlüssel");
            return View(model);
        }

        public ActionResult ParentRegister()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ParentRegister(Person model, string GUID)
        {
            int schoolId = CheckSchoolGUID(GUID);
            if (schoolId != 0)
            {
                model.CreatedDate = DateTime.Now;
                model.IsActive = false;
                model.IsParent = true;
                model.SchoolId = schoolId;
                db.person.Add(model);
                db.SaveChanges();
                SendEmailToUser(model.Email);
                return RedirectToAction("Thanks", new { person = model.Name + "" + model.LastName });
            }
            ModelState.AddModelError("GUID", "Bitte prüfen Aktivierungsschlüssel");
            return View(model);
        }

        public ActionResult PersonDetail(int personId)
        {
            ViewBag.PersonId = personId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonDetail(Address address)
        {
            if (ModelState.IsValid)
            {
                db.Addresses.Add(address);
                db.SaveChanges();
                return View("Login");
            }
            return View(address);
        }



        [NonAction]
        private int CheckSchoolGUID(string guid)
        {
            var record = db.School.Where(p => p.schoolGuid == guid).FirstOrDefault();
            if (record != null)
                return record.SchoolId;
            else
                return 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}