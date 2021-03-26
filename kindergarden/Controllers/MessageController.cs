using kindergarden.Filters;
using kindergarden.Models;
using kindergarden.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kindergarden.Controllers
{
    [LoginCheck]
    public class MessageController : Controller
    {
        private readonly KinderModelContext db = new KinderModelContext();
        // GET: Message
        public ActionResult MyMessage()
        {
            int userId = Convert.ToInt32(Session["userId"]);
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            string role = Convert.ToString(Session["role"]);
            List<PersonMessageVM> model = GetPersonMessageVM(userId);
            
            ViewBag.SendPerson = GetSendPerson(role, schoolId);
            return View(model);

        }

        private List<PersonMessageVM> GetPersonMessageVM(int userId)
        {
            List<PersonMessageVM> pmVMList = new List<PersonMessageVM>();
            string personFullName = string.Empty;
            var model = db.personMessage.Where(p => p.PersonId == userId || p.SenderId == userId).ToList();
            foreach (var item in model)
            {
                PersonMessageVM pmVm = new PersonMessageVM();
                pmVm.PersonMessageId = item.Id;
                pmVm.IsUnread = item.IsUnRead;

                if (item.PersonId == userId)
                {
                    int personId = (int)item.SenderId;
                    personFullName =  GetPersonFullName(personId);
                }
                else
                {
                    int personId = item.PersonId;
                    personFullName = GetPersonFullName(personId);
                }
                pmVm.User = personFullName;
                pmVMList.Add(pmVm);
            }
            return pmVMList;
        }

        private string GetPersonFullName(int personId)
        {
            string fullName = string.Empty;
            var model = db.person.Find(personId);
            fullName = model.Name + " " + model.LastName;
            return fullName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(int PersonId, Message message, HttpPostedFileBase file)
        {
            int personMessageId = 0;
            int SenderId = Convert.ToInt32(Session["userId"]); //Mesaji atan kisi
            int SchoolId = Convert.ToInt32(Session["schoolId"]);
            var CheckPersonMessage = db.personMessage.Where(p => (p.SenderId == SenderId && p.PersonId == PersonId) || (p.SenderId == PersonId && p.PersonId == SenderId)).FirstOrDefault();
            if (CheckPersonMessage == null)
            {
                personMessageId = CreatePersonMessage(SenderId, PersonId);
                CreateMessage(message, personMessageId, file, SchoolId,SenderId);
            }
            else
            {
                var existingPersonMessageId = CheckPersonMessage.Id;
                CreateMessage(message, existingPersonMessageId, file, SchoolId,SenderId);
                MarkUnReadPersonMessage(existingPersonMessageId);
            }
            return RedirectToAction("MyMessage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OnlySendMessage(int PersonId, Message message, HttpPostedFileBase file)
        {
            int schoolId = Convert.ToInt32(Session["schoolId"]);
            int userId = Convert.ToInt32(Session["userId"]);
            message.OwnerId = userId;
            message.IsActive = true;
            message.LeaveDate = DateTime.Now;
            message.sender = GetMessageSender(userId);
            db.Message.Add(message);
            db.SaveChanges();
            int messageId = message.MessageId;
            if (file != null)
            {
                SaveMessageFile(messageId, schoolId, file);
            }
            MarkReadPersonMessage(message.PersonMessageId);
            return RedirectToAction("MyMessage");
        }

        private void MarkReadPersonMessage(int personMessageId)
        {
            var model = db.personMessage.Find(personMessageId);
            model.IsUnRead = false;
            db.SaveChanges();
        }

        public ActionResult DetailMessage(int personMessageId)
        {
            var pm = db.personMessage.Find(personMessageId);

            //ViewBag.Person = "Simdilik bos";//pm.Person.Name + " " + pm.Person.LastName;
            //ViewBag.PersonPhone = "Simdilik bos";// pm.Sender.Gsm;
            var model = db.Message.Include("personMessage").Where(p => p.PersonMessageId == personMessageId).ToList();
            return View(model);
        }



        private void CreateMessage(Message message, int personMessageId, HttpPostedFileBase file,int schoolId,int senderId)
        {
            Message nmessage = new Message();
            nmessage.OwnerId = senderId;
            nmessage.IsActive = true;
            nmessage.Text = message.Text;
            nmessage.LeaveDate = DateTime.Now;
            nmessage.PersonMessageId = personMessageId;
            nmessage.sender = GetMessageSender(senderId);
            db.Message.Add(nmessage);
            db.SaveChanges();
            int messageId = nmessage.MessageId;
            if (file != null)
            {
                SaveMessageFile(messageId, schoolId, file);
            }

        }

        private string GetMessageSender(int senderId)
        {
            var sender = db.person.Find(senderId).Email;
            return sender;
        }

        private void MarkUnReadPersonMessage(int Id)
        {
            var model = db.personMessage.Find(Id);
            model.IsUnRead = true;
            db.SaveChanges();
        }

        private void SaveMessageFile(int messageId, int SchoolId, HttpPostedFileBase item)
        {
            string fileExtension = System.IO.Path.GetExtension(item.FileName);
            string root = "~/SchoolMediaFiles/School" + SchoolId + "/MessageFiles/";
            string fileName = "Message" + messageId + fileExtension;
            item.SaveAs(Server.MapPath(root + fileName));
            var model = db.Message.Find(messageId);
            model.FileName = fileName;
            model.FullPath = root;
            db.SaveChanges();
        }

        private int CreatePersonMessage(int senderId, int personId)
        {
            personMessage npersonMessage = new personMessage();
            npersonMessage.SenderId = senderId;
            npersonMessage.PersonId = personId;
            npersonMessage.IsUnRead = true;
            db.personMessage.Add(npersonMessage);
            db.SaveChanges();
            int personMessageId = npersonMessage.Id;
            return personMessageId;
        }

        private SelectList GetSendPerson(string role, int schoolId)
        {
            List<Person> personList = new List<Person>();
            switch (role)
            {
                case "admin":
                    personList = db.person.Where(p => p.SchoolId == schoolId && p.IsActive == true && (p.IsParent == true || p.IsTeacher == true)).ToList();
                    break;
                case "teacher":
                    personList = db.person.Where(p => p.SchoolId == schoolId && p.IsActive == true && (p.IsAdmin == true || p.IsParent == true)).ToList();
                    break;
                case "parent":
                    personList = db.person.Where(p => p.SchoolId == schoolId && p.IsActive == true && (p.IsAdmin == true || p.IsTeacher == true)).ToList();
                    break;
                default:
                    break;
            }
            var model = new SelectList(personList, "Id", "PerInfo");
            return model;
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