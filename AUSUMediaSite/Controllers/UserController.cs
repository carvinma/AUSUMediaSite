using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AUSUMediaSite.EF;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;

namespace AUSUMediaSite.Controllers
{
    public class UserController : Controller
    {
        private dbmediaEntities db = new dbmediaEntities();

        //
        // GET: /Common/

        public ActionResult List(int? page)
        {
            var query = from s in db.tbUser
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

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Common/Create

        [HttpPost]
        public ActionResult Create(tbUser m)
        {
            if (ModelState.IsValid)
            {
                m.Password=FormsAuthentication.HashPasswordForStoringInConfigFile("123456", "SHA1");
                db.tbUser.Add(m);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(m);
        }

        //
        // GET: /Common/Edit/5

        public ActionResult Edit()
        {
            int id = int.Parse(Session["UserId"].ToString());
            tbUser m = db.tbUser.Find(id);
            if (m == null)
            {
                return HttpNotFound();
            }
            return View(m);
        }

        //
        // POST: /Common/Edit/5

        [HttpPost]
        public ActionResult Edit(tbUser tbuser)
        {
            if (ModelState.IsValid)
            {
                tbUser m = db.tbUser.Find(tbuser.ID);
                m.UserName = tbuser.UserName;
                m.RealName = tbuser.RealName;
                m.Phone = tbuser.Phone;
                m.Email = tbuser.Email;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return View(tbuser);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}