using ContactS.WEB.Models.Filters;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ContactS.WEB.Controllers
{
    public class HomeController : MyController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;

            new LanguageMang().SetLanguage(lang);

            return Redirect(returnUrl);
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}