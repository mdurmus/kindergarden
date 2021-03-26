using kindergarden.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kindergarden.Controllers
{
    [LoginCheck]
    [RoleCheck(RoleName ="teacher")]
    public class TeacherUserController : Controller
    {
      private readonly KinderModelContext db = new KinderModelContext();

        public ActionResult ListCalendarActivities()
        {
            return View();
        }
        public ActionResult ListCalendarAktivitet()
        {
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.CalendarActivities.Where(z => z.SchoolId == SchoolId).ToList();
            var secenek = new object();
            secenek = model.Select(k => new
            {
                id = k.Id,
                title = k.Text,
                start = k.StartDate.AddHours(2),
                end = k.FinishDate.AddHours(2),
            });
            return Json(secenek, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AllActivities()
        {
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.activities.Where(p => p.isActive == true && p.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult DetailActivity(int Id)
        {
            var model = db.activities.Where(p => p.Id == Id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentActivities(activitiesMessage activitiesMessage)
        {
            string owner = Convert.ToString(Session["name"]);
            activitiesMessage.isActive = true;
            activitiesMessage.messageDate = DateTime.Now;
            activitiesMessage.messageOwner = owner;
            db.activitiesMessage.Add(activitiesMessage);
            db.SaveChanges();
            return RedirectToAction("DetailActivity", new { Id = activitiesMessage.activitiesId });
        }
        public ActionResult AllNews()
        {
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.news.Where(p => p.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult NewsDetail(int Id)
        {
            var model = db.news.Find(Id);
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("AllNews");
        }
        public ActionResult ListGallery()
        {
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.gallery.Where(p => p.SchoolId == SchoolId).ToList();
            return View(model);
        }
        public ActionResult ShowGallery(int galleryId)
        {
            var model = db.galleryimage.Where(p => p.galleryId == galleryId).ToList();
            ViewBag.GalleryName = db.gallery.Find(galleryId).name;
            return View(model);
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