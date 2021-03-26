using kindergarden.Filters;
using kindergarden.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kindergarden.Controllers
{
    [LoginCheck]
    [RoleCheck( RoleName ="master")]
    public class MasterUserController : Controller
    {
        readonly KinderModelContext db = new KinderModelContext();

        //Adminleri listeleyen metot
        public ActionResult ListAdmin()
        {
            var model = db.person.Include("School").Where(z => z.IsAdmin == true && z.IsActive == true).ToList();
            return View(model);
        }

        public ActionResult ListSchool()
        {
            var model = db.School.ToList();
            return View(model);
        }

        public ActionResult SchoolDetail(int schoolId)
        {
            var model = db.School.Find(schoolId);
            ViewBag.Create = model.CreatedDate;
            ViewBag.Manager = db.person.Where(p => p.SchoolId == schoolId && p.IsAdmin == true).Count();
            ViewBag.Teacher = db.person.Where(p => p.SchoolId == schoolId && p.IsTeacher == true).Count();
            ViewBag.Parent = db.person.Where(p => p.SchoolId == schoolId && p.IsParent == true).Count();

            return View(model);
        }

        public ActionResult DeleteAdmin(int Id)
        {
            var model = db.person.Find(Id);
            db.person.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ListAdmin");
        }

        public ActionResult ListLogin(DateTime? date)
        {

            List<LogRecord> model = new List<LogRecord>();
            if (date != null)
            {
                model = db.LogRecords.Where(p => p.LoginTime.Year == date.Value.Year && p.LoginTime.Month == date.Value.Month && p.LoginTime.Day == date.Value.Day).ToList();
            }
            else
            {
                model = db.LogRecords.Take(100).ToList();
            }
            return View(model);

        }

        [NonAction]
        private void DeletePictureFile(string filePath)
        {
            if (System.IO.File.Exists(Server.MapPath(filePath)))
            { System.IO.File.Delete(Server.MapPath(filePath)); }
        }

        public ActionResult PendingApprove()
        {
            var model = db.person.Where(P => P.IsAdmin == true && P.IsActive == false).ToList();
            return View(model);
        }

        public ActionResult ApproveAdmin(int Id)
        {
            var model = db.person.Find(Id);
            model.IsActive = true;
            model.IsAdmin = true;
            CreateSchoolFiles((int)model.SchoolId);
            db.SaveChanges();
            return RedirectToAction("PendingApprove");
        }

        private void CreateSchoolFiles(int okulId)
        {
            string schoolFolder = "~/SchoolMediaFiles/School" + okulId;
            string schoolGallery = schoolFolder + "/GalleryFiles";
            string schoolActivity = schoolFolder + "/ActivityFiles";
            string schoolNews = schoolFolder + "/NewsFiles";
            string schoolMessage = schoolFolder + "/MessageFiles";
            Directory.CreateDirectory(Server.MapPath(schoolFolder));
            Directory.CreateDirectory(Server.MapPath(schoolGallery));
            Directory.CreateDirectory(Server.MapPath(schoolActivity));
            Directory.CreateDirectory(Server.MapPath(schoolNews));
            Directory.CreateDirectory(Server.MapPath(schoolMessage));
        }

        public ActionResult RejectAdmin(int Id)
        {
            var model = db.person.Find(Id);
            DeleteSchool((int)model.SchoolId);
            db.person.Remove(model);
            db.SaveChanges();
            return RedirectToAction("PendingApprove");
        }

        private void DeleteSchool(int okulId)
        {
            var model = db.School.Find(okulId);
            db.School.Remove(model);
            db.SaveChanges();
        }

        public ActionResult DeleteSchoolWithRecords(int schoolId)
        {
            DeletePersonBySchoolId(schoolId);
            DeleteActivitiesBySchoolId(schoolId);
            DeleteCalendarActivitiesBySchoolId(schoolId);
            DeleteNewsBySchoolId(schoolId);
            DeleteGaleriesBySchoolId(schoolId);
            DeleteMediaFilesOnServer(schoolId);
            DeleteSchool(schoolId);
            return RedirectToAction("ListSchool");
        }

        private void DeleteMediaFilesOnServer(int schoolId)
        {
            string root = "~/SchoolMediaFiles/School" + schoolId;
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath(root));
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dirx in dir.EnumerateDirectories())
            {
                dirx.Delete(true);
            }
            dir.Delete(true);
        }

        private void DeleteGaleriesBySchoolId(int schoolId)
        {
            var model = db.gallery.Where(p => p.SchoolId == schoolId).ToList();
            foreach (var item in model)
            {
                DeleteGalleryImagesByGalleryId(item.Id);
            }
            db.gallery.RemoveRange(model);
            db.SaveChanges();
        }

        private void DeleteGalleryImagesByGalleryId(int galleryId)
        {
            var model = db.galleryimage.Where(p => p.galleryId == galleryId).ToList();
            db.galleryimage.RemoveRange(model);
            db.SaveChanges();
        }

        private void DeleteNewsBySchoolId(int schoolId)
        {
            var model = db.news.Where(p => p.SchoolId == schoolId).ToList();
            db.news.RemoveRange(model);
            db.SaveChanges();
        }

        private void DeleteCalendarActivitiesBySchoolId(int schoolId)
        {
            var model = db.CalendarActivities.Where(z => z.SchoolId == schoolId).ToList();
            db.CalendarActivities.RemoveRange(model);
            db.SaveChanges();
        }

        private void DeletePersonBySchoolId(int schoolId)
        {
            var persons = db.person.Where(p => p.SchoolId == schoolId).ToList();
            foreach (var item in persons)
            {
                DeleteMessagesByPersonId(item.Id);
            }
            foreach (var item in persons)
            {
                DeletePersonAddressByPersonId(item.Id);
            }
            foreach (var item in persons)
            {
                DeletePersonMessageByPersonId(item.Id);
            }
            db.person.RemoveRange(persons);
            db.SaveChanges();
        }

        private void DeleteMessagesByPersonId(int id)
        {
            var messages = db.Message.Where(p => p.OwnerId == id).ToList();
            db.Message.RemoveRange(messages);
            db.SaveChanges();
        }

        private void DeletePersonMessageByPersonId(int personId)
        {
            var personmessages = db.personMessage.Where(p => p.PersonId == personId).ToList();
            db.personMessage.RemoveRange(personmessages);
            db.SaveChanges();
        }

        private void DeletePersonAddressByPersonId(int personId)
        {
            var personAddress = db.Addresses.Where(p => p.PersonId == personId).ToList();
            db.Addresses.RemoveRange(personAddress);
            db.SaveChanges();
        }

        private void DeleteActivitiesBySchoolId(int schoolId)
        {
            var activities = db.activities.Where(p => p.SchoolId == schoolId).ToList();
            foreach (var item in activities)
            {
                DeleteActivityMessageByActivityId(item.Id);
            }
            foreach (var item in activities)
            {
                DeleteActivityPicturesByActivityId(item.Id);
            }
            db.activities.RemoveRange(activities);
            db.SaveChanges();
        }

        private void DeleteActivityPicturesByActivityId(int activityId)
        {
            var model = db.activitiesPicture.Where(p => p.activitiesId == activityId).ToList();
            db.activitiesPicture.RemoveRange(model);
            db.SaveChanges();
        }

        private void DeleteActivityMessageByActivityId(int activitiyId)
        {
            var activityMessages = db.activitiesMessage.Where(p => p.activitiesId == activitiyId).ToList();
            foreach (var item in activityMessages)
            {
                DeleteAnswerActivityMessagesByActivityMessageId(item.Id);
            }
            db.activitiesMessage.RemoveRange(activityMessages);
            db.SaveChanges();
        }

        private void DeleteAnswerActivityMessagesByActivityMessageId(int activityMessageId)
        {
            var model = db.AnswerActivityMessages.Where(p => p.activitiesMessageId == activityMessageId).ToList();
            db.AnswerActivityMessages.RemoveRange(model);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Admin ekleyen metot
        //public ActionResult CreateAdmin()
        //{
        //    return View();
        //}

        ////Admin kullanicisi olusturan metot
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateAdmin(Person adminPerson)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Person aPerson = new Person();
        //        aPerson.Gsm = adminPerson.Gsm;
        //        aPerson.IsActive = true;
        //        aPerson.IsAdmin = true;
        //        aPerson.IsMaster = false;
        //        aPerson.LastName = adminPerson.LastName;
        //        aPerson.Name = adminPerson.Name;
        //        aPerson.Pass = adminPerson.Pass;
        //        aPerson.Email = adminPerson.Email;
        //        db.person.Add(aPerson);
        //        db.SaveChanges();
        //        return RedirectToAction("MasterPage", "Home");
        //    }

        //    return View();
        //}

        ////Tüm reklamlari listeler
        //public ActionResult ListAdvertiesement()
        //{
        //    var model = db.advertisement.ToList();
        //    return View(model);
        //}

        ////Yeni reklam olusturma ekranini acar.
        //public ActionResult CreateAdvertiesement()
        //{ return View(); }

        ////Yeni reklami kayit etmeye yarayan metot.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateAdvertiesement(advertisement adv, HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        advertisement adver = new advertisement();
        //        adver.companyName = adv.companyName;
        //        adver.finishDate = adv.finishDate;
        //        adver.isActive = true;
        //        adver.mapsLink = adv.mapsLink;
        //        adver.startDate = adv.startDate;
        //        adver.url = adv.url;
        //        db.advertisement.Add(adver);
        //        db.SaveChanges();
        //        int id = adver.Id;
        //        SaveAdvertiesementFile(adv.companyName.Replace(" ", ""), id, file);
        //        return RedirectToAction("ListAdvertiesement");
        //    }
        //    return View();
        //}

        ////Reklam resmini kayit eder.
        //private void SaveAdvertiesementFile(string comName, int advId, HttpPostedFileBase file)
        //{
        //    string root = "~/AdverFiles/";
        //    string fileName = comName + advId;
        //    file.SaveAs(Server.MapPath(root + fileName + ".jpg"));
        //    var model = db.advertisement.Find(advId);
        //    model.filePath = root + fileName + ".jpg";
        //    db.SaveChanges();
        //}

        ////Reklami yayindan kaldiran metot.
        //public ActionResult DeleteAdvertiesement(int Id)
        //{
        //    var model = db.advertisement.Find(Id);
        //    if (model.filePath != null)
        //    { DeletePictureFile(model.filePath); }
        //    db.advertisement.Remove(model);
        //    db.SaveChanges();
        //    return RedirectToAction("ListAdvertiesement");
        //}

        //Reklamin resmini sistemden siler.
    }
}