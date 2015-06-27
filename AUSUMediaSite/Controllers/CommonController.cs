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
using System.Collections;

namespace AUSUMediaSite.Controllers
{
    public class CommonController : Controller
    {
        private dbmediaEntities db = new dbmediaEntities();

        //
        // GET: /Common/

        public ActionResult MediaCategoryList(int? page)
        {
            var query = from s in db.tbCommon
                        where s.Tag==1
                        select s;
            if (page == null)
                page = 1;

            query = query.OrderBy(s => s.ID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SeqNumber = (pageNumber - 1) * pageSize + 1;
            return View(query.ToPagedList(pageNumber, pageSize));
        }

       

        //
        // GET: /Common/Create

        public ActionResult MediaCategoryCreate()
        {
            return View();
        }

        //
        // POST: /Common/Create

        [HttpPost]
        public ActionResult MediaCategoryCreate(tbCommon m)
        {
            if (ModelState.IsValid)
            {
                m.Tag = 1;
                db.tbCommon.Add(m);
                db.SaveChanges();
                return RedirectToAction("MediaCategoryList");
            }

            return View(m);
        }

        //
        // GET: /Common/Edit/5

        public ActionResult MediaCategoryEdit(int id = 0)
        {
            tbCommon tbcommon = db.tbCommon.Find(id);
            if (tbcommon == null)
            {
                return HttpNotFound();
            }
            return View(tbcommon);
        }

        //
        // POST: /Common/Edit/5

        [HttpPost]
        public ActionResult MediaCategoryEdit(tbCommon tbcommon)
        {
            if (ModelState.IsValid)
            {
                tbcommon.Tag = 1;
                db.Entry(tbcommon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MediaCategoryList");
            }
            return View(tbcommon);
        }



        //
        // GET: /Common/

        public ActionResult EqTypeList(int? page)
        {
            var query = from s in db.tbCommon
                        where s.Tag == 2
                        select s;
            if (page == null)
                page = 1;

            query = query.OrderBy(s => s.ID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SeqNumber = (pageNumber - 1) * pageSize + 1;
            return View(query.ToPagedList(pageNumber, pageSize));
        }



        //
        // GET: /Common/Create

        public ActionResult EqTypeCreate()
        {
            return View();
        }

        //
        // POST: /Common/Create

        [HttpPost]
        public ActionResult EqTypeCreate(tbCommon m)
        {
            if (ModelState.IsValid)
            {
                m.Tag = 2;
                db.tbCommon.Add(m);
                db.SaveChanges();
                return RedirectToAction("EqTypeList");
            }

            return View(m);
        }

        //
        // GET: /Common/Edit/5

        public ActionResult EqTypeEdit(int id = 0)
        {
            tbCommon tbcommon = db.tbCommon.Find(id);
            if (tbcommon == null)
            {
                return HttpNotFound();
            }
            return View(tbcommon);
        }

        //
        // POST: /Common/Edit/5

        [HttpPost]
        public ActionResult EqTypeEdit(tbCommon tbcommon)
        {
            if (ModelState.IsValid)
            {
                tbcommon.Tag = 2;
                db.Entry(tbcommon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EqTypeList");
            }
            return View(tbcommon);
        }





        //
        // GET: /Common/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tbCommon tbcommon = db.tbCommon.Find(id);
            if (tbcommon == null)
            {
                return HttpNotFound();
            }
            return View(tbcommon);
        }

        //
        // POST: /Common/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            tbCommon tbcommon = db.tbCommon.Find(id);
            db.tbCommon.Remove(tbcommon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //用于编辑
        public JsonResult GetCommons()
        {
            int tag = int.Parse(Request.QueryString["tag"].ToString());
            ArrayList arr = new ArrayList();
            var query2 = from s in db.tbCommon
                         where s.Tag == tag
                         select s;
            arr.Add(new { Value = "", Display = "" });
            foreach (tbCommon s in query2.ToList())
            {
                arr.Add(new { Value = s.ID.ToString(), Display = s.Name });
            }

            return Json( new JsonResult { Data = arr },JsonRequestBehavior.AllowGet);
        }

        //用于查询

        public JsonResult GetAllCommons()
        {
            int tag = int.Parse(Request.QueryString["tag"].ToString());
            ArrayList arr = new ArrayList();
            var query2 = from s in db.tbCommon
                         where s.Tag == tag
                         select s;
            arr.Add(new { Value = "", Display = "所有" });
            foreach (tbCommon s in query2.ToList())
            {
                arr.Add(new { Value = s.ID.ToString(), Display = s.Name });
            }

            return new JsonResult { Data = arr };
        }



       
    }
}