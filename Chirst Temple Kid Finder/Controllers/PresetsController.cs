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
    public class PresetsController : Controller
    {
        private dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3 db = new dbcaa9cff9bf624b1ebcf9a8120126a40eEntities3();

        // GET: Presets
        public ActionResult Index()
        {
            return View(db.Presets.ToList());
        }

        // GET: Presets/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preset preset = db.Presets.Find(id);
            if (preset == null)
            {
                return HttpNotFound();
            }
            return View(preset);
        }

        // GET: Presets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Presets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Tables")] Preset preset)
        {
            if (ModelState.IsValid)
            {
                db.Presets.Add(preset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(preset);
        }

        // GET: Presets/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preset preset = db.Presets.Find(id);
            if (preset == null)
            {
                return HttpNotFound();
            }
            return View(preset);
        }

        // POST: Presets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name")] Preset preset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(preset);
        }

        // GET: Presets/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preset preset = db.Presets.Find(id);
            if (preset == null)
            {
                return HttpNotFound();
            }
            return View(preset);
        }

        // POST: Presets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Preset preset = db.Presets.Find(id);
            db.Presets.Remove(preset);
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
