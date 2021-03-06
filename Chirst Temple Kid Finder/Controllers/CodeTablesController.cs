﻿using System;
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
    public class CodeTablesController : Controller
    {
        private dbcaa9cff9bf624b1ebcf9a8120126a40eEntities2 db = new dbcaa9cff9bf624b1ebcf9a8120126a40eEntities2();

        // GET: CodeTables
        public ActionResult Index()
        {
            return View(db.CodeTables.ToList());
        }

        /*
        // GET: CodeTables/Details/5
        public ActionResult Details(int? id)
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
        */
        // GET: CodeTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CodeTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChildCode,Room_Number")] CodeTable codeTable)
        {
            if (ModelState.IsValid)
            {
                int id = db.CodeTables.Count();
                CodeTable entryAdd = new CodeTable
                {
                    Id = id,
                    ChildCode = codeTable.ChildCode,
                    Room_Number = codeTable.Room_Number
                };
                db.CodeTables.Add(entryAdd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(codeTable);
        }

        // GET: CodeTables/Edit/5
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

        // POST: CodeTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string roomNumber)
        {
            CodeTable ct = db.CodeTables.Find(id);
            ct.Room_Number = roomNumber;
            db.Entry(ct).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CodeTables/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: CodeTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CodeTable codeTable = db.CodeTables.Find(id);
            db.CodeTables.Remove(codeTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Generate()
        {
            Random rand = new Random();
            int code = rand.Next(10000,100000); //Need to pad string with 0s
            int id = db.CodeTables.Count();
            string codeS = code.ToString();
            CodeTable entryAdd = new CodeTable
            {
                Id = id,
                ChildCode = codeS
            };
            db.CodeTables.Add(entryAdd);
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
