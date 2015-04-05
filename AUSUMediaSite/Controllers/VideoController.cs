using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AUSUMediaSite.Controllers
{
    public class VideoController : Controller
    {
        public ActionResult VideoList()
        {
            return View();
        }
        public ActionResult VideoCategory()
        {
            return View();
        }
      
    }
}