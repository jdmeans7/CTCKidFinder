using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chirst_Temple_Kid_Finder.Models;
using System.Net.Mail;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Chirst_Temple_Kid_Finder.Controllers
{
    public class TeacherController : Controller
    {
        private dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3 db = new dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3();

        // GET: Teacher
        [Authorize (Roles = "Teacher")]
        public ActionResult Index(Preset paramPList)
        {
            return View();
        }

        [Authorize(Roles = "Teacher")]
        // GET: Check-In
        public ActionResult CheckIn(string presetName)
        {
            return View();
        }

        
        //POST: Check-In
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CheckIn(string roomNumber, string childCode)
        {
            bool codeExist = db.CodeTables.AsNoTracking().Any(x => x.ChildCode == childCode);
            if (codeExist)
            {
                CodeTable ct = db.CodeTables.First(x => x.ChildCode == childCode);
                ct.Room_Number = roomNumber;

                /*
                db.CodeTables.Attach(ct);
                var entry = db.Entry(ct);
                entry.Property(x => x.Room_Number).IsModified = true;
                */
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("/Teacher");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Child code not found");
                return View();
            }
        }
        
       
        [Authorize(Roles = "Teacher")]
        // GET: Teacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeAssignTable codeAssignTable = db.CodeAssignTables.Find(id);
            if (codeAssignTable == null)
            {
                return HttpNotFound();
            }
            return View(codeAssignTable);
        }

        [Authorize(Roles = "Teacher")]
        // GET: Teacher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,Email,ChildCode")] CodeAssignTable codeAssignTable)
        {
            if (ModelState.IsValid)
            {
                db.CodeAssignTables.Add(codeAssignTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(codeAssignTable);
        }

        [Authorize(Roles = "Teacher")]
        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeTable codeTable = db.CodeTables.Find(id);
            if (codeTable == null)
            {
                return HttpNotFound();
            }
            return View(codeTable);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Teacher")]
        public ActionResult Edit([Bind(Include = "Id,ChildCode,Room_Number")] CodeTable codeTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codeTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(codeTable);
        }

        [Authorize(Roles ="Teacher")]
        // GET: Teacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeAssignTable codeAssignTable = db.CodeAssignTables.Find(id);
            if (codeAssignTable == null)
            {
                return HttpNotFound();
            }
            return View(codeAssignTable);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            CodeAssignTable codeAssignTable = db.CodeAssignTables.Find(id);
            db.CodeAssignTables.Remove(codeAssignTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles ="Teacher")]
        // GET for Teacher/Message
        public ActionResult Message()
        {
            return View();
        }

        [Authorize(Roles ="Teacher")]
        // POST for Teacher/Message
        public ActionResult SubmitMessage(MessageViewModel model)
        {
            String code = model.KidCode;

            CodeAssignTable KidCodeLookup = db.CodeAssignTables.FirstOrDefault(x => x.ChildCode == code);

            if (KidCodeLookup == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Child Code, try again.");

                return View("Message", model);
            }

            String To = KidCodeLookup.Email;
            String Subject = model.Subject;
            String From = "This message is from " + User.Identity.Name + " at Christ Temple Church." + Environment.NewLine;

            String Message = From + Environment.NewLine + model.Message;

            EmailConstruct(To, Subject, Message);

            return RedirectToAction("Index", "Home");
        }

        // Creates and Send Email
        public void EmailConstruct(String To, String Subject, String Message)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress("mu.az.camp@gmail.com");
            mail.Subject = Subject;
            mail.Body = Message;
            //mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential
                 ("teacher.christtemplechurch@gmail.com", "CHRISTtemplechurch");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception e) { }
        }
    }
}
