using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AUSUMediaSite.EF;
using System.Collections;
using System.IO;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
namespace AUSUMediaSite.Controllers
{
	
	/// <summary>
	/// Description of Android.
	/// </summary>
    public class AndroidController : Controller
	{
        dbmediaEntities db = new dbmediaEntities();

        public JsonResult GetServerData(string sn, string req)
        {
            if (req == "1")
            {
                return RegtoServer(sn);
            }
            else if (req == "2")
            {
                return GetEqMediaList(sn);
            }
            else
            {
                return GetServerDateTime(sn);
            }
        }


        public JsonResult GetEqMediaList(string sn)
        {
            ArrayList arr = new ArrayList();
            var query = from a in db.tbMediaInfo
                        join b in db.tbEqMediaInfo on a.ID equals b.MediaID
                        join c in db.tbEqInfo on b.EqID equals c.ID
                        where c.sn==sn
                        select a;
            foreach(tbMediaInfo m in query.ToList())
            {
                arr.Add(new { MediaName = m.MediaName, SubTitle = m.SubTitle, MediaUrl=m.MediaUrl });
            }
            return Json( new JsonResult { Data = arr },JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServerDateTime(string sn)
        {
            ArrayList arr = new ArrayList();
            arr.Add(new {ServerDateTime=DateTime.Now.ToString() });
            return Json(new JsonResult { Data = arr }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegtoServer(string sn)
        {
            ArrayList arr = new ArrayList();
            try
            {
                var query = from s in db.tbEqInfo
                            where s.sn == sn
                            select s;
                if (query.ToList().Count == 0)
                {
                    tbEqInfo eq = new tbEqInfo();
                    eq.sn = sn;
                    db.tbEqInfo.Add(eq);
                    db.SaveChanges();
                }
                arr.Add(new { result = "1" });
                return Json(new JsonResult { Data = arr }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                arr.Add(new {result="0" });
                return Json(new JsonResult { Data = arr }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult DownloadMedia(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var path = Server.MapPath("~/content/files/") + fileName;
                FileStream fs = new FileStream(path, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";
                //解决文件名乱码问题            
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(fileName));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
            return new EmptyResult();

        }
	}

    
}