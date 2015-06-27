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
using System.Collections;
using System.IO;

namespace AUSUMediaSite.Controllers
{
    public class EqMediaController : Controller
    {
        private dbmediaEntities db = new dbmediaEntities();

        //分配给设备播放视频
        public ActionResult AssignMediaList(string EqTypeID, int? page)
        {
            var query = from s in db.tbEqInfo
                        select s;
            if (page == null)
                page = 1;

            if (!string.IsNullOrEmpty(EqTypeID) && int.Parse(EqTypeID) > 0)
            {
                ViewBag.CurrentEqTypeID = EqTypeID;
                int v = int.Parse(EqTypeID);
                query = query.Where(s => s.EqTypeID == v);
            }

            query = query.OrderByDescending(s => s.sn);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SeqNumber = (pageNumber - 1) * pageSize + 1;
            ViewBag.EqTypeID = GetAllEqType();
            return View(query.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult SelectMediaList(string MediaCategoryID, string EqIds, int? page)
        {
            var query = from s in db.tbMediaInfo
                        select s;
            if (page == null)
                page = 1;
            if (!string.IsNullOrEmpty(MediaCategoryID) && int.Parse(MediaCategoryID) > 0)
            {
                ViewBag.CurrentMediaCategoryID = MediaCategoryID;
                int v = int.Parse(MediaCategoryID);
                query = query.Where(s => s.MediaCategoryID == v);
            }
            query = query.OrderByDescending(s => s.ID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.SeqNumber = (pageNumber - 1) * pageSize + 1;
            ViewBag.MediaCategoryID = GetAllMediaCategory();
            ViewBag.EqIds = EqIds;
            return View(query.ToPagedList(pageNumber, pageSize));
        }


        //某一个设备的播放视频列表
        public ActionResult EqOnlineMediaList(int? page, int id = 0)
        {
            var query = from s in db.tbEqMediaInfo
                        select s;
            if (id > 0)
            {
                query = from s in query
                        where s.EqID == id
                        select s;
            }
            if (page == null)
                page = 1;

            query = query.OrderBy(s => s.EqID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.id = id;
            ViewBag.SeqNumber = (pageNumber - 1) * pageSize + 1;
            ViewBag.EqTypeID = GetAllEqType();
            return View(query.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public JsonResult AddBatch(string EqIds, string MediaIds)
        {
            string[] arrEqId = EqIds.Split(',');
            string[] arrMediaId = MediaIds.Split(',');

            foreach (string s in arrEqId)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    int EqId = int.Parse(s);
                    foreach (string s2 in arrMediaId)
                    {
                        if (!string.IsNullOrEmpty(s2))
                        {
                            int MediaId = int.Parse(s2);
                            var query = from q in db.tbEqMediaInfo
                                        where q.EqID == EqId && q.MediaID == MediaId
                                        select q;
                            if (query.ToList().Count == 0)
                            {
                                tbEqMediaInfo m = new tbEqMediaInfo();
                                m.EqID = EqId;
                                m.MediaID = MediaId;
                                db.tbEqMediaInfo.Add(m);
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
            ArrayList arr = new ArrayList();
            arr.Add(1);

            return new JsonResult { Data = arr };
        }

        //
        // POST: /EqMedia/Delete/5]

        [HttpPost]
        public JsonResult DeleteBatch(string ids)
        {
            string[] arrId = ids.Split(',');
            foreach (string s in arrId)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    int id = int.Parse(s);
                    tbEqMediaInfo tbeqmediainfo = db.tbEqMediaInfo.Find(id);
                    db.tbEqMediaInfo.Remove(tbeqmediainfo);
                }
            }
            ArrayList arr = new ArrayList();
            arr.Add(1);
            db.SaveChanges();
            return new JsonResult { Data = arr };
        }

        public JsonResult GetMp4List()
        {
            string direcotry = Server.MapPath("../Content/Files");
            string[] list = Directory.GetFiles(direcotry);
            List<string> filesName = new List<string>();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].ToLower().Contains(".mp4"))
                {
                    filesName.Add(list[i].Substring(list[i].LastIndexOf(@"\") + 1));
                }
            }
            return new JsonResult { Data = filesName };
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public List<SelectListItem> GetAllMediaCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var query2 = from s in db.tbCommon
                         where s.Tag == 1
                         select s;
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "所有";
            items.Add(item);
            foreach (tbCommon s in query2.ToList())
            {
                item = new SelectListItem();
                item.Value = s.ID.ToString();
                item.Text = s.Name;
                items.Add(item);
            }
            return items;
        }


        public List<SelectListItem> GetAllEqType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var query2 = from s in db.tbCommon
                         where s.Tag == 2
                         select s;
            SelectListItem item = new SelectListItem();
            item.Value = "0";
            item.Text = "所有";
            items.Add(item);
            foreach (tbCommon s in query2.ToList())
            {
                item = new SelectListItem();
                item.Value = s.ID.ToString();
                item.Text = s.Name;
                items.Add(item);
            }
            return items;
        }
    }
}