using kindergarden.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kindergarden.Controllers
{
    [LoginCheck]
    public class HomeController : Controller
    {
         readonly KinderModelContext db = new KinderModelContext();

        public ActionResult AdminPage()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            ViewBag.GUID = db.School.Find(schoolId).schoolGuid;
            ViewBag.Activities = db.activities.Where(p => p.isActive == true && p.SchoolId == schoolId).Count();
            ViewBag.Calendar =  db.CalendarActivities.Where(p => p.SchoolId == schoolId).Count();
            ViewBag.News = db.news.Where(p => p.SchoolId == schoolId).Count();
            var model = db.news.Where(p => p.priority == Priority.Prio0 && p.SchoolId == schoolId).ToList();
            return View(model);
        }

        public ActionResult MasterPage()
        {

            ViewBag.SchoolCount = db.School.Count();
            ViewBag.Parents = db.person.Where(p => p.IsActive == true && p.IsParent == true).Count();
            ViewBag.PayParent = CalculatePayParent();
            ViewBag.Activities = db.activities.Count();

            return View();
        }

        private int CalculatePayParent()
        {
            List<Person> veliler = new List<Person>();
            using (var context = new KinderModelContext())
            {
                var model = context.person.Where(p => p.IsActive == true && p.IsParent == true).ToList();
                foreach (var item in model)
                {
                    TimeSpan sonuc = DateTime.Now - item.CreatedDate;
                    if (sonuc.TotalDays >= 90)
                    {
                        veliler.Add(item);
                    }
                }
            }
            return veliler.Count;
        }

        public ActionResult ParentPage()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            int userId = Convert.ToInt32(Session["userId"]);
            ViewBag.Activities = db.activities.Where(p => p.isActive == true && p.SchoolId == schoolId).Count();
            ViewBag.Messages = db.personMessage.Where(p => p.PersonId == userId).Count();
            ViewBag.News = db.news.Where(p => p.SchoolId == schoolId).Count();
            var model = db.news.Where(p => p.priority == Priority.Prio0 && p.SchoolId == schoolId).Take(5).ToList();
            return View(model);
        }

        public ActionResult TeacherPage()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            int userId = Convert.ToInt32(Session["userId"]);
            ViewBag.Activities = db.activities.Where(p => p.isActive == true && p.SchoolId == schoolId).Count();
            ViewBag.Messages = db.personMessage.Where(p => p.PersonId == userId).Count();
            ViewBag.News = db.news.Where(p => p.SchoolId == schoolId).Count();
            var model = db.news.Where(p => p.priority == Priority.Prio0 && p.SchoolId == schoolId).Take(5).ToList();
            return View(model);
        }

        public ActionResult Chat()
        {
            ViewBag.name = Session["name"];
            return View();
        }
    }

}
