using ChuongTrinh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ChuongTrinh.Controllers
{
    public class ProductController : Controller
    {
        private readonly QuanLyBanHangDB db = new QuanLyBanHangDB();
        // GET: Product
        public ActionResult Details(int? id)
        {
            var product = db.SanPhams.Find(id);
            return View(product);
        }

       
    }
}