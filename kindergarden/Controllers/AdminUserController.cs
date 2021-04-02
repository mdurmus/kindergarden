
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using kindergarden.Filters;
using kindergarden.Models;

namespace kindergarden.Controllers
{
    [LoginCheck]
    [RoleCheck(RoleName ="admin")]
    public class AdminUserController : Controller
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
        public ActionResult ListCalendarActivitiy(DateTime? date)
        {
            int schoolID = Convert.ToInt32(Session["schoolId"]);
            List<CalendarActivity> model = new List<CalendarActivity>();
            if (date != null)
            {
                model = db.CalendarActivities.Where(p => p.SchoolId == schoolID && (p.StartDate.Year == date.Value.Year && p.StartDate.Month == date.Value.Month && p.StartDate.Day == date.Value.Day)).ToList();
            }
            else
            {
                model = db.CalendarActivities.Where(p => p.SchoolId == schoolID).ToList();
            }
            return View(model);
        }
        public ActionResult CreateCalendarActivity()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateCalendarActivity(CalendarActivity calendarActivity)
        {
            int schoolID = Convert.ToInt32(Session["schoolId"]);
            if (ModelState.IsValid)
            {
                calendarActivity.SchoolId = schoolID;
                db.CalendarActivities.Add(calendarActivity);
                db.SaveChanges();
                return RedirectToAction("ListCalendarActivitiy");
            }
            return View(calendarActivity);
        }
        public ActionResult DeleteCalendarActivity(int Id)
        {
            var model = db.CalendarActivities.Find(Id);
            db.CalendarActivities.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListCalendarActivitiy");
        }
        public ActionResult PendingApproveParent()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.person.Where(p => p.SchoolId == schoolId && p.IsActive == false && p.IsParent == true).ToList();
            return View(model);
        }
        public ActionResult ChangeStatusParent(int parentId)
        {
            var model = db.person.Find(parentId);
            if (model.IsActive == true)
            {
                model.IsActive = false;
            }
            else
            {
                model.IsActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("ListParents");
        }
        public ActionResult ApproveParent(int parentId)
        {
            var model = db.person.Find(parentId);
            model.IsActive = true;
            db.SaveChanges();
            SendAcceptNotificationEmailToUser(model.Email);
            return RedirectToAction("PendingApproveParent");
        }
        public ActionResult DenyParent(int parentId)
        {
            var model = db.person.Find(parentId);
            db.person.Remove(model);
            db.SaveChanges();
            return RedirectToAction("PendingApproveParent");
        }
        public ActionResult PendingApproveTeacher()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.person.Where(p => p.SchoolId == schoolId && p.IsActive == false && p.IsTeacher == true).ToList();
            return View(model);
        }
        public ActionResult ChangeStatusTeacher(int teacherId)
        {
            var model = db.person.Find(teacherId);
            if (model.IsActive == true)
            {
                model.IsActive = false;
            }
            else
            {
                model.IsActive = true;
            }
            db.SaveChanges();
            return RedirectToAction("ListTeacher");
        }
        public ActionResult ApproveTeacher(int teacherId)
        {
            var model = db.person.Find(teacherId);
            model.IsActive = true;
            db.SaveChanges();
            SendAcceptNotificationEmailToUser(model.Email);
            return RedirectToAction("PendingApproveTeacher");
        }
        public ActionResult DenyTeacher(int teacherId)
        {
            var model = db.person.Find(teacherId);
            db.person.Remove(model);
            db.SaveChanges();
            return RedirectToAction("PendingApproveTeacher");
        }
        public ActionResult ListActivities()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.activities.Where(p => p.SchoolId == schoolId).ToList();
            return View(model);
        }
        public ActionResult CreateActivity()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActivity(activities activity, List<HttpPostedFileBase> pictures)
        {
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            if (ModelState.IsValid)
            {
                activities act = new activities();
                act.adminText = activity.adminText;
                act.finishDate = activity.finishDate;
                act.fullText = activity.fullText;
                act.isActive = true;
                act.location = activity.location;
                act.miniText = activity.miniText;
                act.startDate = activity.startDate;
                act.subject = activity.subject;
                act.SchoolId = SchoolId;
                db.activities.Add(act);
                db.SaveChanges();
                int activityId = act.Id;
                if (pictures.Count > 1)
                {
                    foreach (var item in pictures)
                    {
                        SaveActivitiyPictures(activityId, SchoolId, item);
                    }
                }
                return RedirectToAction("ListActivities");
            }
            return View();
        }
        public ActionResult DetailActivity(int id)
        {
            var model = db.activities.Find(id);
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("ListActivities");
        }
        public ActionResult DeleteAnswerActivityMessage(int Id, int activityId)
        {
            var model = db.AnswerActivityMessages.Find(Id);
            db.AnswerActivityMessages.Remove(model);
            db.SaveChanges();
            return RedirectToAction("DetailActivity", new { id = activityId });
        }
        public ActionResult CommentAnswer(int commentId, int activitiyId)
        {
            ViewBag.ActivityId = activitiyId;
            ViewBag.activityMessageId = commentId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentAnswer(answerActivityMessage aam)
        {
            aam.isActive = true;
            aam.messageDate = DateTime.Now;
            aam.messageOwner = Convert.ToString(Session["name"]);
            db.AnswerActivityMessages.Add(aam);
            db.SaveChanges();
            return RedirectToAction("DetailActivity", new { Id = aam.activitiesId });
        }
        public ActionResult DeleteActivityMessage(int commentId, int activitiyId)
        {
            var model = db.activitiesMessage.Find(commentId);
            db.activitiesMessage.Remove(model);
            db.SaveChanges();
            return RedirectToAction("DetailActivity", new { id = activitiyId });
        }
        public ActionResult EditActivity(int id)
        {
            var model = db.activities.Find(id);
            if (model != null)
            {
                return View(model);
            }
            return RedirectToAction("ListActivities");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditActivity(activities act)
        {
            if (ModelState.IsValid)
            {
                var model = db.activities.Find(act.Id);
                model.adminText = act.adminText;
                model.finishDate = act.finishDate;
                model.fullText = act.fullText;
                model.location = act.location;
                model.miniText = act.miniText;
                model.startDate = act.startDate;
                model.subject = act.subject;
                model.SchoolId = act.SchoolId;
                db.SaveChanges();
                return RedirectToAction("ListActivities");
            }
            return View();
        }
        public ActionResult DeleteActivity(int Id)
        {
            var model = db.activities.Find(Id);
            if (model != null)
            {
                DeleteActivitiyPictures(Id);
                DeleteActivityMessage(Id);
                db.activities.Remove(model);
                db.SaveChanges();
                return RedirectToAction("ListActivities");
            }
            return View();
        }
        public ActionResult ChangeActivityStatus(int Id)
        {
            var model = db.activities.Find(Id);
            if (model != null)
            {
                if (model.isActive == true)
                {
                    model.isActive = false;
                }
                else
                {
                    model.isActive = true;
                }
                db.SaveChanges();
            }
            return RedirectToAction("ListActivities");
        }
        public ActionResult ListNews()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.news.Where(p => p.SchoolId == schoolId).ToList();
            return View(model);
        }
        public ActionResult CreateNews()
        {
            ViewBag.Priority = new SelectList(Enum.GetValues(typeof(Priority)));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNews(news news, HttpPostedFileBase file)
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            if (ModelState.IsValid)
            {
                news newss = new news();
                newss.newsText = news.newsText;
                newss.subject = news.subject;
                newss.priority = news.priority;
                newss.url = news.url;
                newss.SchoolId = schoolId;
                db.news.Add(newss);
                db.SaveChanges();
                if (file != null)
                {
                    SaveNewsFile(newss.Id, schoolId, file);
                }
                return RedirectToAction("ListNews");
            }
            return View();
        }
        public ActionResult DeleteNews(int Id)
        {
            var model = db.news.Find(Id);
            if (model != null)
            {
                DeleteNewFileOnServer(model.filePath);
                db.news.Remove(model);
                db.SaveChanges();
                return RedirectToAction("ListNews");
            }
            return View();
        }
        public ActionResult ListParents()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.person.Where(p => p.IsParent == true && p.SchoolId == schoolId && p.IsActive == true).ToList();
            return View(model);
        }
        public ActionResult CreateParents()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateParents(Person per)
        {
            if (ModelState.IsValid)
            {
                Person newper = new Person();
                //newper.Gsm = per.Gsm;
                newper.IsActive = true;
                newper.IsParent = true;
                newper.IsAdmin = false;
                newper.IsMaster = false;
                newper.IsTeacher = false;
                newper.LastName = per.LastName;
                newper.Name = per.Name;
                newper.Pass = per.Pass;
                newper.Email = per.Email;
                newper.SchoolId = Convert.ToInt32(Session["schoolId"]);

                db.person.Add(newper);
                db.SaveChanges();
                return RedirectToAction("ListParents");
            }
            return View();
        }
        public ActionResult DeleteParent(int Id)
        {
            var model = db.person.Find(Id);
            if (model != null)
            {
                db.person.Remove(model);
                db.SaveChanges();
                return RedirectToAction("ListParents");
            }
            return View();
        }
        public ActionResult ListTeacher()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.person.Where(p => p.SchoolId == schoolId && p.IsTeacher == true).ToList();
            return View(model);
        }
        public ActionResult CreateTeacher()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateTeacher(Person per)
        {
            if (ModelState.IsValid)
            {
                Person newper = new Person();
                //newper.Gsm = per.Gsm;
                newper.IsActive = true;
                newper.IsParent = false;
                newper.IsAdmin = false;
                newper.IsMaster = false;
                newper.IsTeacher = true;
                newper.LastName = per.LastName;
                newper.Name = per.Name;
                newper.Pass = per.Pass;
                newper.Email = per.Email;
                newper.SchoolId = Convert.ToInt32(Session["schoolId"]);
                db.person.Add(newper);
                db.SaveChanges();
                return RedirectToAction("ListTeacher");
            }
            return View();
        }
        public ActionResult DeleteTeacher(int Id)
        {
            var model = db.person.Find(Id);
            if (model != null)
            {
                db.person.Remove(model);
                db.SaveChanges();
                return RedirectToAction("ListTeacher");
            }
            return View();
        }
        public ActionResult ResetParentPassword(int parentId)
        {
            var model = db.person.Find(parentId);
            model.Pass = "1234";
            db.SaveChanges();
            return RedirectToAction("ListParents");
        }
        public ActionResult ResetTeacherPassword(int parentId)
        {
            var model = db.person.Find(parentId);
            model.Pass = "1234";
            db.SaveChanges();
            return RedirectToAction("ListTeacher");
        }
        public ActionResult ListGallery()
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            var model = db.gallery.Where(p => p.SchoolId == schoolId).ToList();
            return View(model);
        }
        public ActionResult ShowGallery(int galleryId)
        {
            var model = db.galleryimage.Where(p => p.galleryId == galleryId).ToList();
            ViewBag.GalleryName = db.gallery.Find(galleryId).name;
            return View(model);
        }
        public ActionResult CreateGallery()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGallery(gallery galllery, List<HttpPostedFileBase> pictures)
        {
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            if (ModelState.IsValid)
            {
                gallery gal = new gallery();
                gal.name = galllery.name;
                gal.SchoolId = SchoolId;
                db.gallery.Add(gal);
                db.SaveChanges();
                int galleryId = gal.Id;
                string folderName = CreateGalleryFolderOnServer(galllery.name, SchoolId);
                for (int i = 0; i < pictures.Count(); i++)
                {
                    SaveGalleryImagesOnServer(pictures[i], folderName, galleryId, i, SchoolId);
                }
                return RedirectToAction("ListGallery");
            }
            return View();
        }
        public ActionResult DeleteGallery(int galleryId)
        {
            var model = db.gallery.Find(galleryId);
            DeleteGallerFilesOnServer(model.name, model.SchoolId);
            DeleteGalleryFilesOnDatabase(model.Id);
            db.gallery.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListGallery");

        }
        [NonAction]
        private void DeleteGalleryFilesOnDatabase(int Id)
        {
            var model = db.galleryimage.Where(p => p.galleryId == Id).ToList();
            db.galleryimage.RemoveRange(model);
            db.SaveChanges();
        }
        [NonAction]
        private void DeleteGallerFilesOnServer(string name, int SchoolId)
        {
            string folderName = string.Empty;
            folderName = name.Replace(" ", "");
            string root = "~/SchoolMediaFiles/School" + SchoolId + "/GalleryFiles/" + folderName;
            if (System.IO.Directory.Exists(Server.MapPath(root)))
            {
                string[] files = System.IO.Directory.GetFiles(Server.MapPath(root), "*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }
                Directory.Delete(Server.MapPath(root));
            }
        }
        [NonAction]
        private void SaveGalleryImagesOnServer(HttpPostedFileBase picture, string folderName, int galleryId, int count, int SchoolId)
        {
            string root = "~/SchoolMediaFiles/School" + SchoolId + "/GalleryFiles/" + folderName;
            string fileName = "/Picture" + galleryId + "_" + count + ".jpg";
            picture.SaveAs(Server.MapPath(root + fileName));
            galleryImage galim = new galleryImage();
            galim.filePath = root + fileName;
            galim.galleryId = galleryId;
            db.galleryimage.Add(galim);
            db.SaveChanges();
        }
        [NonAction]
        private string CreateGalleryFolderOnServer(string name, int SchoolId)
        {

            string stringFolderName = name.Replace(" ", "");
            string filePath = "~/SchoolMediaFiles/School" + SchoolId + "/GalleryFiles/" + stringFolderName;

            if (!System.IO.Directory.Exists(Server.MapPath(filePath)))
            { System.IO.Directory.CreateDirectory(Server.MapPath(filePath)); }

            return stringFolderName;
        }
        [NonAction]
        private void SaveActivitiyPictures(int activityId, int SchoolId, HttpPostedFileBase item)
        {
            Random sayi = new Random();
            string root = "~/SchoolMediaFiles/School" + SchoolId + "/ActivityFiles/";
            string fileName = "Activity" + activityId + "_" + sayi.Next().ToString() + ".jpg";
            item.SaveAs(Server.MapPath(root + fileName));
            activitiesPicture ap = new activitiesPicture();
            ap.activitiesId = activityId;
            ap.filePath = root + fileName;
            ap.isActive = true;
            db.activitiesPicture.Add(ap);
            db.SaveChanges();
        }
        [NonAction]
        private void DeleteActivityMessage(int Id)
        {
            var model = db.activitiesMessage.Where(p => p.activitiesId == Id).ToList();
            db.activitiesMessage.RemoveRange(model);
            db.SaveChanges();
        }
        [NonAction]
        private void DeleteActivitiyPictures(int Id)
        {
            var model = db.activitiesPicture.Where(p => p.activitiesId == Id).ToList();
            foreach (var item in model)
            {
                DeleteActivityPictureFileOnServer(item.filePath);
            }
            db.activitiesPicture.RemoveRange(model);
            db.SaveChanges();
        }
        [NonAction]
        private void DeleteActivityPictureFileOnServer(string filePath)
        {
            if (System.IO.File.Exists(Server.MapPath(filePath)))
            { System.IO.File.Delete(Server.MapPath(filePath)); }
        }
        [NonAction]
        private void SaveNewsFile(int newsid, int SchoolId, HttpPostedFileBase item)
        {
            string[] ext = item.FileName.Split('.');
            string root = "~/SchoolMediaFiles/School" + SchoolId + "/NewsFiles/";
            string fileName = "News" + newsid + "." + ext[1];
            item.SaveAs(Server.MapPath(root + fileName));
            var model = db.news.Find(newsid);
            model.filePath = root + fileName;
            db.SaveChanges();
        }
        [NonAction]
        private void DeleteNewFileOnServer(string filePath)
        {
            if (System.IO.File.Exists(Server.MapPath(filePath)))
            { System.IO.File.Delete(Server.MapPath(filePath)); }
        }

        public void SendAcceptNotificationEmailToUser(string email)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("info@kita365.de");
            ePosta.To.Add(email);
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
            catch (Exception e)
            {
                string hata = e.Message;
            }
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