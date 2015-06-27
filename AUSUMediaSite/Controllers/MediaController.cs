using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using AUSUMediaSite.EF;
using System.Data;

namespace AUSUMediaSite.Controllers
{
    public class MediaController : Controller
    {
        private dbmediaEntities db = new dbmediaEntities();
        public ActionResult MediaList(int? page)
        {
            var query = from s in db.tbMediaInfo
                        select s;
            query = query.OrderByDescending(s => s.ID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SeqNumber = (pageNumber - 1) * pageSize + 1;
            return View(query.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Common/Create

        public ActionResult MediaCreate()
        {
            return View();
        }

        //
        // POST: /Common/Create

        [HttpPost]
        public ActionResult MediaCreate(tbMediaInfo m)
        {
            if (ModelState.IsValid)
            {
                db.tbMediaInfo.Add(m);
                db.SaveChanges();
                return RedirectToAction("MediaList");
            }

            return View(m);
        }



        public ActionResult MediaEdit(int id=0)
        {
            tbMediaInfo mediaInfo = new tbMediaInfo();
            if (id > 0)
            {
                mediaInfo=db.tbMediaInfo.First(p => p.ID == id);
            }
            
            return View(mediaInfo);
        }

        [HttpPost]
        public ActionResult MediaEdit(tbMediaInfo m)
        {
            if (ModelState.IsValid)
            {
                m.UpdateTime = DateTime.Now;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MediaList");
            }
            return View(m);
        }

        public void MediaDel(int id)
        {
            try
            {
                var e = db.tbMediaInfo.First(r => r.ID == id);
                db.tbMediaInfo.Remove(e);
                db.SaveChanges();                
            }
            catch { }
            Response.Redirect("~/Media/MediaList");
        }
    }
}