using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AUSUMediaSite.EF;
using System.Web.Security;
namespace AUSUMediaSite.Controllers
{
    public class HomeController : Controller
    {
        dbmediaEntities db = new dbmediaEntities();
        //
        // GET: /Home/

        public ActionResult Index()
        {           
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }
        public ActionResult LogOn(string error)
        {            
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.error = error;
            }
            return View();
        }
        [HttpPost]
        public ActionResult LogOn(tbUser model, string returnUrl)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("UserName", "用户名不能为空！");
                return View();
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "密码不能为空！");
                return View();
            }
            string error = string.Empty;
            string secretPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "SHA1");
            var result = from s in db.tbUser
                         where s.UserName == model.UserName && s.Password == secretPassword
                         select s;

            if (result.ToList().Count > 0)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    Session["UserID"] = result.ToList()[0].ID;
                    Session["UserName"] = result.ToList()[0].UserName;
                    if (string.IsNullOrEmpty(result.ToList()[0].RealName))
                    {
                        Session["RealName"] = result.ToList()[0].UserName;
                    }
                    else
                    {
                        Session["RealName"] = result.ToList()[0].RealName;
                    }
                    
                    int userid = result.ToList()[0].ID;
                    int roleid=int.Parse(result.ToList()[0].RoleID.ToString());
                    Session["RoleID"] = result.ToList()[0].RoleID;
                   
                    return RedirectToAction("Index", "Home");
                   
                    
                }
            }
            else
            {
                error="用户名或密码错误";
            }


            // If we got this far, something failed, redisplay form
            return RedirectToAction("LogOn", new { error = error });
        }

        public ActionResult Quit()
        {
            Session.Clear();
            return RedirectToAction("Logon");
        }

    }
}
