using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using AUSUMediaSite.EF;

namespace AUSUMediaSite.Controllers
{
    public class EqInfoController : Controller
    {
        private dbmediaEntities db = new dbmediaEntities();

        //
        // GET: /EqInfo/

        public ActionResult Index(int? page)
        {
            var query = from s in db.tbEqInfo
                        select s;
            if (page == null)
                page = 1;

            query = query.OrderByDescending(s => s.sn);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SeqNumber = (pageNumber - 1) * pageSize+1;
            return View(query.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /EqInfo/Details/5

        public ActionResult Details(int id = 0)
        {
            tbEqInfo tbeqinfo = db.tbEqInfo.Find(id);
            if (tbeqinfo == null)
            {
                return HttpNotFound();
            }
            return View(tbeqinfo);
        }

        //
        // GET: /EqInfo/Create

        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(db.tbCity, "ID", "CityName");
            return View();
        }

        //
        // POST: /EqInfo/Create

        [HttpPost]
        public ActionResult Create(tbEqInfo tbeqinfo)
        {
            if (ModelState.IsValid)
            {
                db.tbEqInfo.Add(tbeqinfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityID = new SelectList(db.tbCity, "ID", "CityName", tbeqinfo.CityID);
            return View(tbeqinfo);
        }

        //
        // GET: /EqInfo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            tbEqInfo tbeqinfo = db.tbEqInfo.Find(id);
            if (tbeqinfo == null)
            {
                return HttpNotFound();
            }
            return View(tbeqinfo);
        }

        //
        // POST: /EqInfo/Edit/5

        [HttpPost]
        public ActionResult Edit(tbEqInfo tbeqinfo)
        {
            try
            {
                tbEqInfo m = db.tbEqInfo.Find(tbeqinfo.ID);
                m.EqTypeID = tbeqinfo.EqTypeID;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tbeqinfo);
            }
        }

        //
        // GET: /EqInfo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tbEqInfo tbeqinfo = db.tbEqInfo.Find(id);
            if (tbeqinfo == null)
            {
                return HttpNotFound();
            }
            return View(tbeqinfo);
        }

        //
        // POST: /EqInfo/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tbEqInfo tbeqinfo = db.tbEqInfo.Find(id);
            db.tbEqInfo.Remove(tbeqinfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}