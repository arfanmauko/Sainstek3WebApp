using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;

namespace SainsTekWepApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PengumumanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Pengumuman
        public ActionResult Index()
        {
            return View(db.Pengumuman.ToList());
        }

        // GET: Admin/Pengumuman/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pengumuman pengumuman = db.Pengumuman.Find(id);

            if (pengumuman == null)
            {
                return HttpNotFound();
            }
            return View(pengumuman);
        }

        // GET: Admin/Pengumuman/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pengumuman/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content")] Pengumuman pengumuman)
        {
            if (ModelState.IsValid)
            {
                pengumuman.DateCreated = DateTime.Now;
                db.Pengumuman.Add(pengumuman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pengumuman);
        }

        // GET: Admin/Pengumuman/Edit/5
        public ActionResult Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Pengumuman pengumuman = db.Pengumuman.Find(id);
            if (pengumuman == null)
            {
                return HttpNotFound();
            }
            return View(pengumuman);
        }

        // POST: Admin/Pengumuman/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,DateCreated,DatePublished")] Pengumuman pengumuman)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pengumuman).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pengumuman);
        }

        // GET: Admin/Pengumuman/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pengumuman pengumuman = db.Pengumuman.Find(id);
            if (pengumuman == null)
            {
                return HttpNotFound();
            }
            return View(pengumuman);
        }

        // POST: Admin/Pengumuman/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Pengumuman pengumuman = db.Pengumuman.Find(id);
            db.Pengumuman.Remove(pengumuman);
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
