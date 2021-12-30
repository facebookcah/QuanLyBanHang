using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuongTrinh.Models;
using PagedList;


namespace ChuongTrinh.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        // này là controller main client nha là màn Home á
        QuanLyBanHangDB db = new QuanLyBanHangDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main( int ?page,int? madm)
        {
            
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            var products = db.SanPhams.Select(p => p).ToList();
            var categories = db.DanhMucs.ToList();
            Session["categories"] = categories;
            if (madm > 0)
            {
                var products1 =products.Where(i=>i.MaDanhMuc==madm).ToList();
                return View(products1.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                
                return View(products.ToPagedList(pageNumber, pageSize));
            }
           
        }
        public ActionResult Cart(int? masp)
        {
            var product = db.SanPhams.Find(masp);

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }
            var isExist = db.TaiKhoans.Any(i => i.TenDangNhap.Equals(account.UserName) && i.MatKhau.Equals(account.Password));
            if (!isExist)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác";
                return View(account);
            }
            else
            {
                var acc = db.TaiKhoans.Where(i=>i.TenDangNhap.Equals(account.UserName)).FirstOrDefault();
                if (acc.MaQuyen != 1)
                {
                    ViewBag.Error = "Tài khoản không có quyền truy cập !!";
                    return View(account);
                }
                else {
                    Session["UserName"] = acc.TenDangNhap;
                    return RedirectToAction("Main");
                }
            }

            return RedirectToAction("Main");
        }


    }
}