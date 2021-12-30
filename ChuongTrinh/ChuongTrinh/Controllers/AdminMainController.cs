using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuongTrinh.Controllers
{
    public class AdminMainController : Controller
    {
        // GET: AdminMain
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminMain()
        {
            return View();
        }    
    }
}