using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chirst_Temple_Kid_Finder.Models;

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

        // GET: Check-In
        public ActionResult CheckIn(string presetName)
        {
            return View();
        }

        //POST: Check-In
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CheckIn(string roomNumber,string childCode)
        {
            CodeTable ct = db.CodeTables.First(x => x.ChildCode == childCode);
            ct.Room_Number = roomNumber;
            db.Entry(ct).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Teacher");
        }

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

        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,ChildCode")] CodeAssignTable codeAssignTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codeAssignTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(codeAssignTable);
        }

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
    }
}
