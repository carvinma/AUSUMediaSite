using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using AUSUMediaSite.EF;

namespace AUSUMediaSite.Controllers
{
    public class VideoController : Controller
    {
        private dbmediaEntities db = new dbmediaEntities();
        public ActionResult VideoList()
        {
            return View();
        }
        public ActionResult VideoCategory(int? page)
        {
            var query = from s in db.tbMediaInfo
                        select s;
            query = query.OrderByDescending(s => s.UpdateTime);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
        }
      
    }
}